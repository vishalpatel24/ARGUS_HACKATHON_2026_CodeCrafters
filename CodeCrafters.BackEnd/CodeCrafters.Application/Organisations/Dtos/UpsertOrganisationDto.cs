using System.ComponentModel.DataAnnotations;

namespace CodeCrafters.Application.Organisations.Dtos;

public sealed class UpsertOrganisationDto
{
    [Required]
    [StringLength(200)]
    public string Name { get; init; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string RegistrationNumber { get; init; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Type { get; init; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string State { get; init; } = string.Empty;

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Annual budget must be a positive value.")]
    public decimal AnnualBudget { get; init; }

    [Required]
    [StringLength(200)]
    public string ContactPersonName { get; init; } = string.Empty;

    [Required]
    [EmailAddress]
    public string ContactPersonEmail { get; init; } = string.Empty;

    [Required]
    [Phone]
    public string ContactPersonPhone { get; init; } = string.Empty;

    [Required]
    [Range(1850, 2026, ErrorMessage = "Year of establishment must be between 1850 and 2026.")]
    public int YearOfEstablishment { get; init; }
}
