using MediatR;

namespace PortfolioWebsite.Application.Features.Projects.Commands.UpdateProject;

public sealed record UpdateProjectCommand(
    int Id,
    string Title,
    string Description,
    string TechStack,
    string? Architecture,
    string? LiveDemoUrl,
    string? GitHubUrl,
    int DisplayOrder,
    bool IsPublished) : IRequest;
