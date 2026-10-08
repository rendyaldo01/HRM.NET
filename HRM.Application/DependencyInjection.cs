using HRM.Application.Services.Employees;
using Microsoft.Extensions.DependencyInjection;

namespace HRM.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();

        return services;
    }
}