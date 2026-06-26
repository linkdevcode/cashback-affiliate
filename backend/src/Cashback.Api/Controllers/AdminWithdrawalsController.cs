using Cashback.Api.Models;
using Cashback.Api.Models.Admin;
using Cashback.Application.Features.Admin.Withdrawals.Common;
using Cashback.Application.Features.Admin.Withdrawals.GetWithdrawalDetail;
using Cashback.Application.Features.Admin.Withdrawals.GetWithdrawals;
using Cashback.Application.Features.Withdrawals.ApproveWithdrawal;
using Cashback.Application.Features.Withdrawals.Common;
using Cashback.Application.Features.Withdrawals.CompleteWithdrawal;
using Cashback.Application.Features.Withdrawals.RejectWithdrawal;
using Cashback.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cashback.Api.Controllers;

/// <summary>
/// Admin endpoints for processing withdrawal requests.
/// </summary>
[Route("api/v1/admin/withdrawals")]
public sealed class AdminWithdrawalsController : AdminControllerBase
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
    /// Returns a paginated list of withdrawals for admin management.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<GetAdminWithdrawalsResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWithdrawals(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? user = null,
        [FromQuery] WithdrawalStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new GetAdminWithdrawalsQuery(page, pageSize, user, status),
            cancellationToken);

        return Ok(ApiResponse<GetAdminWithdrawalsResponse>.Ok(result));
    }

    /// <summary>
    /// Returns detailed information for a single withdrawal.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<AdminWithdrawalDetailDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWithdrawalDetail(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAdminWithdrawalDetailQuery(id), cancellationToken);

        return Ok(ApiResponse<AdminWithdrawalDetailDto>.Ok(result));
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
