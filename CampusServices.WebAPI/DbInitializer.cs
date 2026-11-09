using CampusServices.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CampusServices.WebAPI;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceScopeFactory scopeFactory)
    {
        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        try
        {
            // Direct Connection Check & Table Creation Safe Execution
            await context.Database.EnsureCreatedAsync();
        }
        catch (Exception ex)
        {
            // Suppress Pooler DDL Exception if tables already exist on Supabase
            Console.WriteLine($"Database Migration Note: {ex.Message}");
        }

        try
        {
            string[] roleNames = ["Student", "Staff"];
            foreach (var roleName in roleNames)
            {
                var roleExists = await roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var studentUser = await userManager.FindByEmailAsync("student@campus.edu");
            if (studentUser == null)
            {
                var student = new IdentityUser { UserName = "student@campus.edu", Email = "student@campus.edu", EmailConfirmed = true };
                await userManager.CreateAsync(student, "Student123!");
                await userManager.AddToRoleAsync(student, "Student");
            }

            var staffUser = await userManager.FindByEmailAsync("staff@campus.edu");
            if (staffUser == null)
            {
                var staff = new IdentityUser { UserName = "staff@campus.edu", Email = "staff@campus.edu", EmailConfirmed = true };
                await userManager.CreateAsync(staff, "Staff123!");
                await userManager.AddToRoleAsync(staff, "Staff");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Seeding Note: {ex.Message}");
        }
    }
}