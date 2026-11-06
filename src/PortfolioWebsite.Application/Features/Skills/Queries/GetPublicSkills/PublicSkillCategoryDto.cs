namespace PortfolioWebsite.Application.Features.Skills.Queries.GetPublicSkills;

public sealed record PublicSkillCategoryDto(
    string Name,
    string IconClass,
    IReadOnlyList<string> Skills);
