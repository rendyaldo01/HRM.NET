using HRM.Application.DTOs.Employees;
using HRM.Application.Services.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HRM.Web.Controllers;

[Authorize(Roles = "Admin HR,HR Staff,Viewer")]
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


    [Authorize(Roles = "Admin HR,HR Staff")]
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadDepartmentAndPositionAsync();
        return View();
    }

    [Authorize(Roles = "Admin HR,HR Staff")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEmployeeRequest request)
    {
        if (!ModelState.IsValid)
        {
            await LoadDepartmentAndPositionAsync();
            return View(request);
        }

        try
        {
            await _employeeService.CreateAsync(request);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);

            await LoadDepartmentAndPositionAsync();
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


    [Authorize(Roles = "Admin HR,HR Staff")]
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var employee = await _employeeService.GetByIdAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        var request = new CreateEmployeeRequest
        {
            EmployeeNumber = employee.EmployeeNumber,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            HireDate = employee.HireDate,
            DepartmentId = employee.DepartmentId,
            PositionId = employee.PositionId
        };
        await LoadDepartmentAndPositionAsync();

        return View(request);
    }


    [Authorize(Roles = "Admin HR,HR Staff")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CreateEmployeeRequest request)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.EmployeeId = id;
            await LoadDepartmentAndPositionAsync();
            return View(request);
        }

        try
        {
            await _employeeService.UpdateAsync(id, request);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);

            ViewBag.EmployeeId = id;
            await LoadDepartmentAndPositionAsync();
            return View(request);
        }

        return RedirectToAction(nameof(Index));
    }


    [Authorize(Roles = "Admin HR,HR Staff")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        try
        {
            await _employeeService.DeactivateAsync(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin HR,HR Staff")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Activate(Guid id)
    {
        try
        {
            await _employeeService.ActivateAsync(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task LoadDepartmentAndPositionAsync()
    {
        ViewBag.Departments = await _employeeService.GetDepartmentsAsync();
        ViewBag.Positions = await _employeeService.GetPositionsAsync();
    }

}