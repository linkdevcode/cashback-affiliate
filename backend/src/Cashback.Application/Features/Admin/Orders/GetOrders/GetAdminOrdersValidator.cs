using FluentValidation;

namespace Cashback.Application.Features.Admin.Orders.GetOrders;

/// <summary>
/// Validator for admin order list queries.
/// </summary>
public sealed class GetAdminOrdersValidator : AbstractValidator<GetAdminOrdersQuery>
{
    /// <summary>
    /// Initializes validation rules for admin order list queries.
    /// </summary>
    public GetAdminOrdersValidator()
    {
        RuleFor(query => query.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be at least 1.");

        RuleFor(query => query.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100.");

        RuleFor(query => query.OrderId)
            .MaximumLength(255)
            .When(query => !string.IsNullOrWhiteSpace(query.OrderId))
            .WithMessage("Order ID search must not exceed 255 characters.");

        RuleFor(query => query.User)
            .MaximumLength(255)
            .When(query => !string.IsNullOrWhiteSpace(query.User))
            .WithMessage("User search must not exceed 255 characters.");

        RuleFor(query => query.Status)
            .IsInEnum()
            .When(query => query.Status.HasValue)
            .WithMessage("Status must be a valid order status.");

        RuleFor(query => query)
            .Must(query => !query.FromDate.HasValue || !query.ToDate.HasValue || query.FromDate <= query.ToDate)
            .WithMessage("From date must be earlier than or equal to to date.");
    }
}
