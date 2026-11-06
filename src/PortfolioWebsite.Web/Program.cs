using PortfolioWebsite.Application;
using PortfolioWebsite.Infrastructure;
using PortfolioWebsite.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSingleton<IStaticContentProvider, StaticContentProvider>();
builder.Services.AddScoped<IPortfolioService, PortfolioService>();

var app = builder.Build();

// Development only: apply pending migrations and seed initial content.
if (app.Environment.IsDevelopment())
{
    await app.Services.InitializeInfrastructureAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
