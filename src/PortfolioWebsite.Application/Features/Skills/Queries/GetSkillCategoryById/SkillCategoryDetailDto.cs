namespace PortfolioWebsite.Application.Features.Skills.Queries.GetSkillCategoryById;

public sealed record SkillCategoryDetailDto(
    int Id,
    string Name,
    string IconClass,
    int DisplayOrder,
    IReadOnlyList<string> Skills);
