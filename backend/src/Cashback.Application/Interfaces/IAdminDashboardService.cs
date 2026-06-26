using Cashback.Application.Features.Admin.Common;

namespace Cashback.Application.Interfaces;

/// <summary>
/// Aggregates platform-wide dashboard metrics for administrators.
/// </summary>
public interface IAdminDashboardService
{
    /// <summary>
    /// Gets aggregated admin dashboard statistics.
    /// </summary>
    Task<AdminDashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken);
}
