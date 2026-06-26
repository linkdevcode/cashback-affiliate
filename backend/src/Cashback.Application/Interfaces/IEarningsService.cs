using Cashback.Application.Features.Earnings.Common;

namespace Cashback.Application.Interfaces;

/// <summary>
/// Calculates user earnings summaries from order data.
/// </summary>
public interface IEarningsService
{
    /// <summary>
    /// Gets cashback totals grouped by order status for a user.
    /// </summary>
    Task<EarningsSummaryDto> GetUserEarningsAsync(
        Guid userId,
        CancellationToken cancellationToken);
}
