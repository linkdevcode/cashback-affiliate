using Cashback.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Cashback.Application.Services;

/// <summary>
/// Resolves webhook Sub1 tracking values to platform users.
/// </summary>
public sealed class WebhookSub1UserResolver : IWebhookUserResolver
{
    private const string UserSubPrefix = "USER_";

    private readonly IUserRepository _userRepository;
    private readonly IAffiliateLinkRepository _affiliateLinkRepository;
    private readonly ILogger<WebhookSub1UserResolver> _logger;

    /// <summary>
    /// Initializes a new instance of the webhook Sub1 user resolver.
    /// </summary>
    public WebhookSub1UserResolver(
        IUserRepository userRepository,
        IAffiliateLinkRepository affiliateLinkRepository,
        ILogger<WebhookSub1UserResolver> logger)
    {
        _userRepository = userRepository;
        _affiliateLinkRepository = affiliateLinkRepository;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<WebhookUserResolution?> ResolveBySub1Async(
        string sub1,
        CancellationToken cancellationToken)
    {
        var userId = TryParseUserId(sub1);
        if (userId is null)
        {
            _logger.LogWarning("Unable to parse user identifier from Sub1 value {Sub1}", sub1);
            return null;
        }

        var user = await _userRepository.GetByIdAsync(userId.Value, cancellationToken);
        if (user is null)
        {
            _logger.LogWarning("No user found for Sub1 value {Sub1}", sub1);
            return null;
        }

        var affiliateLink = await _affiliateLinkRepository.GetLatestBySub1Async(sub1, cancellationToken);

        return new WebhookUserResolution(user.Id, affiliateLink?.Id);
    }

    /// <summary>
    /// Parses a user identifier from a Sub1 tracking value.
    /// </summary>
    private static Guid? TryParseUserId(string sub1)
    {
        if (!sub1.StartsWith(UserSubPrefix, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        var userIdValue = sub1[UserSubPrefix.Length..];

        return Guid.TryParse(userIdValue, out var userId) ? userId : null;
    }
}
