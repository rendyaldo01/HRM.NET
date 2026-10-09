using HRM.Application.DTOs.Positions;
using HRM.Application.Services.Positions;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Web.Controllers;

public class PositionsController : Controller
{
    private readonly IPositionService _positionService;

    public PositionsController(IPositionService positionService)
    {
        _positionService = positionService;
    }

    public async Task<IActionResult> Index()
    {
        var positions = await _positionService.GetAllAsync();
        return View(positions);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PositionRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        try
        {
            await _positionService.CreateAsync(request);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(nameof(request.Code), ex.Message);
            return View(request);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var position = await _positionService.GetByIdAsync(id);

        if (position is null)
            return NotFound();

        var request = new PositionRequest
        {
            Code = position.Code,
            Name = position.Name
        };

        return View(request);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, PositionRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        try
        {
            await _positionService.UpdateAsync(id, request);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(nameof(request.Code), ex.Message);
            return View(request);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        try
        {
            await _positionService.DeactivateAsync(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Activate(Guid id)
    {
        try
        {
            await _positionService.ActivateAsync(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }
}
