using FluentValidation;

namespace Cashback.Application.Features.AffiliateLinks.GenerateAffiliateLink;

/// <summary>
/// Validator for affiliate link generation requests.
/// </summary>
public sealed class GenerateAffiliateLinkValidator : AbstractValidator<GenerateAffiliateLinkCommand>
{
    /// <summary>
    /// Initializes validation rules for affiliate link generation.
    /// </summary>
    public GenerateAffiliateLinkValidator()
    {
        RuleFor(command => command.Url)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("URL is required.")
            .Must(BeAValidUrl)
            .WithMessage("URL must be a valid URL format.")
            .Must(BeShopeeUrl)
            .WithMessage("Only Shopee URLs are supported.");
    }

    /// <summary>
    /// Validates that the value is a well-formed HTTP or HTTPS URL.
    /// </summary>
    private static bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url.Trim(), UriKind.Absolute, out var uri)
            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }

    /// <summary>
    /// Validates that the URL belongs to a supported Shopee domain.
    /// </summary>
    private static bool BeShopeeUrl(string url)
    {
        if (!Uri.TryCreate(url.Trim(), UriKind.Absolute, out var uri))
        {
            return false;
        }

        var host = uri.Host.ToLowerInvariant();

        return host is "shopee.vn" or "www.shopee.vn"
            || host.EndsWith(".shopee.vn", StringComparison.Ordinal)
            || host is "shopee.com" or "www.shopee.com"
            || host.EndsWith(".shopee.com", StringComparison.Ordinal);
    }
}
