using Cashback.Application;
using Cashback.Infrastructure;
using Cashback.Persistence;

namespace Cashback.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers();
        services.AddOpenApi();

        services
            .AddApplication()
            .AddInfrastructure(configuration)
            .AddPersistence(configuration);

        return services;
    }
}
