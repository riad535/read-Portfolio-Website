namespace PortfolioWebsite.Application.Features.Projects.Queries.GetProjects;

public sealed record ProjectListItemDto(
    int Id,
    string Title,
    int DisplayOrder,
    bool IsPublished);
