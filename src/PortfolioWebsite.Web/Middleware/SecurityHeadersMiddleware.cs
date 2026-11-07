namespace PortfolioWebsite.Web.Middleware;

public sealed class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers["X-Content-Type-Options"] = "nosniff";
        context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

        if (context.Request.Path.StartsWithSegments("/admin"))
        {
            context.Response.Headers["X-Frame-Options"] = "DENY";
            context.Response.Headers["Cache-Control"] = "no-store";
        }

        return _next(context);
    }
}
