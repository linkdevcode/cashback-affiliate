using Cashback.Api.Authentication;
using Cashback.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Cashback.Api.Extensions;

/// <summary>
/// JWT authentication configuration extensions.
/// </summary>
public static class AuthenticationExtensions
{
    /// <summary>
    /// Registers JWT bearer authentication.
    /// </summary>
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
            ?? throw new InvalidOperationException("JWT configuration is missing.");

        if (string.IsNullOrWhiteSpace(jwtOptions.Secret))
        {
            throw new InvalidOperationException("JWT secret is not configured.");
        }

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();

        return services;
    }
}
