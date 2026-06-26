namespace Cashback.Api.Constants;

/// <summary>
/// HttpContext item keys used by webhook middleware and controllers.
/// </summary>
public static class WebhookHttpContextKeys
{
    /// <summary>
    /// Raw webhook request body captured before model binding.
    /// </summary>
    public const string RawBody = "WebhookRawBody";
}
