namespace Cashback.Application.Interfaces;

/// <summary>
/// Google OAuth identity verification service.
/// </summary>
public interface IGoogleAuthService
{
    /// <summary>
    /// Validates a Google ID token and returns the verified user profile.
    /// </summary>
    Task<GoogleUserInfo?> ValidateIdTokenAsync(string idToken, CancellationToken cancellationToken);
}
