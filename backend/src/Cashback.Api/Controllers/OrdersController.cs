using Cashback.Api.Models;
using Cashback.Application.Features.Orders.Common;
using Cashback.Application.Features.Orders.GetOrderDetail;
using Cashback.Application.Features.Orders.GetOrders;
using Cashback.Application.Features.Orders.GetOrderSummary;
using Cashback.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashback.Api.Controllers;

/// <summary>
/// Order history and summary endpoints.
/// </summary>
[ApiController]
[Route("api/v1/orders")]
[Authorize]
public sealed class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the orders controller.
    /// </summary>
    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns paginated order history for the authenticated user.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<GetOrdersResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrders(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] OrderStatus? status = null,
        [FromQuery] string sortBy = "createdAt",
        [FromQuery(Name = "direction")] string sortDirection = "desc",
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new GetOrdersQuery(page, pageSize, status, sortBy, sortDirection),
            cancellationToken);

        return Ok(ApiResponse<GetOrdersResponse>.Ok(result));
    }

    /// <summary>
    /// Returns aggregated order statistics for the authenticated user.
    /// </summary>
    [HttpGet("summary")]
    [ProducesResponseType(typeof(ApiResponse<GetOrderSummaryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrderSummary(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetOrderSummaryQuery(), cancellationToken);

        return Ok(ApiResponse<GetOrderSummaryResponse>.Ok(result));
    }

    /// <summary>
    /// Returns details for a single order owned by the authenticated user.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<OrderDetailDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrderDetail(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetOrderDetailQuery(id), cancellationToken);

        return Ok(ApiResponse<OrderDetailDto>.Ok(result));
    }
}
