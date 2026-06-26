using MediatR;

namespace Cashback.Application.Features.Admin.GetAdminDashboard;

/// <summary>
/// Query for retrieving admin dashboard statistics.
/// </summary>
public sealed record GetAdminDashboardQuery() : IRequest<GetAdminDashboardResponse>;
