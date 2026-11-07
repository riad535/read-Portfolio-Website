using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PortfolioWebsite.Application;
using PortfolioWebsite.Infrastructure;
using PortfolioWebsite.Web.Middleware;
using PortfolioWebsite.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;

    options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
        ? CookieSecurePolicy.SameAsRequest
        : CookieSecurePolicy.Always;
});
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSingleton<IStaticContentProvider, StaticContentProvider>();
builder.Services.AddScoped<IPortfolioService, PortfolioService>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;

    // The local HTTP launch profile has no HTTPS endpoint; production must always use HTTPS.
    options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
        ? CookieSecurePolicy.SameAsRequest
        : CookieSecurePolicy.Always;

    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
    options.LoginPath = "/admin/login";
    options.AccessDeniedPath = "/admin/login";
});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("login", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0,
                AutoReplenishment = true
            }));

    options.OnRejected = (context, _) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "text/plain; charset=utf-8";
        return new ValueTask(
            context.HttpContext.Response.WriteAsync("Too many requests. Try again later."));
    };
});

var app = builder.Build();

// Security headers are applied before anything can write a response.
app.UseMiddleware<SecurityHeadersMiddleware>();

// Development only: apply pending migrations and seed initial content.
// Production never applies migrations; it only seeds when the database is reachable
// and has no pending migrations.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    await app.Services.InitializeInfrastructureAsync(applyMigrations: true);
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    await app.Services.InitializeInfrastructureAsync(applyMigrations: false);
}

app.UseHttpsRedirection();

// Public 404s use the styled page; the admin area keeps its own error handling.
app.UseWhen(
    context => !context.Request.Path.StartsWithSegments("/admin"),
    branch => branch.UseStatusCodePagesWithReExecute("/Home/PageNotFound"));

app.UseRouting();

app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
