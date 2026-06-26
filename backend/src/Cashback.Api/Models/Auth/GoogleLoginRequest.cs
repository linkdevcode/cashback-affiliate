namespace Cashback.Api.Models.Auth;

/// <summary>
/// Google login request body.
/// </summary>
public sealed class GoogleLoginRequest
{
    /// <summary>
    /// Google OAuth ID token from the client.
    /// </summary>
    public string IdToken { get; init; } = string.Empty;
}
