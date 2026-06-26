using Cashback.Application.Features.AffiliateLinks.Common;
using Cashback.Application.Interfaces;
using Cashback.Domain.Exceptions;
using MediatR;

namespace Cashback.Application.Features.AffiliateLinks.GetAffiliateLinkDetail;

/// <summary>
/// Handles retrieval of a single affiliate link with ownership validation.
/// </summary>
public sealed class GetAffiliateLinkDetailHandler
    : IRequestHandler<GetAffiliateLinkDetailQuery, AffiliateLinkDetailDto>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IAffiliateLinkRepository _affiliateLinkRepository;

    /// <summary>
    /// Initializes a new instance of the get affiliate link detail handler.
    /// </summary>
    public GetAffiliateLinkDetailHandler(
        ICurrentUserService currentUserService,
        IAffiliateLinkRepository affiliateLinkRepository)
    {
        _currentUserService = currentUserService;
        _affiliateLinkRepository = affiliateLinkRepository;
    }

    /// <inheritdoc/>
    public async Task<AffiliateLinkDetailDto> Handle(
        GetAffiliateLinkDetailQuery request,
        CancellationToken cancellationToken)
    {
        var userId = GetAuthenticatedUserId();

        var link = await _affiliateLinkRepository.GetByIdForUserAsync(
            request.Id,
            userId,
            cancellationToken);

        if (link is null)
        {
            throw new NotFoundException("Affiliate link not found.");
        }

        return new AffiliateLinkDetailDto
        {
            Id = link.Id,
            OriginalUrl = link.OriginalUrl,
            AffiliateUrl = link.AffiliateUrl,
            ShortUrl = link.ShortUrl,
            Sub1 = link.Sub1,
            CampaignId = link.CampaignId,
            CreatedAt = link.CreatedAt
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
