using Cashback.Application.Features.Withdrawals.Common;
using MediatR;

namespace Cashback.Application.Features.Withdrawals.ApproveWithdrawal;

/// <summary>
/// Command to approve a pending withdrawal request.
/// </summary>
public sealed record ApproveWithdrawalCommand(Guid WithdrawalId) : IRequest<WithdrawalActionResponse>;
