using System.Net.Http.Headers;
using Cashback.Application.Interfaces;
using Cashback.Infrastructure.Clients;
using Cashback.Infrastructure.Providers;
using Cashback.Infrastructure.Services;
using Cashback.Infrastructure.Settings;
using Cashback.Infrastructure.Webhooks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
        services.Configure<AccesstradeOptions>(configuration.GetSection(AccesstradeOptions.SectionName));
        services.Configure<CashbackOptions>(configuration.GetSection(CashbackOptions.SectionName));
        services.Configure<WithdrawalOptions>(configuration.GetSection(WithdrawalOptions.SectionName));

        services.AddScoped<IGoogleAuthService, GoogleAuthService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddSingleton<IAccesstradeWebhookSettings, AccesstradeWebhookSettings>();
        services.AddSingleton<ICashbackSettings, CashbackSettings>();
        services.AddSingleton<IWithdrawalSettings, WithdrawalSettings>();
        services.AddSingleton<IWebhookValidator, AccesstradeWebhookValidator>();

        services.AddHttpClient<IAffiliateProvider, AccesstradeProvider>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<AccesstradeOptions>>().Value;

            if (!string.IsNullOrWhiteSpace(options.BaseUrl))
            {
                client.BaseAddress = new Uri(options.BaseUrl.TrimEnd('/') + "/");
            }

            if (!string.IsNullOrWhiteSpace(options.Token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Token", options.Token);
            }

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        });

        return services;
    }
}
