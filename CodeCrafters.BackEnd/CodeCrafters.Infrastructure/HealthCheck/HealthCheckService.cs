using CodeCrafters.Application.HealthCheck.Dtos;
using CodeCrafters.Application.HealthCheck.Services;
using CodeCrafters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CodeCrafters.Infrastructure.HealthCheck;

public sealed class HealthCheckService(AppDbContext dbContext, ILogger<HealthCheckService> logger) : IHealthCheckService
{
    public async Task<HealthCheckResponseDto> CheckHealthAsync(CancellationToken cancellationToken = default)
    {
        var dbConnected = await dbContext.Database.CanConnectAsync(cancellationToken);

        logger.LogInformation("Health check executed. DatabaseConnected: {DatabaseConnected}", dbConnected);

        return new HealthCheckResponseDto
        {
            IsHealthy = dbConnected,
            DatabaseConnected = dbConnected,
            UtcTime = DateTime.UtcNow
        };
    }
}

