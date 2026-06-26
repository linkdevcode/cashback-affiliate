using Cashback.Application.Features.Withdrawals.Common;
using Cashback.Application.Interfaces;
using Cashback.Domain.Exceptions;
using MediatR;

namespace Cashback.Application.Features.Withdrawals.GetWithdrawalDetail;

/// <summary>
/// Handles retrieval of a single withdrawal with ownership validation.
/// </summary>
public sealed class GetWithdrawalDetailHandler
    : IRequestHandler<GetWithdrawalDetailQuery, WithdrawalDetailDto>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IWithdrawalRepository _withdrawalRepository;

    /// <summary>
    /// Initializes a new instance of the get withdrawal detail handler.
    /// </summary>
    public GetWithdrawalDetailHandler(
        ICurrentUserService currentUserService,
        IWithdrawalRepository withdrawalRepository)
    {
        _currentUserService = currentUserService;
        _withdrawalRepository = withdrawalRepository;
    }

    /// <inheritdoc/>
    public async Task<WithdrawalDetailDto> Handle(
        GetWithdrawalDetailQuery request,
        CancellationToken cancellationToken)
    {
        var userId = GetAuthenticatedUserId();

        var withdrawal = await _withdrawalRepository.GetByIdForUserAsync(
            request.Id,
            userId,
            cancellationToken);

        if (withdrawal is null)
        {
            throw new NotFoundException("Withdrawal not found.");
        }

        return WithdrawalMapper.ToDetailDto(withdrawal);
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
