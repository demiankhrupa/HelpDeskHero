using HelpDeskHero.Api.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using HelpDeskHero.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskHero.Api.Infrastructure.Services;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roles = ["Admin", "Manager", "Agent", "User"];

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        const string adminUserName = "admin";
        var admin = await userManager.FindByNameAsync(adminUserName);

        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = adminUserName,
                Email = "admin@helpdeskhero.local",
                DisplayName = "System Admin",
                IsActive = true,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(admin, "Admin1234");
            if (createResult.Succeeded)
            {
                await userManager.AddToRolesAsync(admin, ["Admin", "Agent"]);
            }
        }

        const string agentUserName = "agent";
        var agent = await userManager.FindByNameAsync(agentUserName);

        if (agent is null)
        {
            agent = new ApplicationUser
            {
                UserName = agentUserName,
                Email = "agent@helpdeskhero.local",
                DisplayName = "Support Agent",
                IsActive = true,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(agent, "Agent1234");
            if (createResult.Succeeded)
            {
                await userManager.AddToRoleAsync(agent, "Agent");
            }
        }
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

if (!db.TicketSlaPolicies.Any())
{
    db.TicketSlaPolicies.AddRange(
        new TicketSlaPolicy
        {
            Name = "Low SLA",
            Priority = "Low",
            FirstResponseMinutes = 240,
            ResolveMinutes = 2880,
            IsActive = true
        },
        new TicketSlaPolicy
        {
            Name = "Medium SLA",
            Priority = "Medium",
            FirstResponseMinutes = 60,
            ResolveMinutes = 480,
            IsActive = true
        },
        new TicketSlaPolicy
        {
            Name = "High SLA",
            Priority = "High",
            FirstResponseMinutes = 15,
            ResolveMinutes = 120,
            IsActive = true
        });

    await db.SaveChangesAsync();
}
    }
}