using Cashback.Domain.Common;
using Cashback.Domain.Enums;

namespace Cashback.Domain.Entities;

public class WithdrawRequest : BaseEntity
{
    public Guid UserId { get; private set; }

    public decimal Amount { get; private set; }

    public string BankName { get; private set; } = null!;

    public string BankAccountNumber { get; private set; } = null!;

    public string BankAccountName { get; private set; } = null!;

    public WithdrawalStatus Status { get; private set; }

    public string? Note { get; private set; }

    public DateTime RequestedAt { get; private set; }

    public DateTime? ProcessedAt { get; private set; }

    public User User { get; private set; } = null!;
}
