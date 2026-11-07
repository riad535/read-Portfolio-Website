using System.Text;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;

namespace PortfolioWebsite.Web.Controllers;

public class SeoController : Controller
{
    private static readonly XNamespace SitemapNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9";

    [HttpGet]
    [Route("sitemap.xml")]
    [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
    public IActionResult Sitemap()
    {
        var homePath = Url.Action(nameof(HomeController.Index), "Home") ?? "/";
        var homeUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}{homePath}";

        var document = new XDocument(
            new XElement(SitemapNamespace + "urlset",
                new XElement(SitemapNamespace + "url",
                    new XElement(SitemapNamespace + "loc", homeUrl))));

        var xml = $"<?xml version=\"1.0\" encoding=\"utf-8\"?>{Environment.NewLine}{document}";

        return Content(xml, "application/xml", Encoding.UTF8);
    }
}
