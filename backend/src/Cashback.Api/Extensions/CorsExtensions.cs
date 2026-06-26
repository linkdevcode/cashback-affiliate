namespace Cashback.Api.Extensions;

/// <summary>
/// CORS configuration extensions.
/// </summary>
public static class CorsExtensions
{
    /// <summary>
    /// Frontend development CORS policy name.
    /// </summary>
    public const string FrontendPolicy = "Frontend";

    /// <summary>
    /// Registers CORS for local frontend development.
    /// </summary>
    public static IServiceCollection AddFrontendCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(FrontendPolicy, policy =>
            {
                policy
                    .WithOrigins(
                        "http://localhost:3000",
                        "http://127.0.0.1:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }
}
