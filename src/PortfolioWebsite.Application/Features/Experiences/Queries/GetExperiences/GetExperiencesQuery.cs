using MediatR;

namespace PortfolioWebsite.Application.Features.Experiences.Queries.GetExperiences;

public sealed record GetExperiencesQuery : IRequest<IReadOnlyList<ExperienceListItemDto>>;
