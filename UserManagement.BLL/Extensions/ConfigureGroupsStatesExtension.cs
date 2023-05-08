using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Common.Enumerations;
using UserManagement.DAL;
using UserManagement.DAL.Entities;

namespace UserManagement.BLL.Extensions; 

/// <summary>
/// Extension for configuring the database.
/// </summary>
public static class ConfigureGroupsStatesExtension {
    /// <summary>
    /// Method for creating groups and states.
    /// </summary>
    /// <param name="app"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static async Task ConfigureGroupsStatesAsync(this WebApplication app) {
        using var serviceScope = app.Services.CreateScope();

        // Migrate database
        var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
        if (context == null) {
            throw new ArgumentNullException(nameof(context));
        }
        await context.Database.MigrateAsync();
        
        // Create groups
        foreach (var groupCode in Enum.GetValues(typeof(GroupCode)).Cast<GroupCode>()) {
            if (await context.UserGroups.CountAsync(g => g.Code == groupCode) == 0) {
                await context.UserGroups.AddAsync(new UserGroup() {
                    Id = Guid.NewGuid(),
                    Code = groupCode,
                    Description = groupCode.ToString()
                });
            }
        }

        // Create states
        foreach (var stateCode in Enum.GetValues(typeof(StateCode)).Cast<StateCode>()) {
            if (await context.UserStates.CountAsync(s => s.Code == stateCode) == 0) {
                await context.UserStates.AddAsync(new UserState() {
                    Id = Guid.NewGuid(),
                    Code = stateCode,
                    Description = stateCode.ToString()
                });
            }
        }

        await context.SaveChangesAsync();
    }
}