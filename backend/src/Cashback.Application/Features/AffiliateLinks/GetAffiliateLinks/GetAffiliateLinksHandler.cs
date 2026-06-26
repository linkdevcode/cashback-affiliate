using Cashback.Application.Features.AffiliateLinks.Common;
using Cashback.Application.Interfaces;
using Cashback.Domain.Entities;
using Cashback.Domain.Exceptions;
using MediatR;

namespace Cashback.Application.Features.AffiliateLinks.GetAffiliateLinks;

/// <summary>
/// Handles retrieval of paginated affiliate links for the authenticated user.
/// </summary>
public sealed class GetAffiliateLinksHandler
    : IRequestHandler<GetAffiliateLinksQuery, GetAffiliateLinksResponse>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IAffiliateLinkRepository _affiliateLinkRepository;

    /// <summary>
    /// Initializes a new instance of the get affiliate links handler.
    /// </summary>
    public GetAffiliateLinksHandler(
        ICurrentUserService currentUserService,
        IAffiliateLinkRepository affiliateLinkRepository)
    {
        _currentUserService = currentUserService;
        _affiliateLinkRepository = affiliateLinkRepository;
    }

    /// <inheritdoc/>
    public async Task<GetAffiliateLinksResponse> Handle(
        GetAffiliateLinksQuery request,
        CancellationToken cancellationToken)
    {
        var userId = GetAuthenticatedUserId();

        var (items, totalCount) = await _affiliateLinkRepository.GetPagedByUserIdAsync(
            userId,
            request.Page,
            request.PageSize,
            cancellationToken);

        return new GetAffiliateLinksResponse
        {
            Items = items.Select(MapToDto).ToList(),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }

    /// <summary>
    /// Maps an affiliate link entity to a list DTO.
    /// </summary>
    private static AffiliateLinkDto MapToDto(AffiliateLink link)
    {
        return new AffiliateLinkDto
        {
            Id = link.Id,
            OriginalUrl = link.OriginalUrl,
            AffiliateUrl = link.AffiliateUrl,
            ShortUrl = link.ShortUrl,
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
