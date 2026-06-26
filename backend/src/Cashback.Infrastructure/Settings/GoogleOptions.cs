namespace Cashback.Infrastructure.Settings;

/// <summary>
/// Google OAuth configuration options.
/// </summary>
public sealed class GoogleOptions
{
    /// <summary>
    /// Configuration section name.
    /// </summary>
    public const string SectionName = "Google";

    /// <summary>
    /// Google OAuth client identifier.
    /// </summary>
    public string ClientId { get; init; } = string.Empty;
}
