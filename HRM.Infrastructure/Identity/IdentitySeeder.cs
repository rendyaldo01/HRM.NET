using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HRM.Infrastructure.Identity;

public static class IdentitySeeder
{
    public static async Task SeedAsync(
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        using var scope = serviceProvider.CreateScope();

        var roleManager = scope.ServiceProvider
            .GetRequiredService<RoleManager<IdentityRole>>();

        var userManager = scope.ServiceProvider
            .GetRequiredService<UserManager<IdentityUser>>();

        var roleNames = new[] { "Admin HR", "HR Staff", "Viewer" };

        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var roleResult = await roleManager.CreateAsync(
                    new IdentityRole(roleName));

                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Gagal membuat role {roleName}: " +
                        string.Join("; ", roleResult.Errors.Select(x => x.Description)));
                }
            }
        }

        var adminSection = configuration.GetSection("SeedAdmin");

        var userName = adminSection["UserName"];
        var email = adminSection["Email"];
        var password = adminSection["Password"];

        if (string.IsNullOrWhiteSpace(userName) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                "Konfigurasi SeedAdmin belum lengkap. " +
                "Isi UserName, Email, dan Password pada konfigurasi lokal.");
        }

        var adminUser = await userManager.FindByNameAsync(userName);

        if (adminUser is null)
        {
            adminUser = new IdentityUser
            {
                UserName = userName,
                Email = email,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(
                adminUser,
                password);

            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Gagal membuat akun Admin: " +
                    string.Join("; ", createResult.Errors.Select(x => x.Description)));
            }
        }


        const string adminRole = "Admin HR";

        if (!await userManager.IsInRoleAsync(adminUser, adminRole))
        {
            var roleResult = await userManager.AddToRoleAsync(
                adminUser,
                adminRole);

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Gagal menambahkan role Admin HR: " +
                    string.Join("; ", roleResult.Errors.Select(x => x.Description)));
            }
        }


        const string viewerUserName = "viewer";
        const string viewerEmail = "viewer@hrm.local";
        const string viewerRole = "Viewer";

        // Cari akun Viewer
        var viewerUser = await userManager.FindByNameAsync(viewerUserName);

        if (viewerUser is null)
        {
            viewerUser = new IdentityUser
            {
                UserName = viewerUserName,
                Email = viewerEmail,
                EmailConfirmed = true
            };

            // Gunakan password khusus lokal melalui konfigurasi
            var viewerPassword = configuration["SeedAdmin:ViewerPassword"];

            if (string.IsNullOrWhiteSpace(viewerPassword))
            {
                throw new InvalidOperationException(
                    "Konfigurasi SeedAdmin:ViewerPassword belum diisi.");
            }

            var createViewerResult = await userManager.CreateAsync(
                viewerUser,
                viewerPassword);

            if (!createViewerResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Gagal membuat akun Viewer: " +
                    string.Join("; ", createViewerResult.Errors.Select(x => x.Description)));
            }
        }

        if (!await userManager.IsInRoleAsync(viewerUser, viewerRole))
        {
            var addViewerRoleResult = await userManager.AddToRoleAsync(
                viewerUser,
                viewerRole);

            if (!addViewerRoleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Gagal memberikan role Viewer: " +
                    string.Join("; ", addViewerRoleResult.Errors.Select(x => x.Description)));
            }
        }



        const string hrStaffUserName = "hrstaff";
        const string hrStaffEmail = "hrstaff@hrm.local";
        const string hrStaffRole = "HR Staff";

        var hrStaffUser = await userManager.FindByNameAsync(hrStaffUserName);

        if (hrStaffUser is null)
        {
            var hrStaffPassword = configuration["SeedAdmin:HRStaffPassword"];

            if (string.IsNullOrWhiteSpace(hrStaffPassword))
            {
                throw new InvalidOperationException(
                    "Konfigurasi SeedAdmin:HRStaffPassword belum diisi.");
            }

            hrStaffUser = new IdentityUser
            {
                UserName = hrStaffUserName,
                Email = hrStaffEmail,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(
                hrStaffUser,
                hrStaffPassword);

            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Gagal membuat akun HR Staff: " +
                    string.Join("; ", createResult.Errors.Select(x => x.Description)));
            }
        }

        if (!await userManager.IsInRoleAsync(hrStaffUser, hrStaffRole))
        {
            var roleResult = await userManager.AddToRoleAsync(
                hrStaffUser,
                hrStaffRole);

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Gagal memberikan role HR Staff: " +
                    string.Join("; ", roleResult.Errors.Select(x => x.Description)));
            }
        }


    }
}
