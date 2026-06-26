using Cashback.Application.Features.Admin.Withdrawals.Common;
using Cashback.Domain.Enums;
using MediatR;

namespace Cashback.Application.Features.Admin.Withdrawals.GetWithdrawals;

/// <summary>
/// Query to retrieve paginated withdrawals for admin management.
/// </summary>
public sealed record GetAdminWithdrawalsQuery(
    int Page = 1,
    int PageSize = 20,
    string? User = null,
    WithdrawalStatus? Status = null)
    : IRequest<GetAdminWithdrawalsResponse>;
