
using HRM.Application.DTOs.RfidCards;
using HRM.Application.Services.RfidCards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Web.Controllers.Api;

[ApiController]
[Route("api/rfid-cards")]
[Authorize(Roles = "Admin HR,HR Staff")]
public class RfidCardsController : ControllerBase
{
    private readonly IRfidCardService _rfidCardService;

    public RfidCardsController(IRfidCardService rfidCardService)
    {
        _rfidCardService = rfidCardService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var card = await _rfidCardService.GetByIdAsync(id);

        if (card == null)
        {
            return NotFound(new
            {
                message = "RFID card not found."
            });
        }

        return Ok(card);
    }

    [HttpGet("uid/{cardUid}")]
    public async Task<IActionResult> GetByCardUid(string cardUid)
    {
        var card = await _rfidCardService.GetByCardUidAsync(cardUid);

        if (card == null)
        {
            return NotFound(new
            {
                message = "RFID card not found."
            });
        }

        return Ok(card);
    }

    [HttpGet("employee/{employeeId:guid}")]
    public async Task<IActionResult> GetAllByEmployeeId(
        Guid employeeId)
    {
        var cards = await _rfidCardService
            .GetAllByEmployeeIdAsync(employeeId);

        return Ok(cards);
    }

    [HttpPost]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRfidCardRequest request)
    {
        try
        {
            var card = await _rfidCardService.RegisterAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = card.Id },
                card);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPost("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        try
        {
            await _rfidCardService.DeactivateAsync(id);

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}
