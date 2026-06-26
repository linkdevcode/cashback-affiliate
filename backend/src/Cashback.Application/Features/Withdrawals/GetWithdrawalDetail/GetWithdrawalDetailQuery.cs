using Cashback.Application.Features.Withdrawals.Common;
using MediatR;

namespace Cashback.Application.Features.Withdrawals.GetWithdrawalDetail;

/// <summary>
/// Query to retrieve a single withdrawal owned by the authenticated user.
/// </summary>
public sealed record GetWithdrawalDetailQuery(Guid Id) : IRequest<WithdrawalDetailDto>;
