namespace CodeCrafters.Application.HealthCheck.Dtos;

public sealed class HealthCheckResponseDto
{
    public bool IsHealthy { get; init; }

    public bool DatabaseConnected { get; init; }

    public DateTime UtcTime { get; init; }
}

