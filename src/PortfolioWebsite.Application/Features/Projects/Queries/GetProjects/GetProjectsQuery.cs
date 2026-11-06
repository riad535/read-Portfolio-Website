using MediatR;

namespace PortfolioWebsite.Application.Features.Projects.Queries.GetProjects;

public sealed record GetProjectsQuery : IRequest<IReadOnlyList<ProjectListItemDto>>;
