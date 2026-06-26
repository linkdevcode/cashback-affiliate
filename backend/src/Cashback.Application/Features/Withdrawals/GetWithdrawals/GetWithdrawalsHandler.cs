using Cashback.Application.Features.Withdrawals.Common;
using Cashback.Application.Interfaces;
using Cashback.Domain.Exceptions;
using MediatR;

namespace Cashback.Application.Features.Withdrawals.GetWithdrawals;

/// <summary>
/// Handles retrieval of paginated withdrawals for the authenticated user.
/// </summary>
public sealed class GetWithdrawalsHandler : IRequestHandler<GetWithdrawalsQuery, GetWithdrawalsResponse>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IWithdrawalRepository _withdrawalRepository;

    /// <summary>
    /// Initializes a new instance of the get withdrawals handler.
    /// </summary>
    public GetWithdrawalsHandler(
        ICurrentUserService currentUserService,
        IWithdrawalRepository withdrawalRepository)
    {
        _currentUserService = currentUserService;
        _withdrawalRepository = withdrawalRepository;
    }

    /// <inheritdoc/>
    public async Task<GetWithdrawalsResponse> Handle(
        GetWithdrawalsQuery request,
        CancellationToken cancellationToken)
    {
        var userId = GetAuthenticatedUserId();

        var (items, totalCount) = await _withdrawalRepository.GetPagedByUserIdAsync(
            userId,
            request.Page,
            request.PageSize,
            request.Status,
            request.SortBy,
            request.SortDirection,
            cancellationToken);

        return new GetWithdrawalsResponse
        {
            Items = items.Select(WithdrawalMapper.ToDto).ToList(),
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
