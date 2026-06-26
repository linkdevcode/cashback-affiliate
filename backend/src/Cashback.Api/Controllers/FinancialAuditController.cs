using Cashback.Api.Models;
using Cashback.Application.Features.FinancialAudit.Common;
using Cashback.Application.Features.FinancialAudit.GetBalanceAudit;
using Cashback.Application.Features.FinancialAudit.GetFinancialTransactions;
using Cashback.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashback.Api.Controllers;

/// <summary>
/// Financial audit trail endpoints.
/// </summary>
[ApiController]
[Route("api/v1/financial")]
[Authorize]
public sealed class FinancialAuditController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the financial audit controller.
    /// </summary>
    public FinancialAuditController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns a balance audit report for the authenticated user.
    /// </summary>
    [HttpGet("balance-audit")]
    [ProducesResponseType(typeof(ApiResponse<BalanceAuditDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBalanceAudit(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBalanceAuditQuery(), cancellationToken);

        return Ok(ApiResponse<BalanceAuditDto>.Ok(result));
    }

    /// <summary>
    /// Returns paginated financial transactions for the authenticated user.
    /// </summary>
    [HttpGet("transactions")]
    [ProducesResponseType(typeof(ApiResponse<GetFinancialTransactionsResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFinancialTransactions(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] TransactionType? type = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new GetFinancialTransactionsQuery(page, pageSize, type),
            cancellationToken);

        return Ok(ApiResponse<GetFinancialTransactionsResponse>.Ok(result));
    }
}
