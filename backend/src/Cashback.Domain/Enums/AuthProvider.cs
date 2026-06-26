namespace Cashback.Domain.Enums;

/// <summary>
/// Authentication provider source.
/// </summary>
public enum AuthProvider
{
    /// <summary>
    /// Email and password authentication.
    /// </summary>
    Local = 1,

    /// <summary>
    /// Google OAuth authentication.
    /// </summary>
    Google = 2
}
