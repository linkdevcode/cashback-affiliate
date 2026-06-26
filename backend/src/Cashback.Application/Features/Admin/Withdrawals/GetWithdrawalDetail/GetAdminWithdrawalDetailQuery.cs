using Cashback.Application.Features.Admin.Withdrawals.Common;
using MediatR;

namespace Cashback.Application.Features.Admin.Withdrawals.GetWithdrawalDetail;

/// <summary>
/// Query to retrieve detailed withdrawal information for admin management.
/// </summary>
public sealed record GetAdminWithdrawalDetailQuery(Guid WithdrawalId)
    : IRequest<AdminWithdrawalDetailDto>;
