using Cashback.Api.Models;
using Cashback.Application.Features.Admin.Orders.Common;
using Cashback.Application.Features.Admin.Orders.GetOrderDetail;
using Cashback.Application.Features.Admin.Orders.GetOrders;
using Cashback.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cashback.Api.Controllers;

/// <summary>
/// Admin order management endpoints.
/// </summary>
[Route("api/v1/admin/orders")]
public sealed class AdminOrdersController : AdminControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the admin orders controller.
    /// </summary>
    public AdminOrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns a paginated list of orders for admin management.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<GetAdminOrdersResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrders(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? orderId = null,
        [FromQuery] string? user = null,
        [FromQuery] OrderStatus? status = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new GetAdminOrdersQuery(page, pageSize, orderId, user, status, fromDate, toDate),
            cancellationToken);

        return Ok(ApiResponse<GetAdminOrdersResponse>.Ok(result));
    }

    /// <summary>
    /// Returns detailed information for a single order.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<AdminOrderDetailDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrderDetail(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAdminOrderDetailQuery(id), cancellationToken);

        return Ok(ApiResponse<AdminOrderDetailDto>.Ok(result));
    }
}
