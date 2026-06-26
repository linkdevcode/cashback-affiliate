using Cashback.Application.Interfaces;
using Cashback.Domain.Entities;
using Cashback.Domain.Enums;
using Cashback.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Cashback.Application.Features.AffiliateLinks.GenerateAffiliateLink;

/// <summary>
/// Handles affiliate link generation via the configured provider.
/// </summary>
public sealed class GenerateAffiliateLinkHandler
    : IRequestHandler<GenerateAffiliateLinkCommand, GenerateAffiliateLinkResponse>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly IAffiliateProvider _affiliateProvider;
    private readonly IAffiliateLinkRepository _affiliateLinkRepository;
    private readonly ILogger<GenerateAffiliateLinkHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the generate affiliate link handler.
    /// </summary>
    public GenerateAffiliateLinkHandler(
        ICurrentUserService currentUserService,
        IUserRepository userRepository,
        IAffiliateProvider affiliateProvider,
        IAffiliateLinkRepository affiliateLinkRepository,
        ILogger<GenerateAffiliateLinkHandler> logger)
    {
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _affiliateProvider = affiliateProvider;
        _affiliateLinkRepository = affiliateLinkRepository;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<GenerateAffiliateLinkResponse> Handle(
        GenerateAffiliateLinkCommand request,
        CancellationToken cancellationToken)
    {
        var userId = GetAuthenticatedUserId();
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            throw new NotFoundException("User not found.");
        }

        if (user.Status == UserStatus.Suspended)
        {
            throw new ForbiddenException("Your account has been suspended.");
        }

        var originalUrl = request.Url.Trim();
        var sub1 = $"USER_{userId}";

        var providerResult = await _affiliateProvider.GenerateAffiliateLinkAsync(
            new AffiliateLinkGenerationRequest
            {
                OriginalUrl = originalUrl,
                Sub1 = sub1
            },
            cancellationToken);

        if (providerResult is null)
        {
            throw new BusinessRuleException("Unable to generate affiliate link. Please try again later.");
        }

        var affiliateLink = AffiliateLink.Create(
            userId,
            providerResult.OriginalUrl,
            providerResult.AffiliateUrl,
            providerResult.ShortUrl,
            sub1,
            providerResult.CampaignId);

        await _affiliateLinkRepository.AddAsync(affiliateLink, cancellationToken);

        _logger.LogInformation(
            "Affiliate link {AffiliateLinkId} generated for user {UserId}",
            affiliateLink.Id,
            userId);

        return new GenerateAffiliateLinkResponse
        {
            Id = affiliateLink.Id,
            OriginalUrl = providerResult.OriginalUrl,
            AffiliateUrl = providerResult.AffiliateUrl,
            ShortUrl = providerResult.ShortUrl
        };
    }

    /// <summary>
    /// Resolves the authenticated user identifier from the request context.
    /// </summary>
    private Guid GetAuthenticatedUserId()
    {
        if (!_currentUserService.IsAuthenticated || _currentUserService.UserId is null)
        {
            throw new UnauthorizedException("Authentication is required.");
        }

        return _currentUserService.UserId.Value;
    }
}
