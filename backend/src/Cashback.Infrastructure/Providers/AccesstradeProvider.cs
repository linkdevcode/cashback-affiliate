using System.Net.Http.Json;
using System.Text.Json;
using Cashback.Application.Interfaces;
using Cashback.Infrastructure.Providers.Accesstrade;
using Cashback.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cashback.Infrastructure.Providers;

/// <summary>
/// Accesstrade affiliate link generation provider.
/// </summary>
public sealed class AccesstradeProvider : IAffiliateProvider
{
    private const string CreateProductLinkPath = "v1/product_link/create";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;
    private readonly AccesstradeOptions _options;
    private readonly ILogger<AccesstradeProvider> _logger;

    /// <summary>
    /// Initializes a new instance of the Accesstrade provider.
    /// </summary>
    public AccesstradeProvider(
        HttpClient httpClient,
        IOptions<AccesstradeOptions> options,
        ILogger<AccesstradeProvider> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<AffiliateLinkResult?> GenerateAffiliateLinkAsync(
        AffiliateLinkGenerationRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_options.Token) || string.IsNullOrWhiteSpace(_options.CampaignId))
        {
            _logger.LogError("Accesstrade configuration is incomplete");
            return null;
        }

        var apiRequest = new AccesstradeCreateProductLinkRequest
        {
            CampaignId = _options.CampaignId,
            Urls = [request.OriginalUrl],
            Sub1 = request.Sub1,
            Sub2 = request.Sub2,
            Sub3 = request.Sub3
        };

        try
        {
            using var response = await _httpClient.PostAsJsonAsync(
                CreateProductLinkPath,
                apiRequest,
                JsonOptions,
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "Accesstrade API returned HTTP {StatusCode} for product link creation",
                    (int)response.StatusCode);
                return null;
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<AccesstradeCreateProductLinkResponse>(
                JsonOptions,
                cancellationToken);

            return MapResponse(apiResponse, request.OriginalUrl);
        }
        catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
        {
            _logger.LogWarning(ex, "Accesstrade API request timed out");
            return null;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Accesstrade API request failed");
            return null;
        }
        catch (JsonException ex)
        {
            _logger.LogWarning(ex, "Failed to deserialize Accesstrade API response");
            return null;
        }
    }

    /// <summary>
    /// Maps the Accesstrade API response to an application result.
    /// </summary>
    private AffiliateLinkResult? MapResponse(
        AccesstradeCreateProductLinkResponse? apiResponse,
        string fallbackOriginalUrl)
    {
        if (apiResponse is not { Success: true, Data: not null })
        {
            _logger.LogWarning("Accesstrade API returned an unsuccessful response");
            return null;
        }

        if (apiResponse.Data.ErrorLinks.Count > 0)
        {
            _logger.LogWarning(
                "Accesstrade API reported {ErrorCount} failed link(s)",
                apiResponse.Data.ErrorLinks.Count);
            return null;
        }

        var successLink = apiResponse.Data.SuccessLinks.FirstOrDefault();
        if (successLink is null
            || string.IsNullOrWhiteSpace(successLink.AffiliateLink)
            || string.IsNullOrWhiteSpace(successLink.ShortLink))
        {
            _logger.LogWarning("Accesstrade API did not return a valid generated link");
            return null;
        }

        return new AffiliateLinkResult
        {
            OriginalUrl = successLink.UrlOrigin ?? fallbackOriginalUrl,
            AffiliateUrl = successLink.AffiliateLink,
            ShortUrl = successLink.ShortLink,
            CampaignId = _options.CampaignId
        };
    }
}
