using HRM.Application.Interfaces.Repositories;
using HRM.Infrastructure.Persistence;
using HRM.Infrastructure.Persistence.Repositories;
using HRM.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HRM.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(
        configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IPositionRepository, PositionRepository>();
        services.AddScoped<IRfidCardRepository, RfidCardRepository>();
        services.AddScoped<IAttendanceDeviceRepository, AttendanceDeviceRepository>();
        services.AddScoped<IAttendanceEventRepository,AttendanceEventRepository>();

        return services;
    }
}