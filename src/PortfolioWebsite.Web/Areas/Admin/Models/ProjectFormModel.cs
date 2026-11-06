namespace PortfolioWebsite.Web.Areas.Admin.Models;

public sealed class ProjectFormModel
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string TechStack { get; set; } = string.Empty;

    public string? Architecture { get; set; }

    public string? LiveDemoUrl { get; set; }

    public string? GitHubUrl { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsPublished { get; set; } = true;
}
