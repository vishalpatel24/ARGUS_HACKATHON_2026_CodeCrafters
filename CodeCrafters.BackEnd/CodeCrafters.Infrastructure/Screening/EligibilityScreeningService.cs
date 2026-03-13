using CodeCrafters.Application.Screening.Dtos;
using CodeCrafters.Application.Screening.Services;
using CodeCrafters.Domain.Entities;
using CodeCrafters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeCrafters.Infrastructure.Screening;

public class EligibilityScreeningService(AppDbContext db) : IEligibilityScreeningService
{
    public async Task<ScreeningReportDto> RunScreeningAsync(Guid applicationId, CancellationToken ct = default)
    {
        var app = await db.Applications
            .Include(a => a.GrantType)
            .Include(a => a.Applicant)
            .FirstOrDefaultAsync(a => a.Id == applicationId, ct)
            ?? throw new KeyNotFoundException("Application not found.");

        // Delete any existing report for re-screening
        var existingReport = await db.ScreeningReports
            .Include(r => r.Checks)
            .FirstOrDefaultAsync(r => r.ApplicationId == applicationId, ct);
        if (existingReport != null)
        {
            db.ScreeningChecks.RemoveRange(existingReport.Checks);
            db.ScreeningReports.Remove(existingReport);
        }

        var report = new ScreeningReport
        {
            Id = Guid.NewGuid(),
            ApplicationId = applicationId,
            CreatedAt = DateTime.UtcNow,
            GeneratedAt = DateTime.UtcNow
        };

        var checks = new List<ScreeningCheck>();
        var grantCode = app.GrantType.Code.ToUpperInvariant();

        switch (grantCode)
        {
            case "CDG":
                RunCdgChecks(app, checks);
                break;
            case "EIG":
                RunEigChecks(app, checks);
                break;
            case "ECAG":
                RunEcagChecks(app, checks);
                break;
        }

        // Assign display order
        for (int i = 0; i < checks.Count; i++)
        {
            checks[i].DisplayOrder = i + 1;
            checks[i].ScreeningReportId = report.Id;
        }

        report.Checks = checks;
        report.HardChecksPassed = checks.Count(c => c.CheckType == "Hard" && c.Result == "Pass");
        report.HardChecksFailed = checks.Count(c => c.CheckType == "Hard" && c.Result == "Fail");
        report.SoftFlags = checks.Count(c => c.CheckType == "Soft" && c.Result == "Flag");

        report.OverallResult = report.HardChecksFailed > 0 ? "Ineligible" : "PendingReview";

        db.ScreeningReports.Add(report);

        // Update application status
        app.StatusLabel = report.OverallResult == "Ineligible" ? "Ineligible" : "Screening";
        app.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);

        return await MapToDto(report, app, ct);
    }

    public async Task<ScreeningReportDto?> GetByApplicationIdAsync(Guid applicationId, CancellationToken ct = default)
    {
        var report = await db.ScreeningReports
            .Include(r => r.Checks.OrderBy(c => c.DisplayOrder))
            .Include(r => r.Application).ThenInclude(a => a.GrantType)
            .Include(r => r.Application).ThenInclude(a => a.Applicant)
            .Include(r => r.ReviewedByUser)
            .FirstOrDefaultAsync(r => r.ApplicationId == applicationId, ct);

        if (report == null) return null;
        return await MapToDto(report, report.Application, ct);
    }

    public async Task<List<ScreeningReportDto>> GetAllAsync(string? filterResult = null, CancellationToken ct = default)
    {
        var query = db.ScreeningReports
            .Include(r => r.Checks.OrderBy(c => c.DisplayOrder))
            .Include(r => r.Application).ThenInclude(a => a.GrantType)
            .Include(r => r.Application).ThenInclude(a => a.Applicant)
            .Include(r => r.ReviewedByUser)
            .AsQueryable();

        if (!string.IsNullOrEmpty(filterResult))
            query = query.Where(r => r.OverallResult == filterResult);

        var reports = await query.OrderByDescending(r => r.GeneratedAt).ToListAsync(ct);
        var results = new List<ScreeningReportDto>();
        foreach (var r in reports)
            results.Add(await MapToDto(r, r.Application, ct));
        return results;
    }

    public async Task SyncAllPendingAsync(CancellationToken ct = default)
    {
        var applicationIdsWithReports = await db.ScreeningReports.Select(r => r.ApplicationId).ToListAsync(ct);
        var pendingApplicationIds = await db.Applications
            .Where(a => !applicationIdsWithReports.Contains(a.Id))
            .Select(a => a.Id)
            .ToListAsync(ct);

        foreach (var appId in pendingApplicationIds)
        {
            try {
                await RunScreeningAsync(appId, ct);
            } catch {
                // Ignore individual failures in batch
            }
        }
    }

    public async Task<ScreeningReportDto> TakeActionAsync(Guid reportId, Guid officerUserId, ScreeningActionDto action, CancellationToken ct = default)
    {
        var report = await db.ScreeningReports
            .Include(r => r.Checks.OrderBy(c => c.DisplayOrder))
            .Include(r => r.Application).ThenInclude(a => a.GrantType)
            .Include(r => r.Application).ThenInclude(a => a.Applicant)
            .FirstOrDefaultAsync(r => r.Id == reportId, ct)
            ?? throw new KeyNotFoundException("Screening report not found.");

        var validActions = new[] { "ConfirmEligible", "OverrideIneligible", "ClarificationRequested" };
        if (!validActions.Contains(action.Action))
            throw new ArgumentException($"Invalid action. Must be one of: {string.Join(", ", validActions)}");

        if (action.Action != "ConfirmEligible" && string.IsNullOrWhiteSpace(action.Reason))
            throw new ArgumentException("Reason is required for OverrideIneligible and ClarificationRequested.");

        report.OfficerAction = action.Action;
        report.OfficerActionReason = action.Reason;
        report.ReviewedByUserId = officerUserId;
        report.ReviewedAt = DateTime.UtcNow;
        report.UpdatedAt = DateTime.UtcNow;

        // Update application status based on action
        var app = report.Application;
        switch (action.Action)
        {
            case "ConfirmEligible":
                report.OverallResult = "Eligible";
                app.StatusLabel = "Eligible";
                break;
            case "OverrideIneligible":
                report.OverallResult = "Ineligible";
                app.StatusLabel = "Ineligible";
                break;
            case "ClarificationRequested":
                app.StatusLabel = "ClarificationRequested";
                break;
        }

        app.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);

        return await MapToDto(report, app, ct);
    }

    // ──────────────────── CDG Hard + Soft Checks ────────────────────

    private static void RunCdgChecks(Domain.Entities.Applications.Application app, List<ScreeningCheck> checks)
    {
        // E1 - Organisation Type
        var validOrgTypes = new[] { "NGO", "Trust", "Section 8 Company" };
        var orgTypePass = validOrgTypes.Any(t => app.OrganisationType.Contains(t, StringComparison.OrdinalIgnoreCase));
        checks.Add(HardCheck("E1", "Organisation Type", orgTypePass,
            orgTypePass ? "Organisation type is valid." : $"Must be NGO, Trust, or Section 8 Company. Found: {app.OrganisationType}"));

        // E2 - Minimum Age (2 years)
        var minAge = app.YearOfEstablishment <= DateTime.UtcNow.Year - 2;
        checks.Add(HardCheck("E2", "Minimum Age (2 years)", minAge,
            minAge ? $"Established {app.YearOfEstablishment}, meets 2-year minimum." : $"Established {app.YearOfEstablishment}, does not meet 2-year minimum."));

        // E3 - Geographic Focus (rural/semi-urban) — simplified check
        var hasLocation = !string.IsNullOrWhiteSpace(app.StateOfRegistration);
        checks.Add(HardCheck("E3", "Geographic Focus", hasLocation,
            hasLocation ? $"Project location: {app.StateOfRegistration}" : "Project location not specified."));

        // E4 - Funding Range: INR 2L - 20L
        var fundingInRange = app.TotalRequestedAmount >= 200000 && app.TotalRequestedAmount <= 2000000;
        checks.Add(HardCheck("E4", "Funding Range (₹2L–₹20L)", fundingInRange,
            $"Requested: ₹{app.TotalRequestedAmount:N0}. {(fundingInRange ? "Within range." : "Outside allowed range.")}"));

        // E5 - Project Duration: 6–18 months
        var durationCheck = CheckDuration(app, 6, 18);
        checks.Add(HardCheck("E5", "Project Duration (6–18 months)", durationCheck.pass, durationCheck.detail));

        // E6 - Budget Overhead <= 15%
        var overheadCheck = CheckOverhead(app);
        checks.Add(HardCheck("E6", "Budget Overhead (≤15%)", overheadCheck.pass, overheadCheck.detail));

        // E7 - Budget Total Match
        var budgetMatch = CheckBudgetTotal(app);
        checks.Add(HardCheck("E7", "Budget Total Match", budgetMatch.pass, budgetMatch.detail));

        // E8 - Thematic Alignment (AI Soft)
        var thematicScore = ScoreThematicAlignment(app.ProblemStatement, app.ProposedSolution, app.ProjectTitle, "community development");
        checks.Add(SoftCheck("E8", "Thematic Alignment (AI)", thematicScore >= 60 ? "Pass" : "Flag",
            $"AI thematic alignment score: {thematicScore}%. {(thematicScore >= 60 ? "Aligned with community development themes." : "Low alignment — flagged for human review.")}", thematicScore));

        // E9 - Beneficiary Count (AI Soft)
        var beneficiaryCheck = CheckBeneficiaryCount(app.TargetBeneficiariesCount ?? app.DirectBeneficiariesCount, app.TotalRequestedAmount, 50000);
        checks.Add(SoftCheck("E9", "Beneficiary Count (AI)", beneficiaryCheck.pass ? "Pass" : "Flag",
            beneficiaryCheck.detail, beneficiaryCheck.score));
    }

    // ──────────────────── EIG Hard + Soft Checks ────────────────────

    private static void RunEigChecks(Domain.Entities.Applications.Application app, List<ScreeningCheck> checks)
    {
        // E1 - Organisation Type
        var validTypes = new[] { "NGO", "EdTech", "Non-profit", "Research Institution", "University" };
        var orgTypePass = validTypes.Any(t => app.OrganisationType.Contains(t, StringComparison.OrdinalIgnoreCase));
        checks.Add(HardCheck("E1", "Organisation Type", orgTypePass,
            orgTypePass ? "Organisation type is valid." : $"Must be NGO/EdTech/Research/University. Found: {app.OrganisationType}"));

        // E2 - Minimum 1 year
        var minAge = app.YearOfEstablishment <= DateTime.UtcNow.Year - 1;
        checks.Add(HardCheck("E2", "Minimum Operation Period (1 year)", minAge,
            minAge ? $"Established {app.YearOfEstablishment}, meets 1-year minimum." : $"Established {app.YearOfEstablishment}, does not meet minimum."));

        // E3 - Funding Range: INR 5L - 50L
        var fundingInRange = app.TotalRequestedAmount >= 500000 && app.TotalRequestedAmount <= 5000000;
        checks.Add(HardCheck("E3", "Funding Range (₹5L–₹50L)", fundingInRange,
            $"Requested: ₹{app.TotalRequestedAmount:N0}. {(fundingInRange ? "Within range." : "Outside allowed range.")}"));

        // E4 - Project Duration: 12-24 months
        var durationCheck = CheckDuration(app, 12, 24);
        checks.Add(HardCheck("E4", "Project Duration (12–24 months)", durationCheck.pass, durationCheck.detail));

        // E5 - Schools Targeted >= 5
        var schoolsOk = (app.NumberOfSchoolsTargeted ?? 0) >= 5;
        checks.Add(HardCheck("E5", "Schools Targeted (≥5)", schoolsOk,
            $"Schools targeted: {app.NumberOfSchoolsTargeted ?? 0}. {(schoolsOk ? "Meets minimum." : "Below minimum of 5.")}"));

        // E6 - Grade Coverage
        var gradeOk = !string.IsNullOrWhiteSpace(app.GradeLevelsTargeted);
        checks.Add(HardCheck("E6", "Grade Coverage", gradeOk,
            gradeOk ? $"Grade levels: {app.GradeLevelsTargeted}" : "No grade levels specified."));

        // E7 - Budget Overhead
        var overheadCheck = CheckOverhead(app);
        checks.Add(HardCheck("E7", "Budget Overhead Cap (≤15%)", overheadCheck.pass, overheadCheck.detail));

        // E8 - Budget Total Match
        var budgetMatch = CheckBudgetTotal(app);
        checks.Add(HardCheck("E8", "Budget Total Match", budgetMatch.pass, budgetMatch.detail));

        // E9 - Education Theme Alignment (AI Soft)
        var thematicScore = ScoreThematicAlignment(app.ProblemBeingSolved, app.InnovationDescription, app.ProjectTitle, "education innovation");
        checks.Add(SoftCheck("E9", "Education Theme Alignment (AI)", thematicScore >= 65 ? "Pass" : "Flag",
            $"AI education theme score: {thematicScore}%. {(thematicScore >= 65 ? "Aligned." : "Low alignment — flagged for review.")}", thematicScore));

        // E10 - Impact Measurement Plan (AI Soft)
        var planScore = ScoreMeasurementPlan(app.MeasurementPlan);
        checks.Add(SoftCheck("E10", "Impact Measurement Plan (AI)", planScore >= 60 ? "Pass" : "Flag",
            $"AI plan quality score: {planScore}%. {(planScore >= 60 ? "Measurable plan detected." : "Plan appears vague — flagged for review.")}", planScore));
    }

    // ──────────────────── ECAG Hard + Soft Checks ────────────────────

    private static void RunEcagChecks(Domain.Entities.Applications.Application app, List<ScreeningCheck> checks)
    {
        // E1 - Organisation Type
        var validTypes = new[] { "NGO", "FPO", "Farmer Producer", "Panchayat", "Research Institution" };
        var orgTypePass = validTypes.Any(t => app.OrganisationType.Contains(t, StringComparison.OrdinalIgnoreCase));
        checks.Add(HardCheck("E1", "Organisation Type", orgTypePass,
            orgTypePass ? "Organisation type is valid." : $"Must be NGO/FPO/Panchayat/Research. Found: {app.OrganisationType}"));

        // E2 - Funding Range: INR 3L - 30L
        var fundingInRange = app.TotalRequestedAmount >= 300000 && app.TotalRequestedAmount <= 3000000;
        checks.Add(HardCheck("E2", "Funding Range (₹3L–₹30L)", fundingInRange,
            $"Requested: ₹{app.TotalRequestedAmount:N0}. {(fundingInRange ? "Within range." : "Outside allowed range.")}"));

        // E3 - Duration: 6-24 months
        var durationCheck = CheckDuration(app, 6, 24);
        checks.Add(HardCheck("E3", "Duration (6–24 months)", durationCheck.pass, durationCheck.detail));

        // E4 - Budget Overhead
        var overheadCheck = CheckOverhead(app);
        checks.Add(HardCheck("E4", "Budget Overhead Cap (≤15%)", overheadCheck.pass, overheadCheck.detail));

        // E5 - Budget Total Match
        var budgetMatch = CheckBudgetTotal(app);
        checks.Add(HardCheck("E5", "Budget Total Match", budgetMatch.pass, budgetMatch.detail));

        // E6 - Geographic Priority (AI Soft — flag but don't reject)
        var geoScore = ScoreGeographicPriority(app.StateOfRegistration, app.GeographicCoverage);
        checks.Add(SoftCheck("E6", "Geographic Priority Check (AI)", geoScore >= 60 ? "Pass" : "Flag",
            $"AI geographic priority score: {geoScore}%. {(geoScore >= 60 ? "Climate-vulnerable district identified." : "Non-priority area — flagged but not rejected.")}", geoScore));

        // E7 - Environmental Theme Alignment (AI Soft)
        var thematicScore = ScoreThematicAlignment(app.EnvironmentalProblemDesc, app.ProposedIntervention, app.ProjectTitle, "environment climate conservation");
        checks.Add(SoftCheck("E7", "Environmental Theme Alignment (AI)", thematicScore >= 60 ? "Pass" : "Flag",
            $"AI environmental theme score: {thematicScore}%. {(thematicScore >= 60 ? "Aligned." : "Low alignment — flagged for review.")}", thematicScore));

        // E8 - Community Involvement (AI Soft)
        var communityScore = ScoreCommunityInvolvement(app.CommunityInvolvementPlan);
        checks.Add(SoftCheck("E8", "Community Involvement (AI)", communityScore >= 60 ? "Pass" : "Flag",
            $"AI community involvement score: {communityScore}%. {(communityScore >= 60 ? "Substantive plan found." : "Plan appears insufficient — flagged for review.")}", communityScore));
    }

    // ──────────────────── Shared Check Helpers ────────────────────

    private static (bool pass, string detail) CheckDuration(Domain.Entities.Applications.Application app, int minMonths, int maxMonths)
    {
        if (app.ProjectStartDate == null || app.ProjectEndDate == null)
            return (false, "Start date or end date not specified.");

        var months = ((app.ProjectEndDate.Value.Year - app.ProjectStartDate.Value.Year) * 12)
                     + (app.ProjectEndDate.Value.Month - app.ProjectStartDate.Value.Month);
        var pass = months >= minMonths && months <= maxMonths;
        return (pass, $"Duration: {months} months. {(pass ? "Within range." : $"Must be {minMonths}–{maxMonths} months.")}");
    }

    private static (bool pass, string detail) CheckOverhead(Domain.Entities.Applications.Application app)
    {
        if (app.TotalRequestedAmount <= 0)
            return (false, "Total requested amount is zero.");

        var pct = (app.Overheads / app.TotalRequestedAmount) * 100;
        var pass = pct <= 15;
        return (pass, $"Overheads: ₹{app.Overheads:N0} ({pct:N1}% of total). {(pass ? "Within 15% limit." : "Exceeds 15% limit.")}");
    }

    private static (bool pass, string detail) CheckBudgetTotal(Domain.Entities.Applications.Application app)
    {
        var sum = app.PersonnelCosts + app.EquipmentAndMaterials + app.TravelAndLogistics
                  + app.TrainingAndWorkshops + app.TechnologySoftware + app.ContentDevelopment
                  + app.SaplingsAndSeeds + app.CommunityEngagementWages
                  + app.Overheads + app.OtherCosts;
        var diff = Math.Abs(sum - app.TotalRequestedAmount);
        var pass = diff <= 500;
        return (pass, $"Budget lines sum: ₹{sum:N0}. Total requested: ₹{app.TotalRequestedAmount:N0}. Difference: ₹{diff:N0}. {(pass ? "Within ±₹500 tolerance." : "Exceeds ±₹500 tolerance.")}");
    }

    // ──────────────────── AI Scoring Helpers (Heuristic) ────────────────────
    // In production these would call an LLM. Here we use keyword-based heuristics.

    private static int ScoreThematicAlignment(string? problemText, string? solutionText, string? title, string themeKeywords)
    {
        var combined = $"{title} {problemText} {solutionText}".ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(combined)) return 20;

        var keywords = themeKeywords.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var matchCount = keywords.Count(k => combined.Contains(k));
        var wordCount = combined.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

        // Base score from keyword density
        var keywordScore = Math.Min(100, (matchCount * 25));
        // Length bonus — longer, more detailed text scores higher
        var lengthBonus = Math.Min(30, wordCount / 5);

        return Math.Min(100, keywordScore + lengthBonus);
    }

    private static (bool pass, string detail, int score) CheckBeneficiaryCount(int? count, decimal totalAmount, decimal maxCostPerBeneficiary)
    {
        if (count == null || count <= 0)
            return (false, "Beneficiary count is zero or not specified.", 0);

        var costPer = totalAmount / count.Value;
        var pass = costPer < maxCostPerBeneficiary;
        var score = pass ? 80 : 40;
        return (pass, $"Beneficiaries: {count}. Cost per beneficiary: ₹{costPer:N0}. {(pass ? $"Below ₹{maxCostPerBeneficiary:N0} threshold." : $"Exceeds ₹{maxCostPerBeneficiary:N0} — flagged.")}", score);
    }

    private static int ScoreMeasurementPlan(string? plan)
    {
        if (string.IsNullOrWhiteSpace(plan)) return 20;
        var lower = plan.ToLowerInvariant();
        var indicatorKeywords = new[] { "indicator", "measure", "baseline", "target", "assessment", "evaluate", "outcome", "metric", "survey", "test score" };
        var matches = indicatorKeywords.Count(k => lower.Contains(k));
        var wordCount = plan.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
        return Math.Min(100, (matches * 15) + Math.Min(30, wordCount / 3));
    }

    private static int ScoreGeographicPriority(string? state, string? coverage)
    {
        var combined = $"{state} {coverage}".ToLowerInvariant();
        var climateKeywords = new[] { "coastal", "drought", "flood", "hill", "cyclone", "erosion", "sundarbans", "kutch", "vidarbha", "bundelkhand", "arid" };
        var matches = climateKeywords.Count(k => combined.Contains(k));
        return matches > 0 ? 80 : 45;
    }

    private static int ScoreCommunityInvolvement(string? plan)
    {
        if (string.IsNullOrWhiteSpace(plan)) return 15;
        var lower = plan.ToLowerInvariant();
        var keywords = new[] { "community", "village", "panchayat", "gram sabha", "participation", "co-design", "stakeholder", "meeting", "consent", "women", "youth" };
        var matches = keywords.Count(k => lower.Contains(k));
        var wordCount = plan.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
        return Math.Min(100, (matches * 12) + Math.Min(25, wordCount / 4));
    }

    // ──────────────────── Check Constructors ────────────────────

    private static ScreeningCheck HardCheck(string code, string name, bool pass, string details) => new()
    {
        Id = Guid.NewGuid(),
        CheckCode = code,
        CheckName = name,
        CheckType = "Hard",
        Result = pass ? "Pass" : "Fail",
        Details = details
    };

    private static ScreeningCheck SoftCheck(string code, string name, string result, string details, int? aiScore = null) => new()
    {
        Id = Guid.NewGuid(),
        CheckCode = code,
        CheckName = name,
        CheckType = "Soft",
        Result = result,
        Details = details,
        AiScore = aiScore
    };

    // ──────────────────── Mapping ────────────────────

    private async Task<ScreeningReportDto> MapToDto(ScreeningReport report, Domain.Entities.Applications.Application app, CancellationToken ct)
    {
        var reviewer = report.ReviewedByUserId != null
            ? await db.Users.FindAsync(new object[] { report.ReviewedByUserId.Value }, ct)
            : null;

        return new ScreeningReportDto
        {
            Id = report.Id,
            ApplicationId = app.Id,
            ApplicationReference = app.ReferenceNumber,
            ApplicationTitle = app.ProjectTitle,
            GrantTypeCode = app.GrantType?.Code ?? "",
            ApplicantName = app.Applicant?.Name ?? "",
            OrganisationName = app.LegalName,
            OverallResult = report.OverallResult,
            HardChecksPassed = report.HardChecksPassed,
            HardChecksFailed = report.HardChecksFailed,
            SoftFlags = report.SoftFlags,
            OfficerAction = report.OfficerAction,
            OfficerActionReason = report.OfficerActionReason,
            ReviewedByUserName = reviewer?.Name,
            ReviewedAt = report.ReviewedAt,
            GeneratedAt = report.GeneratedAt,
            Checks = report.Checks.Select(c => new ScreeningCheckDto
            {
                Id = c.Id,
                CheckCode = c.CheckCode,
                CheckName = c.CheckName,
                CheckType = c.CheckType,
                Result = c.Result,
                Details = c.Details,
                AiScore = c.AiScore,
                DisplayOrder = c.DisplayOrder
            }).OrderBy(c => c.DisplayOrder).ToList()
        };
    }
}
