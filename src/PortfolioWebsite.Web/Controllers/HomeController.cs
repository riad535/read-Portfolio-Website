using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PortfolioWebsite.Web.Models;
using PortfolioWebsite.Web.Services;

namespace PortfolioWebsite.Web.Controllers;

public class HomeController : Controller
{
    private readonly IPortfolioService _portfolioService;
    private readonly IStaticContentProvider _staticContentProvider;

    public HomeController(
        IPortfolioService portfolioService,
        IStaticContentProvider staticContentProvider)
    {
        _portfolioService = portfolioService;
        _staticContentProvider = staticContentProvider;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var portfolio = await _portfolioService.GetPortfolioAsync(cancellationToken);

        SetSharedViewData(portfolio);
        ViewData["Title"] = portfolio.PageTitle;
        ViewData["MetaDescription"] = portfolio.MetaDescription;
        ViewData["IsHomePage"] = true;
        SetSeoViewData(portfolio);

        return View(portfolio);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult PageNotFound()
    {
        var staticContent = _staticContentProvider.GetStaticContent();

        SetSharedViewData(staticContent);
        ViewData["Title"] = "Page Not Found";
        ViewData["RobotsNoIndex"] = true;
        Response.StatusCode = StatusCodes.Status404NotFound;

        return View("NotFound");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var originalPath = HttpContext.Features.Get<IExceptionHandlerPathFeature>()?.Path;
        var isAdminPath = !string.IsNullOrEmpty(originalPath) &&
                          new PathString(originalPath).StartsWithSegments("/admin");

        if (isAdminPath)
        {
            ViewData["Title"] = "Error";

            return View("~/Areas/Admin/Views/Shared/AdminError.cshtml");
        }

        var staticContent = _staticContentProvider.GetStaticContent();

        SetSharedViewData(staticContent);
        ViewData["Title"] = "Error";
        ViewData["RobotsNoIndex"] = true;

        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private void SetSharedViewData(PortfolioViewModel portfolio)
    {
        ViewData["BrandName"] = portfolio.Profile.FullName;
        ViewData["Navigation"] = portfolio.Navigation;
        ViewData["ButtonLabels"] = portfolio.ButtonLabels;
        ViewData["GitHubUrl"] = portfolio.Profile.GitHubUrl;
        ViewData["LinkedInUrl"] = portfolio.Profile.LinkedInUrl;
        ViewData["FooterText"] = portfolio.FooterText;
    }

    private void SetSeoViewData(PortfolioViewModel portfolio)
    {
        var canonicalPath = Url.Action(nameof(Index), "Home") ?? "/";
        var canonicalUrl = ToAbsoluteUrl(canonicalPath);

        ViewData["CanonicalUrl"] = canonicalUrl;
        ViewData["OgTitle"] = portfolio.OgTitle;
        ViewData["OgDescription"] = portfolio.OgDescription;
        ViewData["OgImageUrl"] = ToAbsoluteUrl(portfolio.OgImagePath);
    }

    private string ToAbsoluteUrl(string path)
    {
        return $"{Request.Scheme}://{Request.Host}{Request.PathBase}{path}";
    }
}
