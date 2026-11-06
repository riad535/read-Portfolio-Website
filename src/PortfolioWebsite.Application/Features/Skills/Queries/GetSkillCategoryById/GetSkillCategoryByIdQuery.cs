using MediatR;

namespace PortfolioWebsite.Application.Features.Skills.Queries.GetSkillCategoryById;

public sealed record GetSkillCategoryByIdQuery(int Id) : IRequest<SkillCategoryDetailDto>;
