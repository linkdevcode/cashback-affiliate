using MediatR;

namespace Cashback.Application.Features.AffiliateLinks.GenerateAffiliateLink;

/// <summary>
/// Command to generate an affiliate tracking link from a product URL.
/// </summary>
public sealed record GenerateAffiliateLinkCommand(string Url) : IRequest<GenerateAffiliateLinkResponse>;
