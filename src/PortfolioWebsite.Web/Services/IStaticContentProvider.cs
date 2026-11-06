using PortfolioWebsite.Web.Models;

namespace PortfolioWebsite.Web.Services;

public interface IStaticContentProvider
{
    PortfolioViewModel GetStaticContent();
}
