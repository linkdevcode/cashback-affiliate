using Cashback.Application.Features.FinancialAudit.Common;
using Cashback.Application.Interfaces;
using Cashback.Domain.Exceptions;
using MediatR;

namespace Cashback.Application.Features.FinancialAudit.GetFinancialTransactions;

/// <summary>
/// Handles retrieval of paginated financial transactions for the authenticated user.
/// </summary>
public sealed class GetFinancialTransactionsHandler
    : IRequestHandler<GetFinancialTransactionsQuery, GetFinancialTransactionsResponse>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ITransactionRepository _transactionRepository;

    /// <summary>
    /// Initializes a new instance of the get financial transactions handler.
    /// </summary>
    public GetFinancialTransactionsHandler(
        ICurrentUserService currentUserService,
        ITransactionRepository transactionRepository)
    {
        _currentUserService = currentUserService;
        _transactionRepository = transactionRepository;
    }

    /// <inheritdoc/>
    public async Task<GetFinancialTransactionsResponse> Handle(
        GetFinancialTransactionsQuery request,
        CancellationToken cancellationToken)
    {
        var userId = GetAuthenticatedUserId();

        var (items, totalCount) = await _transactionRepository.GetPagedByUserIdAsync(
            userId,
            request.Page,
            request.PageSize,
            request.Type,
            cancellationToken);

        return new GetFinancialTransactionsResponse
        {
            Items = items.Select(FinancialAuditMapper.ToDto).ToList(),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
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
