namespace Cashback.Application.Interfaces;

/// <summary>
/// Resolves webhook tracking parameters to platform users.
/// </summary>
public interface IWebhookUserResolver
{
    /// <summary>
    /// Resolves a webhook Sub1 tracking value to a user.
    /// </summary>
    Task<WebhookUserResolution?> ResolveBySub1Async(
        string sub1,
        CancellationToken cancellationToken);
}
