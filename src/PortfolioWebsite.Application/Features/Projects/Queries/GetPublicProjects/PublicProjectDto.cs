namespace PortfolioWebsite.Application.Features.Projects.Queries.GetPublicProjects;

public sealed record PublicProjectDto(
    string Title,
    string Description,
    string TechStack,
    string? Architecture,
    string? LiveDemoUrl,
    string? GitHubUrl);
