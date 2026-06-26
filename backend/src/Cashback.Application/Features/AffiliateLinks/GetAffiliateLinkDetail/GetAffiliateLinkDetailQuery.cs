using Cashback.Application.Features.AffiliateLinks.Common;
using MediatR;

namespace Cashback.Application.Features.AffiliateLinks.GetAffiliateLinkDetail;

/// <summary>
/// Query to retrieve a single affiliate link for the authenticated user.
/// </summary>
public sealed record GetAffiliateLinkDetailQuery(Guid Id) : IRequest<AffiliateLinkDetailDto>;
