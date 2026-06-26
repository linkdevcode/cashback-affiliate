namespace Cashback.Application.Interfaces;

/// <summary>
/// Creates or updates orders from webhook synchronization events.
/// </summary>
public interface IOrderSynchronizationService
{
    /// <summary>
    /// Creates a new order or updates an existing order to the target status.
    /// </summary>
    Task<OrderSyncResponse> SynchronizeAsync(
        OrderSyncRequest request,
        CancellationToken cancellationToken);
}
