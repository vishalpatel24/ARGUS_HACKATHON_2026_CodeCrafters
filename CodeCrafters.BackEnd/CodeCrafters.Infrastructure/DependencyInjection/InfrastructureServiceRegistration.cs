using CodeCrafters.Application.Auth.Services;
using CodeCrafters.Application.HealthCheck.Services;
using CodeCrafters.Infrastructure.Auth;
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
            ?? "Server=(localdb)\\MSSQLLocalDB;Database=CodeCraftersDb;Trusted_Connection=True;TrustServerCertificate=True;";

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        services.AddSingleton<JwtTokenService>();

        services.AddScoped<IHealthCheckService, HealthCheckService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<CodeCrafters.Application.Grants.Services.IGrantService, CodeCrafters.Infrastructure.Grants.GrantService>();
        services.AddScoped<CodeCrafters.Application.Organisations.Services.IOrganisationService, CodeCrafters.Infrastructure.Organisations.OrganisationService>();

        return services;
    }
}

