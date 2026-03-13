using CodeCrafters.Application.HealthCheck.Services;
using CodeCrafters.Infrastructure.Data;
using CodeCrafters.Infrastructure.HealthCheck;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeCrafters.Infrastructure.DependencyInjection;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("codecraftersdb")
            ?? configuration.GetConnectionString("DefaultConnection")
            ?? "Server=localhost;Database=CodeCraftersDb;Trusted_Connection=True;TrustServerCertificate=True;";

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IHealthCheckService, HealthCheckService>();

        return services;
    }
}

