using FluentValidation;

namespace Cashback.Application.Features.Auth.LoginWithGoogle;

/// <summary>
/// Validator for Google login requests.
/// </summary>
public sealed class LoginWithGoogleValidator : AbstractValidator<LoginWithGoogleCommand>
{
    /// <summary>
    /// Initializes validation rules for Google login.
    /// </summary>
    public LoginWithGoogleValidator()
    {
        RuleFor(command => command.IdToken)
            .NotEmpty()
            .WithMessage("Google ID token is required.");
    }
}
