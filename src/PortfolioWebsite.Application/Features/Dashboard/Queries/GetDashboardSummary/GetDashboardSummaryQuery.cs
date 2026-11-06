using MediatR;

namespace PortfolioWebsite.Application.Features.Dashboard.Queries.GetDashboardSummary;

public sealed record GetDashboardSummaryQuery : IRequest<DashboardSummaryDto>;
