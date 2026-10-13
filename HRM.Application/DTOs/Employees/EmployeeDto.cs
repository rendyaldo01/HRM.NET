namespace HRM.Application.DTOs.Employees;

public class EmployeeDto
{
    public Guid Id { get; set; }

    public string EmployeeNumber { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime HireDate { get; set; }

    public Guid? DepartmentId { get; set; }

    public string? DepartmentName { get; set; }

    public Guid? PositionId { get; set; }

    public string? PositionName { get; set; }

    public bool IsActive { get; set; }
}