using Cashback.Application.Features.Withdrawals.Common;
using MediatR;

namespace Cashback.Application.Features.Withdrawals.RejectWithdrawal;

/// <summary>
/// Command to reject a pending withdrawal request.
/// </summary>
public sealed record RejectWithdrawalCommand(
    Guid WithdrawalId,
    string? Reason) : IRequest<WithdrawalActionResponse>;
