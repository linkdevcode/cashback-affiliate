using FluentValidation;

namespace Cashback.Application.Features.Admin.Users.GetUserDetail;

/// <summary>
/// Validator for admin user detail queries.
/// </summary>
public sealed class GetUserDetailValidator : AbstractValidator<GetUserDetailQuery>
{
    /// <summary>
    /// Initializes validation rules for admin user detail queries.
    /// </summary>
    public GetUserDetailValidator()
    {
        RuleFor(query => query.UserId)
            .NotEmpty()
            .WithMessage("User identifier is required.");
    }
}
