using HRM.Application.Services.Employees;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Web.Controllers;

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