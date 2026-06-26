using FluentValidation;

namespace Cashback.Application.Features.Auth.RefreshToken;

/// <summary>
/// Validator for refresh token requests.
/// </summary>
public sealed class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
{
    /// <summary>
    /// Initializes validation rules for token refresh.
    /// </summary>
    public RefreshTokenValidator()
    {
        RuleFor(command => command.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required.");
    }
}
