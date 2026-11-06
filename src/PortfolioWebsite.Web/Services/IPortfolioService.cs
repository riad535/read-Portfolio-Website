using PortfolioWebsite.Web.Models;

namespace PortfolioWebsite.Web.Services;

public interface IPortfolioService
{
    Task<PortfolioViewModel> GetPortfolioAsync(CancellationToken cancellationToken = default);
}
