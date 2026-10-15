
using HRM.Application.Services.RfidCards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Web.Controllers;

[Authorize(Roles = "Admin HR")]
public class RfidCardsController : Controller
{
    private readonly IRfidCardService _rfidCardService;

    public RfidCardsController(
        IRfidCardService rfidCardService)
    {
        _rfidCardService = rfidCardService;
    }

    public async Task<IActionResult> Index()
    {
        var cards = await _rfidCardService.GetAllAsync();

        return View(cards);
    }
}
