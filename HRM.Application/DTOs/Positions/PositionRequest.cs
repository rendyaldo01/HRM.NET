using System.ComponentModel.DataAnnotations;

namespace HRM.Application.DTOs.Positions;

public class PositionRequest
{
    [Required(ErrorMessage = "Position code is required.")]
    [StringLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required(ErrorMessage = "Position name is required.")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
}