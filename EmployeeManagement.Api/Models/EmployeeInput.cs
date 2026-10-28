using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Api.Models;

public sealed class EmployeeInput
{
    [Required, StringLength(100, MinimumLength = 2)]
    public string FullName { get; init; } = string.Empty;

    [Required, EmailAddress, StringLength(200)]
    public string Email { get; init; } = string.Empty;

    [Required, StringLength(80)]
    public string Department { get; init; } = string.Empty;

    [Range(0, 10_000_000)]
    public decimal Salary { get; init; }
}
