using Cashback.Api.Authorization;
using Cashback.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Cashback.Api.Extensions;

/// <summary>
/// Authorization policy configuration extensions.
/// </summary>
public static class AuthorizationExtensions
{
    /// <summary>
    /// Registers role-based authorization policies.
    /// </summary>
    public static IServiceCollection AddAdminAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                AuthorizationPolicies.AdminOnly,
                policy => policy.RequireRole(nameof(UserRole.Admin)));
        });

        return services;
    }
}
