using Cashback.Application.Interfaces;
using Cashback.Infrastructure.Clients;
using Cashback.Infrastructure.Services;
using Cashback.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cashback.Infrastructure;

/// <summary>
/// Infrastructure layer dependency injection configuration.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers infrastructure layer services.
    /// </summary>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<GoogleOptions>(configuration.GetSection(GoogleOptions.SectionName));
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

        services.AddScoped<IGoogleAuthService, GoogleAuthService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
}
