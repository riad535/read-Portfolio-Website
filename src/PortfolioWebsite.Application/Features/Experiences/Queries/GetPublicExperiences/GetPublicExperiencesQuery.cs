using MediatR;

namespace PortfolioWebsite.Application.Features.Experiences.Queries.GetPublicExperiences;

public sealed record GetPublicExperiencesQuery : IRequest<IReadOnlyList<PublicExperienceDto>>;
