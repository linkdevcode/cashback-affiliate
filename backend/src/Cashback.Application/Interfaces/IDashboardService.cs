using Cashback.Application.Features.Dashboard.Common;

namespace Cashback.Application.Interfaces;

/// <summary>
/// Aggregates dashboard metrics for a user.
/// </summary>
public interface IDashboardService
{
    /// <summary>
    /// Gets the dashboard summary for a user.
    /// </summary>
    Task<DashboardSummaryDto> GetDashboardSummaryAsync(
        Guid userId,
        CancellationToken cancellationToken);
}
