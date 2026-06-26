using FluentValidation;

namespace Cashback.Application.Features.Admin.Withdrawals.GetWithdrawals;

/// <summary>
/// Validator for admin withdrawal list queries.
/// </summary>
public sealed class GetAdminWithdrawalsValidator : AbstractValidator<GetAdminWithdrawalsQuery>
{
    /// <summary>
    /// Initializes validation rules for admin withdrawal list queries.
    /// </summary>
    public GetAdminWithdrawalsValidator()
    {
        RuleFor(query => query.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be at least 1.");

        RuleFor(query => query.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100.");

        RuleFor(query => query.User)
            .MaximumLength(255)
            .When(query => !string.IsNullOrWhiteSpace(query.User))
            .WithMessage("User search must not exceed 255 characters.");

        RuleFor(query => query.Status)
            .IsInEnum()
            .When(query => query.Status.HasValue)
            .WithMessage("Status must be a valid withdrawal status.");
    }
}
