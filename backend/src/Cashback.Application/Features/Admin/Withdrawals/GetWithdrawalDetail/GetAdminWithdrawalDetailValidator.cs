using FluentValidation;

namespace Cashback.Application.Features.Admin.Withdrawals.GetWithdrawalDetail;

/// <summary>
/// Validator for admin withdrawal detail queries.
/// </summary>
public sealed class GetAdminWithdrawalDetailValidator : AbstractValidator<GetAdminWithdrawalDetailQuery>
{
    /// <summary>
    /// Initializes validation rules for admin withdrawal detail queries.
    /// </summary>
    public GetAdminWithdrawalDetailValidator()
    {
        RuleFor(query => query.WithdrawalId)
            .NotEmpty()
            .WithMessage("Withdrawal identifier is required.");
    }
}
