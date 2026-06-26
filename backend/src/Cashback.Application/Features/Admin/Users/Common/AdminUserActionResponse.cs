namespace Cashback.Application.Features.Admin.Users.Common;

/// <summary>
/// Response returned after an admin user status action.
/// </summary>
public sealed class AdminUserActionResponse
{
    /// <summary>
    /// User identifier.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Updated user status value.
    /// </summary>
    public int Status { get; init; }

    /// <summary>
    /// Updated user status name.
    /// </summary>
    public string StatusName { get; init; } = null!;
}
