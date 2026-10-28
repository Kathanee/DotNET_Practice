namespace EmployeeManagement.Api.Models;

public sealed record Employee(
    int Id,
    string FullName,
    string Email,
    string Department,
    decimal Salary);
