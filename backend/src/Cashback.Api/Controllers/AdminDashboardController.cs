using Cashback.Api.Models;
using Cashback.Application.Features.Admin.GetAdminDashboard;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cashback.Api.Controllers;

/// <summary>
/// Admin dashboard endpoints.
/// </summary>
[Route("api/v1/admin/dashboard")]
public sealed class AdminDashboardController : AdminControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the admin dashboard controller.
    /// </summary>
    public AdminDashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns aggregated platform statistics for administrators.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<GetAdminDashboardResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDashboardSummary(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAdminDashboardQuery(), cancellationToken);

        return Ok(ApiResponse<GetAdminDashboardResponse>.Ok(result));
    }
}
