using HRM.Application.Services.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HRM.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IEmployeeService _employeeService;

    public HomeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public async Task<IActionResult> Index()
    {
        var employeeCount = await _employeeService.GetCountAsync();

        ViewBag.EmployeeCount = employeeCount;

        return View();
    }
}