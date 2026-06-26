namespace Cashback.Domain.Enums;

/// <summary>
/// Supported affiliate merchant platform.
/// </summary>
public enum MerchantType
{
    /// <summary>
    /// Shopee marketplace.
    /// </summary>
    Shopee = 1,

    /// <summary>
    /// Lazada marketplace.
    /// </summary>
    Lazada = 2,

    /// <summary>
    /// TikTok Shop marketplace.
    /// </summary>
    TikTokShop = 3,

    /// <summary>
    /// Other or unsupported merchant.
    /// </summary>
    Other = 99
}
