namespace PortfolioWebsite.Application.Features.Skills.Queries.GetSkillCategories;

public sealed record SkillCategoryListItemDto(
    int Id,
    string Name,
    string IconClass,
    int DisplayOrder,
    int SkillCount);
