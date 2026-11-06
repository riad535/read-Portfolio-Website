using MediatR;

namespace PortfolioWebsite.Application.Features.Skills.Queries.GetSkillCategories;

public sealed record GetSkillCategoriesQuery : IRequest<IReadOnlyList<SkillCategoryListItemDto>>;
