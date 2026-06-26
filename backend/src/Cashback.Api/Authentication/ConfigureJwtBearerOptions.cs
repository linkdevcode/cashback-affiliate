using System.Security.Claims;
using System.Text;
using Cashback.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Cashback.Api.Authentication;

/// <summary>
/// Configures JWT bearer authentication from application JWT settings.
/// </summary>
internal sealed class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly IOptions<JwtOptions> _jwtOptions;

    /// <summary>
    /// Initializes a new instance of the JWT bearer options configurator.
    /// </summary>
    public ConfigureJwtBearerOptions(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions;
    }

    /// <inheritdoc/>
    public void Configure(JwtBearerOptions options)
    {
        Configure(JwtBearerDefaults.AuthenticationScheme, options);
    }

    /// <inheritdoc/>
    public void Configure(string? name, JwtBearerOptions options)
    {
        if (!string.Equals(name, JwtBearerDefaults.AuthenticationScheme, StringComparison.Ordinal))
        {
            return;
        }

        var jwtOptions = _jwtOptions.Value;

        if (string.IsNullOrWhiteSpace(jwtOptions.Secret))
        {
            throw new InvalidOperationException("JWT secret is not configured.");
        }

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtOptions.Secret)),
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero
        };
    }
}
