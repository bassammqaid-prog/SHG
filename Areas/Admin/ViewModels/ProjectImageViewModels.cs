using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Al_Shami_AISS_.Areas.Admin.ViewModels;

/// <summary>
/// Represents the project gallery listing screen in the admin area.
/// It groups the related project information with its uploaded gallery images for easier UI rendering.
/// </summary>
public class ProjectImageListViewModel
{
    public int ProjectId { get; set; }

    [Display(Name = "Project")]
    public string ProjectTitle { get; set; } = string.Empty;

    public List<ProjectImageSummaryViewModel> Images { get; set; } = new();
}

/// <summary>
/// Represents one gallery image shown inside the project image list.
/// </summary>
public class ProjectImageSummaryViewModel
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}

/// <summary>
/// Used when an administrator uploads a new project gallery image.
/// The view model keeps HTTP data separate from the EF entity and helps model binding stay clean.
/// </summary>
public class ProjectImageCreateViewModel
{
    public int ProjectId { get; set; }

    [Display(Name = "Project")]
    public string ProjectTitle { get; set; } = string.Empty;

    [Display(Name = "Upload File From Computer")]
    public IFormFile? UploadedFile { get; set; }

    // Maintain compatibility with existing form binding while exposing the clearer UploadedFile property
    // required for the project gallery upload flow.
    [Display(Name = "Image File")]
    public IFormFile? ImageFile
    {
        get => UploadedFile;
        set => UploadedFile = value;
    }

    [Url(ErrorMessage = "Please provide a valid image URL.")]
    [Display(Name = "Image URL")]
    public string? ImageUrl { get; set; }
}

/// <summary>
/// Used when deleting a project gallery image.
/// </summary>
public class ProjectImageDeleteViewModel
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string ProjectTitle { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}
