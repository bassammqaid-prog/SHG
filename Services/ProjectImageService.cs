using Al_Shami_ِAISS_.Models;
using Al_Shami_ِAISS_.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Al_Shami_ِAISS_.Services;

/// <summary>
/// Contains business rules for the project gallery feature.
/// The service validates the incoming upload, stores files on disk when necessary,
/// and delegates database work to the project image repository.
/// </summary>
public class ProjectImageService
{
    private const long MaxImageSizeInBytes = 5 * 1024 * 1024;
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".webp"
    };

    private readonly IProjectImageRepository _projectImageRepository;

    /// <summary>
    /// Initializes the service with the project image repository.
    /// </summary>
    /// <param name="projectImageRepository">Repository used for persistence.</param>
    public ProjectImageService(IProjectImageRepository projectImageRepository)
    {
        _projectImageRepository = projectImageRepository;
    }

    /// <summary>
    /// Retrieves all gallery images for a specific project.
    /// </summary>
    /// <param name="projectId">The project identifier.</param>
    /// <returns>A list of project gallery images.</returns>
    public async Task<IEnumerable<ProjectImage>> GetByProjectIdAsync(int projectId)
    {
        if (projectId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(projectId), "Project ID must be greater than zero.");
        }

        return await _projectImageRepository.GetByProjectIdAsync(projectId);
    }

    /// <summary>
    /// Retrieves a single project image by identifier.
    /// </summary>
    /// <param name="id">The project image identifier.</param>
    /// <returns>The image record if it exists; otherwise null.</returns>
    public async Task<ProjectImage?> GetByIdAsync(int id)
    {
        return await _projectImageRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// Adds a new project image using an uploaded file or remote URL.
    /// </summary>
    /// <param name="projectId">The parent project identifier.</param>
    /// <param name="imageFile">An uploaded image from the local machine.</param>
    /// <param name="imageUrl">A remote or relative image URL.</param>
    /// <param name="hostingEnvironment">The web host environment that exposes the web root.</param>
    /// <returns>The saved project image entity.</returns>
    public async Task<ProjectImage> AddAsync(int projectId, IFormFile? imageFile, string? imageUrl, IWebHostEnvironment hostingEnvironment)
    {
        if (projectId <= 0)
        {
            throw new InvalidOperationException("A valid project is required before saving a gallery image.");
        }

        if (imageFile is null && string.IsNullOrWhiteSpace(imageUrl))
        {
            throw new InvalidOperationException("Please upload an image file or provide an image URL.");
        }

        if (imageFile is not null)
        {
            ValidateImageFile(imageFile);
        }

        if (!string.IsNullOrWhiteSpace(imageUrl) && !IsAllowedImageSource(imageUrl))
        {
            throw new InvalidOperationException("Please provide a valid image URL.");
        }

        var storedImageUrl = await ResolveImageUrlAsync(imageFile, imageUrl, hostingEnvironment);

        var projectImage = new ProjectImage
        {
            ProjectId = projectId,
            ImageUrl = storedImageUrl,
            CreatedAt = DateTime.UtcNow
        };

        await _projectImageRepository.AddAsync(projectImage);
        await _projectImageRepository.SaveChangesAsync();

        return projectImage;
    }

    /// <summary>
    /// Replaces the image URL of an existing gallery image.
    /// This supports the 'replace image' requirement without forcing the user to delete and recreate the record manually.
    /// </summary>
    /// <param name="id">The image identifier.</param>
    /// <param name="imageFile">A new file to upload.</param>
    /// <param name="imageUrl">An alternative URL source.</param>
    /// <param name="hostingEnvironment">The web root used for local uploads.</param>
    public async Task ReplaceAsync(int id, IFormFile? imageFile, string? imageUrl, IWebHostEnvironment hostingEnvironment)
    {
        var existingImage = await _projectImageRepository.GetByIdAsync(id);
        if (existingImage is null)
        {
            throw new KeyNotFoundException($"Project image with id {id} was not found.");
        }

        var replacementUrl = await ResolveImageUrlAsync(imageFile, imageUrl, hostingEnvironment);
        existingImage.ImageUrl = replacementUrl;

        _projectImageRepository.Update(existingImage);
        await _projectImageRepository.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a gallery image record and removes the local file if it was stored in the application web root.
    /// </summary>
    /// <param name="id">The image identifier.</param>
    /// <param name="hostingEnvironment">The hosting environment used to locate uploaded files.</param>
    public async Task DeleteAsync(int id, IWebHostEnvironment hostingEnvironment)
    {
        var projectImage = await _projectImageRepository.GetByIdAsync(id);
        if (projectImage is null)
        {
            throw new KeyNotFoundException($"Project image with id {id} was not found.");
        }

        if (TryNormalizeLocalRelativePath(projectImage.ImageUrl, out var relativePath))
        {
            var fullPath = Path.GetFullPath(Path.Combine(hostingEnvironment.WebRootPath, relativePath));
            var webRootPath = Path.GetFullPath(hostingEnvironment.WebRootPath);
            if (fullPath.StartsWith(webRootPath, StringComparison.OrdinalIgnoreCase) && File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        _projectImageRepository.Delete(projectImage);
        await _projectImageRepository.SaveChangesAsync();
    }

    /// <summary>
    /// Validates the uploaded file before storage.
    /// </summary>
    /// <param name="imageFile">The uploaded file to validate.</param>
    private static void ValidateImageFile(IFormFile imageFile)
    {
        if (imageFile.Length == 0)
        {
            throw new InvalidOperationException("The selected file is empty.");
        }

        if (imageFile.Length > MaxImageSizeInBytes)
        {
            throw new InvalidOperationException("The selected image must be 5 MB or smaller.");
        }

        var extension = Path.GetExtension(imageFile.FileName);
        if (!AllowedExtensions.Contains(extension))
        {
            throw new InvalidOperationException("Only JPG, JPEG, PNG, and WEBP image files are allowed.");
        }
    }

    /// <summary>
    /// Ensures the URL is from an approved image source and not a malicious script or traversal path.
    /// </summary>
    /// <param name="imageUrl">The URL to validate.</param>
    /// <returns><c>true</c> when the URL is safe; otherwise <c>false</c>.</returns>
    private static bool IsAllowedImageSource(string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            return false;
        }

        var trimmedUrl = imageUrl.Trim();
        if (trimmedUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
            || trimmedUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return Uri.TryCreate(trimmedUrl, UriKind.Absolute, out var uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }

        if (trimmedUrl.StartsWith("/", StringComparison.Ordinal) || trimmedUrl.StartsWith("~/", StringComparison.Ordinal))
        {
            return !trimmedUrl.Contains("..", StringComparison.Ordinal)
                && !trimmedUrl.Contains('\\');
        }

        return false;
    }

    /// <summary>
    /// Normalizes a local image path so deletion remains within the web root and cannot traverse directories.
    /// </summary>
    /// <param name="imageUrl">The stored image URL.</param>
    /// <param name="relativePath">The normalized path relative to the web root.</param>
    /// <returns><c>true</c> if the path is valid for local cleanup; otherwise <c>false</c>.</returns>
    private static bool TryNormalizeLocalRelativePath(string? imageUrl, out string relativePath)
    {
        relativePath = string.Empty;

        if (string.IsNullOrWhiteSpace(imageUrl) || imageUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var sanitized = imageUrl.Trim().TrimStart('~');
        if (sanitized.StartsWith('/'))
        {
            sanitized = sanitized.TrimStart('/');
        }

        if (sanitized.Contains("..", StringComparison.Ordinal) || sanitized.Contains('\\'))
        {
            return false;
        }

        relativePath = sanitized.Replace('/', Path.DirectorySeparatorChar);
        return !string.IsNullOrWhiteSpace(relativePath);
    }

    private static async Task<string> ResolveImageUrlAsync(IFormFile? imageFile, string? imageUrl, IWebHostEnvironment hostingEnvironment)
    {
        if (imageFile is not null && imageFile.Length > 0)
        {
            ValidateImageFile(imageFile);

            var uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "uploads", "projects");
            Directory.CreateDirectory(uploadFolder);

            var extension = Path.GetExtension(imageFile.FileName);
            var fileName = $"{Guid.NewGuid():N}_{Path.GetFileNameWithoutExtension(imageFile.FileName)}{extension}";
            var safeName = string.Concat(fileName.Where(ch => !Path.GetInvalidFileNameChars().Contains(ch)));
            var filePath = Path.Combine(uploadFolder, safeName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return "/uploads/projects/" + safeName;
        }

        if (!string.IsNullOrWhiteSpace(imageUrl))
        {
            return imageUrl.Trim();
        }

        throw new InvalidOperationException("The image URL could not be resolved from the uploaded file or direct URL input.");
    }
}
