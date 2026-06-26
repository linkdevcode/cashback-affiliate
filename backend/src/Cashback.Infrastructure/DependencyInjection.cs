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
        return services;
    }
}
