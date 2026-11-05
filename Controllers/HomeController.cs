using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PortfolioWebsite.Models;
using PortfolioWebsite.Services;

namespace PortfolioWebsite.Controllers;

public class HomeController : Controller
{
    private readonly IPortfolioService _portfolioService;

    public HomeController(IPortfolioService portfolioService)
    {
        _portfolioService = portfolioService;
    }

    public IActionResult Index()
    {
        return View(LoadPortfolioViewData());
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        LoadPortfolioViewData();

        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private PortfolioViewModel LoadPortfolioViewData()
    {
        var portfolio = _portfolioService.GetPortfolio();

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
