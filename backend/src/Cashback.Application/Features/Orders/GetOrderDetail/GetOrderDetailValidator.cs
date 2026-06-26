using FluentValidation;

namespace Cashback.Application.Features.Orders.GetOrderDetail;

/// <summary>
/// Validator for order detail queries.
/// </summary>
public sealed class GetOrderDetailValidator : AbstractValidator<GetOrderDetailQuery>
{
    /// <summary>
    /// Initializes validation rules for order detail queries.
    /// </summary>
    public GetOrderDetailValidator()
    {
        RuleFor(query => query.Id)
            .NotEmpty()
            .WithMessage("Order identifier is required.");
    }
}
