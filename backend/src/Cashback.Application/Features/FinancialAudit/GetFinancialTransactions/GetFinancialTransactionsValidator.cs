using Cashback.Domain.Enums;
using FluentValidation;

namespace Cashback.Application.Features.FinancialAudit.GetFinancialTransactions;

/// <summary>
/// Validator for financial transaction audit queries.
/// </summary>
public sealed class GetFinancialTransactionsValidator
    : AbstractValidator<GetFinancialTransactionsQuery>
{
    /// <summary>
    /// Initializes validation rules for financial transaction audit queries.
    /// </summary>
    public GetFinancialTransactionsValidator()
    {
        RuleFor(query => query.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be at least 1.");

        RuleFor(query => query.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100.");

        RuleFor(query => query.Type)
            .IsInEnum()
            .When(query => query.Type.HasValue)
            .WithMessage("Type must be a valid transaction type.");
    }
}
