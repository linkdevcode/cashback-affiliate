namespace Cashback.Application.Interfaces;

/// <summary>
/// Result of webhook security validation.
/// </summary>
public sealed class WebhookValidationResult
{
    /// <summary>
    /// Indicates whether the webhook request passed security validation.
    /// </summary>
    public bool IsValid { get; init; }

    /// <summary>
    /// Reason the webhook request was rejected.
    /// </summary>
    public string? FailureReason { get; init; }

    /// <summary>
    /// Creates a successful validation result.
    /// </summary>
    public static WebhookValidationResult Success()
    {
        return new WebhookValidationResult { IsValid = true };
    }

    /// <summary>
    /// Creates a failed validation result.
    /// </summary>
    public static WebhookValidationResult Failure(string reason)
    {
        return new WebhookValidationResult
        {
            IsValid = false,
            FailureReason = reason
        };
    }
}
