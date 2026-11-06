using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PortfolioWebsite.Web.Models;
using PortfolioWebsite.Web.Services;

namespace PortfolioWebsite.Web.Controllers;

public class HomeController : Controller
{
    private readonly IPortfolioService _portfolioService;

    public HomeController(IPortfolioService portfolioService)
    {
        _portfolioService = portfolioService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        return View(await LoadPortfolioViewDataAsync(cancellationToken));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Error(CancellationToken cancellationToken)
    {
        await LoadPortfolioViewDataAsync(cancellationToken);

        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private async Task<PortfolioViewModel> LoadPortfolioViewDataAsync(CancellationToken cancellationToken)
    {
        var portfolio = await _portfolioService.GetPortfolioAsync(cancellationToken);

        ViewData["Title"] = portfolio.PageTitle;
        ViewData["MetaDescription"] = portfolio.MetaDescription;
        ViewData["BrandName"] = portfolio.Profile.FullName;
        ViewData["Navigation"] = portfolio.Navigation;
        ViewData["ButtonLabels"] = portfolio.ButtonLabels;
        ViewData["GitHubUrl"] = portfolio.Profile.GitHubUrl;
        ViewData["LinkedInUrl"] = portfolio.Profile.LinkedInUrl;
        ViewData["FooterText"] = portfolio.FooterText;

        return portfolio;
    }
}
