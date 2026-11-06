namespace PortfolioWebsite.Application.Features.Projects.Queries.GetProjectById;

public sealed record ProjectDetailDto(
    int Id,
    string Title,
    string Description,
    string TechStack,
    string? Architecture,
    string? LiveDemoUrl,
    string? GitHubUrl,
    int DisplayOrder,
    bool IsPublished);
