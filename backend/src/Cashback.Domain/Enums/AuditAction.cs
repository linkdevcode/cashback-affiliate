namespace Cashback.Domain.Enums;

/// <summary>
/// Standardized audit log action identifiers.
/// </summary>
public enum AuditAction
{
    /// <summary>
    /// A new user account was created.
    /// </summary>
    UserCreated = 1,

    /// <summary>
    /// A user account was updated.
    /// </summary>
    UserUpdated = 2,

    /// <summary>
    /// A user account was disabled.
    /// </summary>
    UserDisabled = 3,

    /// <summary>
    /// An affiliate link was generated.
    /// </summary>
    LinkGenerated = 10,

    /// <summary>
    /// An order was created.
    /// </summary>
    OrderCreated = 20,

    /// <summary>
    /// An order was updated.
    /// </summary>
    OrderUpdated = 21,

    /// <summary>
    /// A withdrawal request was submitted.
    /// </summary>
    WithdrawalRequested = 30,

    /// <summary>
    /// A withdrawal request was approved.
    /// </summary>
    WithdrawalApproved = 31,

    /// <summary>
    /// A withdrawal request was rejected.
    /// </summary>
    WithdrawalRejected = 32,

    /// <summary>
    /// A withdrawal request was completed.
    /// </summary>
    WithdrawalCompleted = 33
}
