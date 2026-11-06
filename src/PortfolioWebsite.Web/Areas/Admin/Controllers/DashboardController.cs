using MediatR;
using Microsoft.AspNetCore.Mvc;
using PortfolioWebsite.Application.Features.Dashboard.Queries.GetDashboardSummary;

namespace PortfolioWebsite.Web.Areas.Admin.Controllers;

[Route("admin")]
public sealed class DashboardController : AdminControllerBase
{
    private readonly ISender _sender;

    public DashboardController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var summary = await _sender.Send(new GetDashboardSummaryQuery(), cancellationToken);

        ViewData["Title"] = "Dashboard";

        return View(summary);
    }
}
