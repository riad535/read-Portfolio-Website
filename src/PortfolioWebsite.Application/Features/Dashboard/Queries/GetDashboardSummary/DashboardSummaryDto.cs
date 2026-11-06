namespace PortfolioWebsite.Application.Features.Dashboard.Queries.GetDashboardSummary;

public sealed record DashboardSummaryDto(
    int SkillCategoryCount,
    int ProjectCount,
    int ExperienceCount);
