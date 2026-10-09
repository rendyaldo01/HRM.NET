using HRM.Application.Services.Departments;
using HRM.Application.Services.Employees;
using HRM.Application.Services.Positions;
using HRM.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HRM.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IPositionService, PositionService>();

        return services;
    }
}
