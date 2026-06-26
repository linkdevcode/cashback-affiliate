using Cashback.Application.Features.Admin.Users.Common;

namespace Cashback.Application.Features.Admin.Users.GetUserDetail;

/// <summary>
/// Detailed admin user response with profile and activity summaries.
/// </summary>
public sealed class GetUserDetailResponse
{
    /// <summary>
    /// User profile information.
    /// </summary>
    public AdminUserProfileDto Profile { get; init; } = null!;

    /// <summary>
    /// Aggregated order statistics for the user.
    /// </summary>
    public AdminUserOrderSummaryDto OrderSummary { get; init; } = null!;

    /// <summary>
    /// Aggregated withdrawal statistics for the user.
    /// </summary>
    public AdminUserWithdrawalSummaryDto WithdrawalSummary { get; init; } = null!;
}
