using Cashback.Api.Models;
using Cashback.Api.Models.AffiliateLinks;
using Cashback.Application.Features.AffiliateLinks.Common;
using Cashback.Application.Features.AffiliateLinks.GenerateAffiliateLink;
using Cashback.Application.Features.AffiliateLinks.GetAffiliateLinkDetail;
using Cashback.Application.Features.AffiliateLinks.GetAffiliateLinks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashback.Api.Controllers;

/// <summary>
/// Affiliate link generation and history endpoints.
/// </summary>
[ApiController]
[Route("api/v1/affiliate-links")]
[Authorize]
public sealed class AffiliateLinksController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the affiliate links controller.
    /// </summary>
    public AffiliateLinksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Generates an affiliate tracking link from a Shopee product URL.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<GenerateAffiliateLinkResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GenerateAffiliateLink(
        [FromBody] GenerateAffiliateLinkRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GenerateAffiliateLinkCommand(request.Url),
            cancellationToken);

        return Ok(ApiResponse<GenerateAffiliateLinkResponse>.Ok(result));
    }

    /// <summary>
    /// Returns paginated affiliate link history for the authenticated user.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<GetAffiliateLinksResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAffiliateLinks(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new GetAffiliateLinksQuery(page, pageSize),
            cancellationToken);

        return Ok(ApiResponse<GetAffiliateLinksResponse>.Ok(result));
    }

    /// <summary>
    /// Returns details for a single affiliate link owned by the authenticated user.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<AffiliateLinkDetailDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAffiliateLinkDetail(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetAffiliateLinkDetailQuery(id),
            cancellationToken);

        return Ok(ApiResponse<AffiliateLinkDetailDto>.Ok(result));
    }
}
