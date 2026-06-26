using Cashback.Application.Features.Admin.Withdrawals.Common;
using Cashback.Application.Interfaces;
using Cashback.Domain.Exceptions;
using MediatR;

namespace Cashback.Application.Features.Admin.Withdrawals.GetWithdrawalDetail;

/// <summary>
/// Handles retrieval of detailed withdrawal information for admin management.
/// </summary>
public sealed class GetAdminWithdrawalDetailHandler
    : IRequestHandler<GetAdminWithdrawalDetailQuery, AdminWithdrawalDetailDto>
{
    private readonly IWithdrawalRepository _withdrawalRepository;

    /// <summary>
    /// Initializes a new instance of the get admin withdrawal detail handler.
    /// </summary>
    public GetAdminWithdrawalDetailHandler(IWithdrawalRepository withdrawalRepository)
    {
        _withdrawalRepository = withdrawalRepository;
    }

    /// <inheritdoc/>
    public async Task<AdminWithdrawalDetailDto> Handle(
        GetAdminWithdrawalDetailQuery request,
        CancellationToken cancellationToken)
    {
        var withdrawal = await _withdrawalRepository.GetByIdForAdminAsync(
            request.WithdrawalId,
            cancellationToken);

        if (withdrawal is null)
        {
            throw new NotFoundException("Withdrawal not found.");
        }

        return AdminWithdrawalMapper.ToDetail(withdrawal);
    }
}
