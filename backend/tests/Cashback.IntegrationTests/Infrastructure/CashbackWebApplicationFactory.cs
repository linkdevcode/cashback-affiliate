using Cashback.Persistence.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cashback.IntegrationTests.Infrastructure;

/// <summary>
/// Test host factory for API integration tests.
/// </summary>
public sealed class CashbackWebApplicationFactory : WebApplicationFactory<Program>, IDisposable
{
    private readonly SqliteConnection _connection;

    /// <summary>
    /// Initializes a new instance of the API test host factory.
    /// </summary>
    public CashbackWebApplicationFactory()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
    }

    /// <summary>
    /// Ensures the in-memory database schema is created.
    /// </summary>
    public async Task InitializeDatabaseAsync()
    {
        await using var scope = Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
    }

    /// <inheritdoc/>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((_, configurationBuilder) =>
        {
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["UseSqliteForTests"] = "true",
                ["ConnectionStrings:DefaultConnection"] = _connection.ConnectionString,
                ["Jwt:Secret"] = "integration-test-secret-key-minimum-32-characters",
                ["Jwt:Issuer"] = "cashback-test",
                ["Jwt:Audience"] = "cashback-test-client",
                ["Jwt:AccessTokenExpirationMinutes"] = "15",
                ["Jwt:RefreshTokenExpirationDays"] = "7",
                ["RUN_MIGRATIONS"] = "false",
                ["Google:ClientId"] = "integration-test-google-client-id",
                ["Accesstrade:BaseUrl"] = "https://api.accesstrade.vn",
                ["Accesstrade:Token"] = "test-token",
                ["Accesstrade:CampaignId"] = "test-campaign",
                ["Accesstrade:WebhookSecret"] = "test-webhook-secret",
                ["Cashback:CashbackPercentage"] = "80",
                ["Cashback:PlatformCommissionPercentage"] = "20",
                ["Withdrawal:MinimumWithdrawalAmount"] = "50000"
            });
        });
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _connection.Dispose();
        }

        base.Dispose(disposing);
    }

    void IDisposable.Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
