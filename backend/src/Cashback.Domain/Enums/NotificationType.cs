namespace Cashback.Domain.Enums;

/// <summary>
/// In-app notification category.
/// </summary>
public enum NotificationType
{
    /// <summary>
    /// Notification for an approved order.
    /// </summary>
    OrderApproved = 1,

    /// <summary>
    /// Notification for a rejected order.
    /// </summary>
    OrderRejected = 2,

    /// <summary>
    /// Notification for a created withdrawal request.
    /// </summary>
    WithdrawalCreated = 3,

    /// <summary>
    /// Notification for an approved withdrawal.
    /// </summary>
    WithdrawalApproved = 4,

    /// <summary>
    /// Notification for a rejected withdrawal.
    /// </summary>
    WithdrawalRejected = 5,

    /// <summary>
    /// Notification for a completed withdrawal.
    /// </summary>
    WithdrawalCompleted = 6,

    /// <summary>
    /// System-wide announcement notification.
    /// </summary>
    SystemAnnouncement = 7
}
