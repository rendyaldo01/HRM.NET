using HRM.Application.DTOs.Employees;
using HRM.Application.Services.Employees;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Web.Controllers;

public class EmployeesController : Controller
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public async Task<IActionResult> Index()
    {
        var employees = await _employeeService.GetAllAsync();

        return View(employees);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateEmployeeRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        try
        {
            await _employeeService.CreateAsync(request);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                nameof(request.EmployeeNumber),
                ex.Message);

            return View(request);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var employee = await _employeeService.GetByIdAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        return View(employee);
    }
}