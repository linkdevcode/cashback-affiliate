using MediatR;

namespace Cashback.Application.Features.Dashboard.GetDashboardSummary;

/// <summary>
/// Query to retrieve dashboard summary for the authenticated user.
/// </summary>
public sealed record GetDashboardSummaryQuery() : IRequest<GetDashboardSummaryResponse>;
