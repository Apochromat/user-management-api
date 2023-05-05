using Microsoft.Extensions.DependencyInjection;
using UserManagement.DAL;

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
        return services;
    }
}