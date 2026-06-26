namespace Cashback.Api.Models.Admin;

/// <summary>
/// Request body for rejecting a withdrawal request.
/// </summary>
public sealed class RejectWithdrawalRequest
{
    /// <summary>
    /// Optional reason for rejecting the withdrawal.
    /// </summary>
    public string? Reason { get; init; }
}
