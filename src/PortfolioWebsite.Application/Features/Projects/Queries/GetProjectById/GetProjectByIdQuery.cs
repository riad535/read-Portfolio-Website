using MediatR;

namespace PortfolioWebsite.Application.Features.Projects.Queries.GetProjectById;

public sealed record GetProjectByIdQuery(int Id) : IRequest<ProjectDetailDto>;
