namespace Cashback.Application.Features.Withdrawals.Common;

/// <summary>
/// Response returned after an admin withdrawal action.
/// </summary>
public sealed class WithdrawalActionResponse
{
    /// <summary>
    /// Identifier of the processed withdrawal.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Current withdrawal status value.
    /// </summary>
    public int Status { get; init; }

    /// <summary>
    /// Human-readable withdrawal status name.
    /// </summary>
    public string StatusName { get; init; } = null!;
}
