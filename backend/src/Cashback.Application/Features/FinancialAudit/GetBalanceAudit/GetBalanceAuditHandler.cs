using Cashback.Application.Features.FinancialAudit.Common;
using Cashback.Application.Interfaces;
using Cashback.Domain.Exceptions;
using MediatR;

namespace Cashback.Application.Features.FinancialAudit.GetBalanceAudit;

/// <summary>
/// Handles retrieval of a balance audit report for the authenticated user.
/// </summary>
public sealed class GetBalanceAuditHandler : IRequestHandler<GetBalanceAuditQuery, BalanceAuditDto>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IBalanceService _balanceService;

    /// <summary>
    /// Initializes a new instance of the get balance audit handler.
    /// </summary>
    public GetBalanceAuditHandler(
        ICurrentUserService currentUserService,
        IBalanceService balanceService)
    {
        _currentUserService = currentUserService;
        _balanceService = balanceService;
    }

    /// <inheritdoc/>
    public async Task<BalanceAuditDto> Handle(
        GetBalanceAuditQuery request,
        CancellationToken cancellationToken)
    {
        var userId = GetAuthenticatedUserId();
        var audit = await _balanceService.GetBalanceAuditAsync(userId, cancellationToken);

        return FinancialAuditMapper.ToDto(audit);
    }

    /// <summary>
    /// Resolves the authenticated user identifier from the request context.
    /// </summary>
    private Guid GetAuthenticatedUserId()
    {
        if (!_currentUserService.IsAuthenticated || _currentUserService.UserId is null)
        {
            throw new UnauthorizedException("Authentication is required.");
        }

        return _currentUserService.UserId.Value;
    }
}
