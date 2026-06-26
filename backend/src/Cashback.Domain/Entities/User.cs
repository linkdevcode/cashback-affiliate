using Cashback.Domain.Common;
using Cashback.Domain.Enums;

namespace Cashback.Domain.Entities;

public class User : AuditableEntity
{
    public string Email { get; private set; } = null!;

    public string? PasswordHash { get; private set; }

    public string FullName { get; private set; } = null!;

    public string? AvatarUrl { get; private set; }

    public AuthProvider Provider { get; private set; }

    public string? ProviderUserId { get; private set; }

    public UserRole Role { get; private set; }

    public bool IsActive { get; private set; }

    public bool EmailVerified { get; private set; }

    public decimal AvailableBalance { get; private set; }

    public decimal PendingBalance { get; private set; }

    public decimal LifetimeCashback { get; private set; }

    public DateTime? LastLoginAt { get; private set; }

    public ICollection<AffiliateLink> AffiliateLinks { get; private set; } = [];

    public ICollection<Order> Orders { get; private set; } = [];

    public ICollection<CommissionTransaction> CommissionTransactions { get; private set; } = [];

    public ICollection<WithdrawRequest> WithdrawRequests { get; private set; } = [];

    public ICollection<Notification> Notifications { get; private set; } = [];
}
