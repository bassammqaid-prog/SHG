namespace Al_Shami_ِAISS_.ViewModels;

/// <summary>
/// Represents the data displayed on the public home page.
/// This ViewModel is intentionally designed for the presentation layer and does not expose the database entities directly.
/// The controller maps domain data into this model before sending it to the Razor view.
/// </summary>
public class HomeViewModel
{
    /// <summary>
    /// Short company introduction shown in the hero area and overview section.
    /// </summary>
    public string CompanyIntroduction { get; set; } = "Al-Shami Integrated Solutions & Supply (AISS) delivers trusted industrial, safety, and energy solutions across the region.";

    /// <summary>
    /// Mission statement for the public about section.
    /// </summary>
    public string MissionStatement { get; set; } = "To provide safe, reliable, and innovative solutions that support the growth and operational excellence of our clients.";

    /// <summary>
    /// Collection of categories highlighted on the home page.
    /// </summary>
    public List<HomeCategoryItemViewModel> FeaturedCategories { get; set; } = new();

    /// <summary>
    /// Collection of featured products displayed in promotional cards.
    /// </summary>
    public List<HomeProductItemViewModel> FeaturedProducts { get; set; } = new();

    /// <summary>
    /// Collection of featured portfolio projects displayed in the website hero or showcase section.
    /// </summary>
    public List<HomeProjectItemViewModel> FeaturedProjects { get; set; } = new();

    /// <summary>
    /// A short list of services offered by the company.
    /// </summary>
    public List<string> Services { get; set; } = new();

    /// <summary>
    /// Reasons customers choose AISS.
    /// </summary>
    public List<string> WhyChooseUs { get; set; } = new();
}

/// <summary>
/// Represents a category shown on the home page.
/// </summary>
public class HomeCategoryItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

/// <summary>
/// Represents a product card on the home page.
/// </summary>
public class HomeProductItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Brand { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public string CategoryName { get; set; } = string.Empty;
}

/// <summary>
/// Represents a project card on the home page.
/// </summary>
public class HomeProjectItemViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ClientName { get; set; }
    public string? Location { get; set; }
    public string? ImageUrl { get; set; }
}
