using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Common.AuthenticationHandlers;

namespace UserManagement.BLL.Extensions; 

/// <summary>
/// Extension for adding dependencies
/// </summary>
public static class AddBasicAuthExtension {
    /// <summary>
    /// Add business logic layer dependencies
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddBasicAuth(this IServiceCollection services) {
        services.AddAuthentication()
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);
        return services;
    }
}