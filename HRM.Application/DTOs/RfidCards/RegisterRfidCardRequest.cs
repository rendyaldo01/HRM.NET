
namespace HRM.Application.DTOs.RfidCards;

public class RegisterRfidCardRequest
{
    public Guid EmployeeId { get; set; }
    public string CardUid { get; set; } = string.Empty;
}
