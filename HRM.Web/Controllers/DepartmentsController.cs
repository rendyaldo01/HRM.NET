using HRM.Application.DTOs.Departments;
using HRM.Application.Services.Departments;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Web.Controllers;

public class DepartmentsController : Controller
{
    private readonly IDepartmentService _departmentService;

    public DepartmentsController(
        IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    public async Task<IActionResult> Index()
    {
        var departments =
            await _departmentService.GetAllAsync();

        return View(departments);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DepartmentRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        try
        {
            await _departmentService.CreateAsync(request);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                nameof(request.Code),
                ex.Message);

            return View(request);
        }

        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var department = await _departmentService.GetByIdAsync(id);

        if (department == null)
        {
            return NotFound();
        }

        var request = new DepartmentRequest
        {
            Code = department.Code,
            Name = department.Name
        };

        return View(request);
    }

    [HttpPost]  
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id,DepartmentRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        try
        {
            await _departmentService.UpdateAsync(id, request);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        try
        {
            await _departmentService.DeactivateAsync(id);
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
            await _departmentService.ActivateAsync(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }
}