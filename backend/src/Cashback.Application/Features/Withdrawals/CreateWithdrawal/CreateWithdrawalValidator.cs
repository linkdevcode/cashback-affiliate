using Cashback.Application.Interfaces;
using FluentValidation;

namespace Cashback.Application.Features.Withdrawals.CreateWithdrawal;

/// <summary>
/// Validator for withdrawal request creation.
/// </summary>
public sealed class CreateWithdrawalValidator : AbstractValidator<CreateWithdrawalCommand>
{
    /// <summary>
    /// Initializes validation rules for withdrawal request creation.
    /// </summary>
    public CreateWithdrawalValidator(IWithdrawalSettings withdrawalSettings)
    {
        RuleFor(command => command.Amount)
            .GreaterThan(0)
            .WithMessage("Withdrawal amount must be greater than zero.")
            .GreaterThanOrEqualTo(withdrawalSettings.MinimumWithdrawalAmount)
            .WithMessage($"Minimum withdrawal amount is {withdrawalSettings.MinimumWithdrawalAmount:N0} VND.");

        RuleFor(command => command.BankName)
            .NotEmpty()
            .WithMessage("Bank name is required.")
            .MaximumLength(255);

        RuleFor(command => command.BankAccountNumber)
            .NotEmpty()
            .WithMessage("Bank account number is required.")
            .MaximumLength(100);

        RuleFor(command => command.BankAccountHolder)
            .NotEmpty()
            .WithMessage("Bank account holder name is required.")
            .MaximumLength(255);
    }
}
