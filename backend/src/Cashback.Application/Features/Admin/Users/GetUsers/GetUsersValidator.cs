using FluentValidation;

namespace Cashback.Application.Features.Admin.Users.GetUsers;

/// <summary>
/// Validator for admin user list queries.
/// </summary>
public sealed class GetUsersValidator : AbstractValidator<GetUsersQuery>
{
    /// <summary>
    /// Initializes validation rules for admin user list queries.
    /// </summary>
    public GetUsersValidator()
    {
        RuleFor(query => query.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be at least 1.");

        RuleFor(query => query.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100.");

        RuleFor(query => query.Email)
            .MaximumLength(255)
            .When(query => !string.IsNullOrWhiteSpace(query.Email))
            .WithMessage("Email search must not exceed 255 characters.");

        RuleFor(query => query.Name)
            .MaximumLength(255)
            .When(query => !string.IsNullOrWhiteSpace(query.Name))
            .WithMessage("Name search must not exceed 255 characters.");

        RuleFor(query => query.Status)
            .IsInEnum()
            .When(query => query.Status.HasValue)
            .WithMessage("Status must be a valid user status.");
    }
}
