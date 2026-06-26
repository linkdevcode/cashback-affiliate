using Cashback.Api.Models;
using Cashback.Api.Models.Withdrawals;
using Cashback.Application.Features.Withdrawals.Common;
using Cashback.Application.Features.Withdrawals.CreateWithdrawal;
using Cashback.Application.Features.Withdrawals.GetWithdrawalDetail;
using Cashback.Application.Features.Withdrawals.GetWithdrawals;
using Cashback.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashback.Api.Controllers;

/// <summary>
/// Withdrawal request endpoints.
/// </summary>
[ApiController]
[Route("api/v1/withdrawals")]
[Authorize]
public sealed class WithdrawalsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the withdrawals controller.
    /// </summary>
    public WithdrawalsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new withdrawal request for the authenticated user.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CreateWithdrawalResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateWithdrawal(
        [FromBody] CreateWithdrawalRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new CreateWithdrawalCommand(
                request.Amount,
                request.BankName,
                request.BankAccountNumber,
                request.BankAccountName),
            cancellationToken);

        return Ok(ApiResponse<CreateWithdrawalResponse>.Ok(
            result,
            "Withdrawal request submitted"));
    }

    /// <summary>
    /// Returns paginated withdrawal history for the authenticated user.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<GetWithdrawalsResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWithdrawals(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] WithdrawalStatus? status = null,
        [FromQuery] string sortBy = "requestedAt",
        [FromQuery(Name = "direction")] string sortDirection = "desc",
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new GetWithdrawalsQuery(page, pageSize, status, sortBy, sortDirection),
            cancellationToken);

        return Ok(ApiResponse<GetWithdrawalsResponse>.Ok(result));
    }

    /// <summary>
    /// Returns details for a single withdrawal owned by the authenticated user.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<WithdrawalDetailDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWithdrawalDetail(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetWithdrawalDetailQuery(id), cancellationToken);

        return Ok(ApiResponse<WithdrawalDetailDto>.Ok(result));
    }
}
