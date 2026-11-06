using MediatR;
using Microsoft.EntityFrameworkCore;
using PortfolioWebsite.Application.Common.Interfaces;

namespace PortfolioWebsite.Application.Features.Dashboard.Queries.GetDashboardSummary;

public sealed class GetDashboardSummaryHandler
    : IRequestHandler<GetDashboardSummaryQuery, DashboardSummaryDto>
{
    private readonly IApplicationDbContext _context;

    public GetDashboardSummaryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardSummaryDto> Handle(
        GetDashboardSummaryQuery request,
        CancellationToken cancellationToken)
    {
        var skillCategoryCount = await _context.SkillCategories
            .AsNoTracking()
            .CountAsync(cancellationToken);

        var projectCount = await _context.Projects
            .AsNoTracking()
            .CountAsync(cancellationToken);

        var experienceCount = await _context.Experiences
            .AsNoTracking()
            .CountAsync(cancellationToken);

        return new DashboardSummaryDto(
            skillCategoryCount,
            projectCount,
            experienceCount);
    }
}
