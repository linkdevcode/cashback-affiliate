namespace Cashback.Domain.Enums;

public enum TransactionType
{
    CashbackPending = 1,
    CashbackApproved = 2,
    CashbackReversed = 3,
    WithdrawalRequested = 4,
    WithdrawalCompleted = 5,
    Adjustment = 6
}
