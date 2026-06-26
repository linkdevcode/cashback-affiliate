using Cashback.Domain.Enums;
using MediatR;

namespace Cashback.Application.Features.Withdrawals.GetWithdrawals;

/// <summary>
/// Query to retrieve paginated withdrawals for the authenticated user.
/// </summary>
public sealed record GetWithdrawalsQuery(
    int Page = 1,
    int PageSize = 20,
    WithdrawalStatus? Status = null,
    string SortBy = "requestedAt",
    string SortDirection = "desc")
    : IRequest<GetWithdrawalsResponse>;
