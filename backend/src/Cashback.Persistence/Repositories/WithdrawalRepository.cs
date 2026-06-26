using Cashback.Application.Interfaces;
using Cashback.Domain.Enums;
using Cashback.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Cashback.Persistence.Repositories;

/// <summary>
/// Entity Framework implementation of withdrawal persistence.
/// </summary>
public class WithdrawalRepository : IWithdrawalRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the withdrawal repository.
    /// </summary>
    public WithdrawalRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<decimal> GetTotalCompletedAmountByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        return await _context.WithdrawRequests
            .AsNoTracking()
            .Where(request => request.UserId == userId && request.Status == WithdrawalStatus.Completed)
            .SumAsync(request => request.Amount, cancellationToken);
    }
}
