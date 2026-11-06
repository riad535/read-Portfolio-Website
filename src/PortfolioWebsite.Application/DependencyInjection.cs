using Microsoft.Extensions.DependencyInjection;
using PortfolioWebsite.Application.Common.Behaviours;
using PortfolioWebsite.Application.Common.Interfaces;

namespace PortfolioWebsite.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            configuration.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        });

        RegisterValidators(services);

        return services;
    }

    private static void RegisterValidators(IServiceCollection services)
    {
        var validatorInterface = typeof(IRequestValidator<>);

        var registrations = typeof(DependencyInjection).Assembly
            .GetTypes()
            .Where(type => type is { IsAbstract: false, IsInterface: false })
            .SelectMany(type => type
                .GetInterfaces()
                .Where(@interface =>
                    @interface.IsGenericType &&
                    @interface.GetGenericTypeDefinition() == validatorInterface)
                .Select(@interface => new { Service = @interface, Implementation = type }));

        foreach (var registration in registrations)
        {
            services.AddTransient(registration.Service, registration.Implementation);
        }
    }
}
