using Cashback.Application.Interfaces;
using Cashback.Domain.Entities;
using Cashback.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Cashback.Persistence.Repositories;

/// <summary>
/// Entity Framework implementation of webhook event persistence.
/// </summary>
public class WebhookEventRepository : IWebhookEventRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the webhook event repository.
    /// </summary>
    public WebhookEventRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<WebhookEvent?> GetByProviderAndOrderIdAsync(
        string provider,
        string providerOrderId,
        CancellationToken cancellationToken)
    {
        return await _context.WebhookEvents
            .AsNoTracking()
            .FirstOrDefaultAsync(
                webhookEvent =>
                    webhookEvent.Provider == provider
                    && webhookEvent.ProviderOrderId == providerOrderId,
                cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<WebhookEvent?> GetByIdForUpdateAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.WebhookEvents
            .FirstOrDefaultAsync(webhookEvent => webhookEvent.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<WebhookEvent> RegisterAsync(
        WebhookEvent webhookEvent,
        CancellationToken cancellationToken)
    {
        var existingEvent = await _context.WebhookEvents
            .FirstOrDefaultAsync(
                currentEvent =>
                    currentEvent.Provider == webhookEvent.Provider
                    && currentEvent.ProviderOrderId == webhookEvent.ProviderOrderId,
                cancellationToken);

        if (existingEvent is not null)
        {
            return existingEvent;
        }

        try
        {
            await _context.WebhookEvents.AddAsync(webhookEvent, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return webhookEvent;
        }
        catch (DbUpdateException)
        {
            var concurrentEvent = await _context.WebhookEvents
                .FirstAsync(
                    currentEvent =>
                        currentEvent.Provider == webhookEvent.Provider
                        && currentEvent.ProviderOrderId == webhookEvent.ProviderOrderId,
                    cancellationToken);

            return concurrentEvent;
        }
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        _context.WebhookEvents.Update(webhookEvent);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
