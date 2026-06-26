using Cashback.Application.Features.Admin.Common;
using Cashback.Application.Interfaces;
using MediatR;

namespace Cashback.Application.Features.Admin.GetAdminDashboard;

/// <summary>
/// Handles retrieval of admin dashboard statistics.
/// </summary>
public sealed class GetAdminDashboardHandler
    : IRequestHandler<GetAdminDashboardQuery, GetAdminDashboardResponse>
{
    private readonly IAdminDashboardService _adminDashboardService;

    /// <summary>
    /// Initializes a new instance of the get admin dashboard handler.
    /// </summary>
    public GetAdminDashboardHandler(IAdminDashboardService adminDashboardService)
    {
        _adminDashboardService = adminDashboardService;
    }

    /// <inheritdoc/>
    public async Task<GetAdminDashboardResponse> Handle(
        GetAdminDashboardQuery request,
        CancellationToken cancellationToken)
    {
        var summary = await _adminDashboardService.GetDashboardSummaryAsync(cancellationToken);

        return AdminDashboardMapper.ToResponse(summary);
    }
}
