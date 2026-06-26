using Cashback.Domain.Enums;

namespace Cashback.Application.Features.Admin.Orders.Common;

/// <summary>
/// User summary attached to admin order responses.
/// </summary>
public sealed class AdminOrderUserDto
{
    /// <summary>
    /// User identifier.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// User email address.
    /// </summary>
    public string Email { get; init; } = null!;

    /// <summary>
    /// User display name.
    /// </summary>
    public string FullName { get; init; } = null!;
}
