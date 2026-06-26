using Cashback.Domain.Common;
using Cashback.Domain.Enums;

namespace Cashback.Domain.Entities;

/// <summary>
/// In-app notification delivered to a user.
/// </summary>
public class Notification : BaseEntity
{
    /// <summary>
    /// Recipient of the notification.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Category of the notification.
    /// </summary>
    public NotificationType Type { get; private set; }

    /// <summary>
    /// Notification title shown to the user.
    /// </summary>
    public string Title { get; private set; } = null!;

    /// <summary>
    /// Notification message body.
    /// </summary>
    public string Message { get; private set; } = null!;

    /// <summary>
    /// Indicates whether the notification has been read.
    /// </summary>
    public bool IsRead { get; private set; }

    /// <summary>
    /// UTC timestamp when the notification was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// UTC timestamp when the notification was marked as read.
    /// </summary>
    public DateTime? ReadAt { get; private set; }

    /// <summary>
    /// User associated with the notification.
    /// </summary>
    public User User { get; private set; } = null!;
}
