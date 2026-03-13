using CodeCrafters.Application.HealthCheck.Dtos;

namespace CodeCrafters.Application.HealthCheck.Services;

public interface IHealthCheckService
{
    Task<HealthCheckResponseDto> CheckHealthAsync(CancellationToken cancellationToken = default);
}

