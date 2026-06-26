using FluentValidation;

namespace Cashback.Application.Features.Admin.Orders.GetOrderDetail;

/// <summary>
/// Validator for admin order detail queries.
/// </summary>
public sealed class GetAdminOrderDetailValidator : AbstractValidator<GetAdminOrderDetailQuery>
{
    /// <summary>
    /// Initializes validation rules for admin order detail queries.
    /// </summary>
    public GetAdminOrderDetailValidator()
    {
        RuleFor(query => query.OrderId)
            .NotEmpty()
            .WithMessage("Order identifier is required.");
    }
}
