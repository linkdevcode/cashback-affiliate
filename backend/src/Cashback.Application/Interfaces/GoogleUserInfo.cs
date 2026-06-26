namespace Cashback.Application.Interfaces;

/// <summary>
/// Verified Google account profile from an ID token.
/// </summary>
public sealed class GoogleUserInfo
{
    /// <summary>
    /// Google subject identifier.
    /// </summary>
    public string ProviderUserId { get; init; } = null!;

    /// <summary>
    /// User email address from Google.
    /// </summary>
    public string Email { get; init; } = null!;

    /// <summary>
    /// User display name from Google.
    /// </summary>
    public string FullName { get; init; } = null!;

    /// <summary>
    /// Avatar URL from Google.
    /// </summary>
    public string? AvatarUrl { get; init; }
}
