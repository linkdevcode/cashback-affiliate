using FluentValidation;

namespace Cashback.Application.Features.Withdrawals.RejectWithdrawal;

/// <summary>
/// Validator for withdrawal rejection requests.
/// </summary>
public sealed class RejectWithdrawalValidator : AbstractValidator<RejectWithdrawalCommand>
{
    /// <summary>
    /// Initializes validation rules for withdrawal rejection.
    /// </summary>
    public RejectWithdrawalValidator()
    {
        RuleFor(command => command.Reason)
            .MaximumLength(500)
            .When(command => !string.IsNullOrWhiteSpace(command.Reason))
            .WithMessage("Rejection reason cannot exceed 500 characters.");
    }
}
