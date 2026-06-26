namespace Cashback.Application.Interfaces;

/// <summary>
/// Resolved user ownership for a webhook tracking parameter.
/// </summary>
public sealed class WebhookUserResolution
{
    /// <summary>
    /// User identifier resolved from the tracking parameter.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Affiliate link identifier when one matches the tracking parameter.
    /// </summary>
    public Guid? AffiliateLinkId { get; init; }

    /// <summary>
    /// Creates a webhook user resolution result.
    /// </summary>
    public WebhookUserResolution(Guid userId, Guid? affiliateLinkId)
    {
        UserId = userId;
        AffiliateLinkId = affiliateLinkId;
    }
}
