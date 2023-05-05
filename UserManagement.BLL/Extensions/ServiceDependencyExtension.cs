using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.BLL.Services;
using UserManagement.Common.Interfaces;
using UserManagement.DAL;
using UserManagement.DAL.Entities;

namespace UserManagement.BLL.Extensions; 

/// <summary>
/// Extension for adding dependencies
/// </summary>
public static class ServiceDependencyExtension {
    /// <summary>
    /// Add business logic layer dependencies
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddBllDependencies(this IServiceCollection services) {
        services.AddDbContext<ApplicationDbContext>();
        services.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddSignInManager<SignInManager<User>>()
            .AddUserManager<UserManager<User>>()
            .AddRoleManager<RoleManager<IdentityRole<Guid>>>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}