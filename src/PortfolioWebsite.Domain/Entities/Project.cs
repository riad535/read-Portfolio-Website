using PortfolioWebsite.Domain.Common;

namespace PortfolioWebsite.Domain.Entities;

public class Project : BaseEntity
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
