using Cashback.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Cashback.Api.Extensions;

/// <summary>
/// Database startup extensions for the API host.
/// </summary>
public static class DatabaseExtensions
{
    /// <summary>
    /// Applies pending EF Core migrations when enabled by configuration.
    /// </summary>
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        if (!app.Configuration.GetValue("RUN_MIGRATIONS", false))
        {
            return;
        }

        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}
