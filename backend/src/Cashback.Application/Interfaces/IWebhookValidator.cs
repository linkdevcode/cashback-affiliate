namespace Cashback.Application.Interfaces;

/// <summary>
/// Validates webhook request authenticity for a specific provider.
/// </summary>
public interface IWebhookValidator
{
    /// <summary>
    /// Provider identifier handled by this validator.
    /// </summary>
    string Provider { get; }

    /// <summary>
    /// Validates webhook secret and signature for the given request context.
    /// </summary>
    WebhookValidationResult Validate(WebhookValidationContext context);
}
