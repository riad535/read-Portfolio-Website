using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PortfolioWebsite.Web.Areas.Admin.Models;

namespace PortfolioWebsite.Web.Areas.Admin.Controllers;

[Area("Admin")]
public sealed class AccountController : Controller
{
    private const string InvalidCredentialsMessage = "Invalid email or password.";
    private const string LockedOutMessage = "Too many attempts. Try again later.";

    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager,
        ILogger<AccountController> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    [HttpGet("admin/login")]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost("admin/login")]
    [AllowAnonymous]
    [EnableRateLimiting("login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email.Trim());

        if (user is null)
        {
            LogLoginFailure("FailedLogin");
            ModelState.AddModelError(string.Empty, InvalidCredentialsMessage);
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(
            user,
            model.Password,
            isPersistent: false,
            lockoutOnFailure: true);

        if (result.Succeeded)
        {
            if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return LocalRedirect(model.ReturnUrl);
            }

            return RedirectToAction("Index", "Dashboard");
        }

        LogLoginFailure(result.IsLockedOut ? "Lockout" : "FailedLogin");

        ModelState.AddModelError(
            string.Empty,
            result.IsLockedOut ? LockedOutMessage : InvalidCredentialsMessage);

        return View(model);
    }

    [HttpPost("admin/logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction(nameof(Login));
    }

    private void LogLoginFailure(string eventType)
    {
        var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        _logger.LogWarning(
            "Admin login rejected. EventType: {EventType}. ClientIp: {ClientIp}",
            eventType,
            clientIp);
    }
}
