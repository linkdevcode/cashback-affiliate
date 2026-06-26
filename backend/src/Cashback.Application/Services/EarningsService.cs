using Cashback.Application.Features.Earnings.Common;
using Cashback.Application.Interfaces;

namespace Cashback.Application.Services;

/// <summary>
/// Calculates user earnings summaries from order cashback amounts.
/// </summary>
public sealed class EarningsService : IEarningsService
{
    private readonly IOrderRepository _orderRepository;

    /// <summary>
    /// Initializes a new instance of the earnings service.
    /// </summary>
    public EarningsService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    /// <inheritdoc/>
    public async Task<EarningsSummaryDto> GetUserEarningsAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var earnings = await _orderRepository.GetEarningsByStatusAsync(userId, cancellationToken);

        return new EarningsSummaryDto
        {
            PendingCashback = earnings.PendingCashback,
            ApprovedCashback = earnings.ApprovedCashback,
            RejectedCashback = earnings.RejectedCashback
        };
    }
}
