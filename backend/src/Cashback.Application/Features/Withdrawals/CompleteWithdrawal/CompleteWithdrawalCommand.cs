using Cashback.Application.Features.Withdrawals.Common;
using MediatR;

namespace Cashback.Application.Features.Withdrawals.CompleteWithdrawal;

/// <summary>
/// Command to mark an approved withdrawal as completed.
/// </summary>
public sealed record CompleteWithdrawalCommand(Guid WithdrawalId) : IRequest<WithdrawalActionResponse>;
