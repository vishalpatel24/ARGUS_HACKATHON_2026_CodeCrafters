using CodeCrafters.Application.Screening.Dtos;

namespace CodeCrafters.Application.Screening.Services;

public interface IEligibilityScreeningService
{
    /// <summary>Run eligibility screening on a submitted application and generate a report.</summary>
    Task<ScreeningReportDto> RunScreeningAsync(Guid applicationId, CancellationToken ct = default);

    /// <summary>Get screening report for an application.</summary>
    Task<ScreeningReportDto?> GetByApplicationIdAsync(Guid applicationId, CancellationToken ct = default);

    /// <summary>Get all screening reports (for Program Officer queue).</summary>
    Task<List<ScreeningReportDto>> GetAllAsync(string? filterResult = null, CancellationToken ct = default);

    /// <summary>Run screening for any application that doesn't have a report yet.</summary>
    Task SyncAllPendingAsync(CancellationToken ct = default);

    /// <summary>Program Officer takes action on a screening report.</summary>
    Task<ScreeningReportDto> TakeActionAsync(Guid reportId, Guid officerUserId, ScreeningActionDto action, CancellationToken ct = default);
}
