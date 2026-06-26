using Cashback.Application.Interfaces;
using Cashback.Domain.Entities;
using Cashback.Domain.Enums;
using Cashback.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Cashback.Persistence.Repositories;

/// <summary>
/// Entity Framework implementation of order persistence.
/// </summary>
public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the order repository.
    /// </summary>
    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<Order?> GetByNetworkOrderIdAsync(
        string networkOrderId,
        CancellationToken cancellationToken)
    {
        return await _context.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(order => order.NetworkOrderId == networkOrderId, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<Order?> GetByNetworkOrderIdForUpdateAsync(
        string networkOrderId,
        CancellationToken cancellationToken)
    {
        return await _context.Orders
            .FirstOrDefaultAsync(order => order.NetworkOrderId == networkOrderId, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<Order?> GetByIdForUserAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken)
    {
        return await _context.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(order => order.Id == id && order.UserId == userId, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<(IReadOnlyList<Order> Items, int TotalCount)> GetPagedByUserIdAsync(
        Guid userId,
        int page,
        int pageSize,
        OrderStatus? status,
        string sortBy,
        string sortDirection,
        CancellationToken cancellationToken)
    {
        var query = _context.Orders
            .AsNoTracking()
            .Where(order => order.UserId == userId);

        if (status.HasValue)
        {
            query = query.Where(order => order.Status == status.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await ApplySorting(query, sortBy, sortDirection)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    /// <inheritdoc/>
    public async Task<OrderUserSummary> GetUserSummaryAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var orders = _context.Orders
            .AsNoTracking()
            .Where(order => order.UserId == userId);

        return new OrderUserSummary(
            await orders.CountAsync(cancellationToken),
            await orders.CountAsync(order => order.Status == OrderStatus.Pending, cancellationToken),
            await orders.CountAsync(order => order.Status == OrderStatus.Approved, cancellationToken),
            await orders.CountAsync(order => order.Status == OrderStatus.Rejected, cancellationToken),
            await orders.SumAsync(order => order.CommissionAmount, cancellationToken),
            await orders.SumAsync(order => order.CashbackAmount, cancellationToken));
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<Order>> GetRecentByUserIdAsync(
        Guid userId,
        int count,
        CancellationToken cancellationToken)
    {
        return await _context.Orders
            .AsNoTracking()
            .Where(order => order.UserId == userId)
            .OrderByDescending(order => order.CreatedAt)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<MonthlyCashbackTotal>> GetMonthlyCashbackTotalsAsync(
        Guid userId,
        int monthCount,
        CancellationToken cancellationToken)
    {
        var utcNow = DateTime.UtcNow;
        var startMonth = new DateTime(utcNow.Year, utcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc)
            .AddMonths(-(monthCount - 1));

        var grouped = await _context.Orders
            .AsNoTracking()
            .Where(order => order.UserId == userId && order.CreatedAt >= startMonth)
            .GroupBy(order => new { order.CreatedAt.Year, order.CreatedAt.Month })
            .Select(group => new MonthlyCashbackTotal(
                group.Key.Year,
                group.Key.Month,
                group.Sum(order => order.CashbackAmount)))
            .ToListAsync(cancellationToken);

        var results = new List<MonthlyCashbackTotal>(monthCount);

        for (var index = 0; index < monthCount; index++)
        {
            var monthDate = startMonth.AddMonths(index);
            var existing = grouped.FirstOrDefault(
                item => item.Year == monthDate.Year && item.Month == monthDate.Month);

            results.Add(existing ?? new MonthlyCashbackTotal(monthDate.Year, monthDate.Month, 0m));
        }

        return results;
    }

    /// <inheritdoc/>
    public async Task<EarningsByStatus> GetEarningsByStatusAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var totalsByStatus = await _context.Orders
            .AsNoTracking()
            .Where(order => order.UserId == userId)
            .GroupBy(order => order.Status)
            .Select(group => new
            {
                group.Key,
                TotalCashback = group.Sum(order => order.CashbackAmount)
            })
            .ToListAsync(cancellationToken);

        return new EarningsByStatus(
            totalsByStatus.FirstOrDefault(item => item.Key == OrderStatus.Pending)?.TotalCashback ?? 0m,
            totalsByStatus.FirstOrDefault(item => item.Key == OrderStatus.Approved)?.TotalCashback ?? 0m,
            totalsByStatus.FirstOrDefault(item => item.Key == OrderStatus.Rejected)?.TotalCashback ?? 0m);
    }

    /// <inheritdoc/>
    public async Task AddAsync(Order order, CancellationToken cancellationToken)
    {
        await _context.Orders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(Order order, CancellationToken cancellationToken)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Applies sorting to an order query.
    /// </summary>
    private static IQueryable<Order> ApplySorting(
        IQueryable<Order> query,
        string sortBy,
        string sortDirection)
    {
        var descending = sortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);

        return sortBy.ToLowerInvariant() switch
        {
            "commissionamount" => descending
                ? query.OrderByDescending(order => order.CommissionAmount)
                : query.OrderBy(order => order.CommissionAmount),
            "cashbackamount" => descending
                ? query.OrderByDescending(order => order.CashbackAmount)
                : query.OrderBy(order => order.CashbackAmount),
            "status" => descending
                ? query.OrderByDescending(order => order.Status)
                : query.OrderBy(order => order.Status),
            _ => descending
                ? query.OrderByDescending(order => order.CreatedAt)
                : query.OrderBy(order => order.CreatedAt)
        };
    }
}
