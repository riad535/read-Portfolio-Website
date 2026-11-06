using MediatR;

namespace PortfolioWebsite.Application.Features.Experiences.Queries.GetExperienceById;

public sealed record GetExperienceByIdQuery(int Id) : IRequest<ExperienceDetailDto>;
