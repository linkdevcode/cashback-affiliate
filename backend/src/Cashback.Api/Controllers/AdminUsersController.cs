using Cashback.Api.Models;
using Cashback.Application.Features.Admin.Users.ActivateUser;
using Cashback.Application.Features.Admin.Users.Common;
using Cashback.Application.Features.Admin.Users.GetUserDetail;
using Cashback.Application.Features.Admin.Users.GetUsers;
using Cashback.Application.Features.Admin.Users.SuspendUser;
using Cashback.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cashback.Api.Controllers;

/// <summary>
/// Admin user management endpoints.
/// </summary>
[Route("api/v1/admin/users")]
public sealed class AdminUsersController : AdminControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the admin users controller.
    /// </summary>
    public AdminUsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns a paginated list of users for admin management.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<GetUsersResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? email = null,
        [FromQuery] string? name = null,
        [FromQuery] UserStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new GetUsersQuery(page, pageSize, email, name, status),
            cancellationToken);

        return Ok(ApiResponse<GetUsersResponse>.Ok(result));
    }

    /// <summary>
    /// Returns detailed information for a single user.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<GetUserDetailResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserDetail(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUserDetailQuery(id), cancellationToken);

        return Ok(ApiResponse<GetUserDetailResponse>.Ok(result));
    }

    /// <summary>
    /// Suspends a user account.
    /// </summary>
    [HttpPost("{id:guid}/suspend")]
    [ProducesResponseType(typeof(ApiResponse<AdminUserActionResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SuspendUser(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new SuspendUserCommand(id), cancellationToken);

        return Ok(ApiResponse<AdminUserActionResponse>.Ok(result));
    }

    /// <summary>
    /// Activates a suspended user account.
    /// </summary>
    [HttpPost("{id:guid}/activate")]
    [ProducesResponseType(typeof(ApiResponse<AdminUserActionResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ActivateUser(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ActivateUserCommand(id), cancellationToken);

        return Ok(ApiResponse<AdminUserActionResponse>.Ok(result));
    }
}
