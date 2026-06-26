using Cashback.Application;
using Cashback.Application.Interfaces;
using Cashback.Api.Services;
using Cashback.Infrastructure;
using Cashback.Persistence;

namespace Cashback.Api.Extensions;

/// <summary>
/// API service registration extensions.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers API and application services.
    /// </summary>
    public static IServiceCollection AddApiServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers();
        services.AddOpenApi();

        services.AddJwtAuthentication(configuration);
        services.AddAdminAuthorizationPolicies();

        services.AddFrontendCors();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services
            .AddApplication()
            .AddInfrastructure(configuration)
            .AddPersistence(configuration);

        return services;
    }
}
