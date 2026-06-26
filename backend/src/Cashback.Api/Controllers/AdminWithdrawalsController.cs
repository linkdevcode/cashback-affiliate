using Cashback.Api.Models;
using Cashback.Api.Models.Admin;
using Cashback.Application.Features.Withdrawals.ApproveWithdrawal;
using Cashback.Application.Features.Withdrawals.Common;
using Cashback.Application.Features.Withdrawals.CompleteWithdrawal;
using Cashback.Application.Features.Withdrawals.RejectWithdrawal;
using Cashback.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashback.Api.Controllers;

/// <summary>
/// Admin endpoints for processing withdrawal requests.
/// </summary>
[ApiController]
[Route("api/v1/admin/withdrawals")]
[Authorize(Roles = nameof(UserRole.Admin))]
public sealed class AdminWithdrawalsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the admin withdrawals controller.
    /// </summary>
    public AdminWithdrawalsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Approves a pending withdrawal request.
    /// </summary>
    [HttpPost("{id:guid}/approve")]
    [ProducesResponseType(typeof(ApiResponse<WithdrawalActionResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ApproveWithdrawal(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ApproveWithdrawalCommand(id), cancellationToken);

        return Ok(ApiResponse<WithdrawalActionResponse>.Ok(result));
    }

    /// <summary>
    /// Rejects a pending withdrawal request and restores the user's balance.
    /// </summary>
    [HttpPost("{id:guid}/reject")]
    [ProducesResponseType(typeof(ApiResponse<WithdrawalActionResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RejectWithdrawal(
        Guid id,
        [FromBody] RejectWithdrawalRequest? request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new RejectWithdrawalCommand(id, request?.Reason),
            cancellationToken);

        return Ok(ApiResponse<WithdrawalActionResponse>.Ok(result));
    }

    /// <summary>
    /// Marks an approved withdrawal as completed.
    /// </summary>
    [HttpPost("{id:guid}/complete")]
    [ProducesResponseType(typeof(ApiResponse<WithdrawalActionResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CompleteWithdrawal(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CompleteWithdrawalCommand(id), cancellationToken);

        return Ok(ApiResponse<WithdrawalActionResponse>.Ok(result));
    }
}
