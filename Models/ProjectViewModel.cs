namespace PortfolioWebsite.Models;

public class ProjectViewModel
{
    public string Title { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public string TechStack { get; init; } = string.Empty;

    public string? Architecture { get; init; }

    public string? LiveDemoUrl { get; init; }

    public string? GitHubUrl { get; init; }
}
