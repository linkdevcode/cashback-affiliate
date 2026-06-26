using MediatR;

namespace Cashback.Application.Features.Orders.GetOrderSummary;

/// <summary>
/// Query to retrieve aggregated order statistics for the authenticated user.
/// </summary>
public sealed record GetOrderSummaryQuery() : IRequest<GetOrderSummaryResponse>;
