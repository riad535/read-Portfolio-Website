using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PortfolioWebsite.Application.Common.Interfaces;
using PortfolioWebsite.Infrastructure.Persistence;
using PortfolioWebsite.Infrastructure.Persistence.Seed;

namespace PortfolioWebsite.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' is not configured.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString, sqlServer =>
                sqlServer.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 10;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddScoped<IApplicationDbContext>(serviceProvider =>
            serviceProvider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<DbSeeder>();

        return services;
    }

    /// <summary>
    /// Initializes the database. Migrations are applied only when <paramref name="applyMigrations"/>
    /// is true (Development). In Production the initializer never migrates: it seeds only when the
    /// database is reachable and has no pending migrations. Production failures are logged and
    /// seeding is skipped so the application keeps running.
    /// </summary>
    public static async Task InitializeInfrastructureAsync(
        this IServiceProvider services,
        bool applyMigrations,
        CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var logger = scope.ServiceProvider
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("PortfolioWebsite.Infrastructure.DatabaseInitialization");

        try
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (applyMigrations)
            {
                await context.Database.MigrateAsync(cancellationToken);
            }
            else
            {
                var pendingMigrations = await context.Database
                    .GetPendingMigrationsAsync(cancellationToken);

                if (pendingMigrations.Any())
                {
                    logger.LogError(
                        "Database seeding was skipped because migrations are pending. " +
                        "Apply the migration script before starting the application.");
                    return;
                }
            }

            var seeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
            await seeder.SeedAsync(cancellationToken);
        }
        catch (Exception exception) when (!applyMigrations)
        {
            logger.LogError(
                exception,
                "Database seeding was skipped because the database could not be reached. " +
                "The application will continue to run without seeding.");
        }
    }
}
