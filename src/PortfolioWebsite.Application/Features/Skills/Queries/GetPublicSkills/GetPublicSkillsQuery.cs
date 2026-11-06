using MediatR;

namespace PortfolioWebsite.Application.Features.Skills.Queries.GetPublicSkills;

public sealed record GetPublicSkillsQuery : IRequest<IReadOnlyList<PublicSkillCategoryDto>>;
