using Microsoft.Extensions.DependencyInjection;

namespace CodeCrafters.Application.DependencyInjection;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register application-layer services (interfaces + implementations) here as they are added.
        return services;
    }
}

