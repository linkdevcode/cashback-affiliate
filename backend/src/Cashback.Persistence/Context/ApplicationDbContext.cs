using Cashback.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cashback.Persistence.Context;

/// <summary>
/// Entity Framework database context for the application.
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the database context.
    /// </summary>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Platform users.
    /// </summary>
    public DbSet<User> Users => Set<User>();

    /// <summary>
    /// User refresh tokens.
    /// </summary>
    public DbSet<UserRefreshToken> UserRefreshTokens => Set<UserRefreshToken>();

    /// <summary>
    /// Generated affiliate links.
    /// </summary>
    public DbSet<AffiliateLink> AffiliateLinks => Set<AffiliateLink>();

    /// <summary>
    /// Affiliate conversion orders.
    /// </summary>
    public DbSet<Order> Orders => Set<Order>();

    /// <summary>
    /// Commission balance transactions.
    /// </summary>
    public DbSet<CommissionTransaction> CommissionTransactions => Set<CommissionTransaction>();

    /// <summary>
    /// Withdrawal requests.
    /// </summary>
    public DbSet<WithdrawRequest> WithdrawRequests => Set<WithdrawRequest>();

    /// <summary>
    /// User notifications.
    /// </summary>
    public DbSet<Notification> Notifications => Set<Notification>();

    /// <summary>
    /// Stored webhook events.
    /// </summary>
    public DbSet<WebhookEvent> WebhookEvents => Set<WebhookEvent>();

    /// <summary>
    /// System configuration settings.
    /// </summary>
    public DbSet<SystemSetting> SystemSettings => Set<SystemSetting>();

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
