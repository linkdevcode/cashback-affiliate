using FluentValidation;

namespace Cashback.Application.Features.Orders.GetOrders;

/// <summary>
/// Validator for paginated order list queries.
/// </summary>
public sealed class GetOrdersValidator : AbstractValidator<GetOrdersQuery>
{
    private static readonly string[] AllowedSortFields =
    [
        "createdat",
        "commissionamount",
        "cashbackamount",
        "status"
    ];

    /// <summary>
    /// Initializes validation rules for order list queries.
    /// </summary>
    public GetOrdersValidator()
    {
        RuleFor(query => query.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be at least 1.");

        RuleFor(query => query.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100.");

        RuleFor(query => query.Status)
            .IsInEnum()
            .When(query => query.Status.HasValue)
            .WithMessage("Status must be a valid order status.");

        RuleFor(query => query.SortBy)
            .Must(sortBy => AllowedSortFields.Contains(sortBy.ToLowerInvariant()))
            .WithMessage("Sort by must be one of: createdAt, commissionAmount, cashbackAmount, status.");

        RuleFor(query => query.SortDirection)
            .Must(direction =>
                direction.Equals("asc", StringComparison.OrdinalIgnoreCase) ||
                direction.Equals("desc", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Sort direction must be asc or desc.");
    }
}
