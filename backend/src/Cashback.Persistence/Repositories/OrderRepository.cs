using Cashback.Application.Interfaces;
using Cashback.Domain.Entities;
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
}
