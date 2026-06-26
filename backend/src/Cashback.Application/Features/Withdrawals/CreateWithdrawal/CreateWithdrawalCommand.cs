using MediatR;

namespace Cashback.Application.Features.Withdrawals.CreateWithdrawal;

/// <summary>
/// Command to create a new withdrawal request.
/// </summary>
public sealed record CreateWithdrawalCommand(
    decimal Amount,
    string BankName,
    string BankAccountNumber,
    string BankAccountHolder) : IRequest<CreateWithdrawalResponse>;
