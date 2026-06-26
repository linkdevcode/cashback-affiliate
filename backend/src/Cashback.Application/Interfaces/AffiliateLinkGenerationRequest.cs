namespace Cashback.Application.Interfaces;

/// <summary>
/// Input parameters for affiliate link generation.
/// </summary>
public sealed class AffiliateLinkGenerationRequest
{
    /// <summary>
    /// Original product URL to convert.
    /// </summary>
    public string OriginalUrl { get; init; } = null!;

    /// <summary>
    /// Primary tracking parameter, typically the user identifier.
    /// </summary>
    public string Sub1 { get; init; } = null!;

    /// <summary>
    /// Optional secondary tracking parameter.
    /// </summary>
    public string? Sub2 { get; init; }

    /// <summary>
    /// Optional tertiary tracking parameter.
    /// </summary>
    public string? Sub3 { get; init; }
}
