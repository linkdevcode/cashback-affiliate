namespace Cashback.Application.Features.Admin.Withdrawals.Common;

/// <summary>
/// User summary attached to admin withdrawal responses.
/// </summary>
public sealed class AdminWithdrawalUserDto
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
