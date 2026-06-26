using Cashback.Application.Features.Admin.Withdrawals.Common;
using Cashback.Application.Interfaces;
using MediatR;

namespace Cashback.Application.Features.Admin.Withdrawals.GetWithdrawals;

/// <summary>
/// Handles retrieval of paginated withdrawals for admin management.
/// </summary>
public sealed class GetAdminWithdrawalsHandler
    : IRequestHandler<GetAdminWithdrawalsQuery, GetAdminWithdrawalsResponse>
{
    private readonly IWithdrawalRepository _withdrawalRepository;

    /// <summary>
    /// Initializes a new instance of the get admin withdrawals handler.
    /// </summary>
    public GetAdminWithdrawalsHandler(IWithdrawalRepository withdrawalRepository)
    {
        _withdrawalRepository = withdrawalRepository;
    }

    /// <inheritdoc/>
    public async Task<GetAdminWithdrawalsResponse> Handle(
        GetAdminWithdrawalsQuery request,
        CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _withdrawalRepository.GetPagedForAdminAsync(
            request.Page,
            request.PageSize,
            request.User,
            request.Status,
            cancellationToken);

        return new GetAdminWithdrawalsResponse
        {
            Items = items.Select(AdminWithdrawalMapper.ToListItem).ToList(),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
