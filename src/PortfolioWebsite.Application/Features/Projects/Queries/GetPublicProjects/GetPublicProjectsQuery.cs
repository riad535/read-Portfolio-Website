using MediatR;

namespace PortfolioWebsite.Application.Features.Projects.Queries.GetPublicProjects;

public sealed record GetPublicProjectsQuery : IRequest<IReadOnlyList<PublicProjectDto>>;
