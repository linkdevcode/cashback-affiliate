using Cashback.Application.Features.Dashboard.Common;
using Cashback.Application.Interfaces;
using Cashback.Domain.Exceptions;
using MediatR;

namespace Cashback.Application.Features.Dashboard.GetDashboardSummary;

/// <summary>
/// Handles retrieval of dashboard summary for the authenticated user.
/// </summary>
public sealed class GetDashboardSummaryHandler
    : IRequestHandler<GetDashboardSummaryQuery, GetDashboardSummaryResponse>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDashboardService _dashboardService;

    /// <summary>
    /// Initializes a new instance of the get dashboard summary handler.
    /// </summary>
    public GetDashboardSummaryHandler(
        ICurrentUserService currentUserService,
        IDashboardService dashboardService)
    {
        _currentUserService = currentUserService;
        _dashboardService = dashboardService;
    }

    /// <inheritdoc/>
    public async Task<GetDashboardSummaryResponse> Handle(
        GetDashboardSummaryQuery request,
        CancellationToken cancellationToken)
    {
        var userId = GetAuthenticatedUserId();

        var summary = await _dashboardService.GetDashboardSummaryAsync(userId, cancellationToken);

        return DashboardMapper.ToResponse(summary);
    }

    /// <summary>
    /// Resolves the authenticated user identifier from the request context.
    /// </summary>
    private Guid GetAuthenticatedUserId()
    {
        if (!_currentUserService.IsAuthenticated || _currentUserService.UserId is null)
        {
            throw new UnauthorizedException("Authentication is required.");
        }

        return _currentUserService.UserId.Value;
    }
}
