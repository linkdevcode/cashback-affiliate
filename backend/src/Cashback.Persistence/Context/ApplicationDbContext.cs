using Cashback.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cashback.Persistence.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<AffiliateLink> AffiliateLinks => Set<AffiliateLink>();

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<CommissionTransaction> CommissionTransactions => Set<CommissionTransaction>();

    public DbSet<WithdrawRequest> WithdrawRequests => Set<WithdrawRequest>();

    public DbSet<Notification> Notifications => Set<Notification>();

    public DbSet<WebhookEvent> WebhookEvents => Set<WebhookEvent>();

    public DbSet<SystemSetting> SystemSettings => Set<SystemSetting>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
