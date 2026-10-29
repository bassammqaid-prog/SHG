using System.Text;

namespace Al_Shami_ِAISS_.Services;

/// <summary>
/// Centralizes the generation and validation rules for SEO-friendly slugs.
/// Slugs are important because they make URLs readable for users and search engines,
/// while also creating stable routes that can survive database or content changes.
/// </summary>
public static class SeoSlugService
{
    /// <summary>
    /// Converts a source value into a lowercase, URL-safe slug.
    /// Example: "Steel Security Door" becomes "steel-security-door".
    /// </summary>
    /// <param name="value">The source text to convert.</param>
    /// <returns>A normalized slug that is safe for URL routing.</returns>
    public static string GenerateSlug(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "item";
        }

        var builder = new StringBuilder();
        var previousDash = false;

        foreach (var character in value.Trim())
        {
            if (char.IsLetterOrDigit(character))
            {
                builder.Append(char.ToLowerInvariant(character));
                previousDash = false;
            }
            else if (!previousDash)
            {
                builder.Append('-');
                previousDash = true;
            }
        }

        var slug = builder.ToString().Trim('-');
        return string.IsNullOrWhiteSpace(slug) ? "item" : slug;
    }

    /// <summary>
    /// Generates a unique slug by checking whether the slug already exists in the data store.
    /// This prevents duplicate URLs and helps avoid SEO content conflicts.
    /// </summary>
    /// <param name="value">The source text used to build the original slug.</param>
    /// <param name="exists">A callback that checks whether a slug is already in use.</param>
    /// <param name="currentSlug">The current slug when an item is being edited.</param>
    /// <returns>A unique slug ready to persist to the database.</returns>
    public static async Task<string> EnsureUniqueSlugAsync(string? value, Func<string, Task<bool>> exists, string? currentSlug = null)
    {
        var baseSlug = GenerateSlug(value);
        if (string.IsNullOrWhiteSpace(baseSlug))
        {
            return "item";
        }

        if (!string.IsNullOrWhiteSpace(currentSlug) && string.Equals(baseSlug, currentSlug, StringComparison.OrdinalIgnoreCase))
        {
            return currentSlug;
        }

        if (!await exists(baseSlug))
        {
            return baseSlug;
        }

        var suffix = 2;
        var candidate = $"{baseSlug}-{suffix}";
        while (await exists(candidate))
        {
            suffix++;
            candidate = $"{baseSlug}-{suffix}";
        }

        return candidate;
    }
}
