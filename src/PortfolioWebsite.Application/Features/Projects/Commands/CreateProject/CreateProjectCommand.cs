using MediatR;

namespace PortfolioWebsite.Application.Features.Projects.Commands.CreateProject;

public sealed record CreateProjectCommand(
    string Title,
    string Description,
    string TechStack,
    string? Architecture,
    string? LiveDemoUrl,
    string? GitHubUrl,
    int DisplayOrder,
    bool IsPublished) : IRequest<int>;
