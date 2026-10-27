using Al_Shami_ِAISS_.Models;
using Al_Shami_ِAISS_.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Al_Shami_ِAISS_.Services;

/// <summary>
/// Manages product gallery image logic.
/// This service keeps the controller thin and centralizes image validation, file upload handling, and repository orchestration.
/// </summary>
public class ProductImageService
{
    private const long MaxImageSizeInBytes = 5 * 1024 * 1024;
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".webp"
    };

    private readonly IProductImageRepository _productImageRepository;

    /// <summary>
    /// Initializes the product image service with a repository dependency.
    /// </summary>
    /// <param name="productImageRepository">The repository used for product image persistence.</param>
    public ProductImageService(IProductImageRepository productImageRepository)
    {
        _productImageRepository = productImageRepository;
    }

    /// <summary>
    /// Returns all images for a specific product.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <returns>A list of product images.</returns>
    public async Task<IEnumerable<ProductImage>> GetByProductIdAsync(int productId)
    {
        if (productId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(productId), "Product ID must be greater than zero.");
        }

        return await _productImageRepository.GetByProductIdAsync(productId);
    }

    /// <summary>
    /// Returns a single product image by identifier.
    /// </summary>
    /// <param name="id">The image identifier.</param>
    /// <returns>The image if found; otherwise null.</returns>
    public async Task<ProductImage?> GetByIdAsync(int id)
    {
        return await _productImageRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// Adds a product image using either an uploaded file or a direct URL.
    /// </summary>
    /// <param name="productId">The owning product identifier.</param>
    /// <param name="imageFile">An optional uploaded file.</param>
    /// <param name="imageUrl">An optional external or relative URL.</param>
    /// <param name="hostingEnvironment">The web host environment used to save uploaded files.</param>
    /// <returns>The saved product image.</returns>
    public async Task<ProductImage> AddAsync(int productId, IFormFile? imageFile, string? imageUrl, IWebHostEnvironment hostingEnvironment)
    {
        if (productId <= 0)
        {
            throw new InvalidOperationException("A valid product is required before an image can be uploaded.");
        }

        if (imageFile is null && string.IsNullOrWhiteSpace(imageUrl))
        {
            throw new InvalidOperationException("Please upload an image file or provide an image URL.");
        }

        if (imageFile is not null)
        {
            ValidateImageFile(imageFile);
        }

        if (!string.IsNullOrWhiteSpace(imageUrl))
        {
            var trimmedUrl = imageUrl.Trim();
            if (!trimmedUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                && !trimmedUrl.StartsWith("/", StringComparison.Ordinal)
                && !trimmedUrl.StartsWith("~", StringComparison.Ordinal))
            {
                throw new InvalidOperationException("Please provide a valid image URL.");
            }
        }

        var storedImageUrl = await ResolveImageUrlAsync(imageFile, imageUrl, hostingEnvironment);

        var productImage = new ProductImage
        {
            ProductId = productId,
            ImageUrl = storedImageUrl
        };

        await _productImageRepository.AddAsync(productImage);
        await _productImageRepository.SaveChangesAsync();

        return productImage;
    }

    /// <summary>
    /// Deletes an image record and removes the physical file from the web root if it was uploaded locally.
    /// </summary>
    /// <param name="id">The image identifier.</param>
    /// <param name="hostingEnvironment">The hosting environment used to locate uploaded files.</param>
    public async Task DeleteAsync(int id, IWebHostEnvironment hostingEnvironment)
    {
        var productImage = await _productImageRepository.GetByIdAsync(id);
        if (productImage is null)
        {
            throw new KeyNotFoundException($"Product image with id {id} was not found.");
        }

        if (!string.IsNullOrWhiteSpace(productImage.ImageUrl)
            && !productImage.ImageUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
        {
            var relativePath = productImage.ImageUrl.TrimStart('~', '/');
            var filePath = Path.Combine(hostingEnvironment.WebRootPath, relativePath.Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        _productImageRepository.Delete(productImage);
        await _productImageRepository.SaveChangesAsync();
    }

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

    private static async Task<string> ResolveImageUrlAsync(IFormFile? imageFile, string? imageUrl, IWebHostEnvironment hostingEnvironment)
    {
        if (imageFile is not null && imageFile.Length > 0)
        {
            ValidateImageFile(imageFile);

            var uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "uploads", "products");
            Directory.CreateDirectory(uploadFolder);

            var extension = Path.GetExtension(imageFile.FileName);
            var fileName = $"{Guid.NewGuid():N}_{Path.GetFileNameWithoutExtension(imageFile.FileName)}{extension}";
            var safeName = string.Concat(fileName.Where(ch => !Path.GetInvalidFileNameChars().Contains(ch)));
            var filePath = Path.Combine(uploadFolder, safeName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return "/uploads/products/" + safeName;
        }

        if (!string.IsNullOrWhiteSpace(imageUrl))
        {
            return imageUrl.Trim();
        }

        throw new InvalidOperationException("The image URL could not be resolved from the uploaded file or direct URL input.");
    }
}
