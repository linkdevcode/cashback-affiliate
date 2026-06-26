namespace Cashback.Domain.Enums;

/// <summary>
/// Affiliate order lifecycle status.
/// </summary>
public enum OrderStatus
{
    /// <summary>
    /// Order is awaiting merchant confirmation.
    /// </summary>
    Pending = 1,

    /// <summary>
    /// Order commission has been approved.
    /// </summary>
    Approved = 2,

    /// <summary>
    /// Order was rejected or cancelled.
    /// </summary>
    Rejected = 3
}
