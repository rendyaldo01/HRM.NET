using System.ComponentModel.DataAnnotations;

namespace HRM.Application.DTOs.Departments;

public class DepartmentRequest
{
    [Required]
    [StringLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
}