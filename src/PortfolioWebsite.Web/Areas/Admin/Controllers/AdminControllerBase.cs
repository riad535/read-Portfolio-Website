using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PortfolioWebsite.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public abstract class AdminControllerBase : Controller
{
}
