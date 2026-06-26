using Cashback.Application.Features.AffiliateLinks.Common;
using MediatR;

namespace Cashback.Application.Features.AffiliateLinks.GetAffiliateLinks;

/// <summary>
/// Query to retrieve paginated affiliate links for the authenticated user.
/// </summary>
public sealed record GetAffiliateLinksQuery(int Page = 1, int PageSize = 20)
    : IRequest<GetAffiliateLinksResponse>;
