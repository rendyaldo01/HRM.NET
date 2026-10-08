using System.ComponentModel.DataAnnotations;

namespace HRM.Application.DTOs.Employees;

public class CreateEmployeeRequest
{
    [Required]
    [StringLength(50)]
    public string EmployeeNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public DateTime HireDate { get; set; }
}