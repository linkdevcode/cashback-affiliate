using Cashback.Domain.Entities;
using Cashback.Domain.Enums;

namespace Cashback.Application.Features.Orders.Common;

/// <summary>
/// Maps order entities to API DTOs.
/// </summary>
public static class OrderMapper
{
    /// <summary>
    /// Maps an order entity to a list DTO.
    /// </summary>
    public static OrderDto ToDto(Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            OrderCode = order.NetworkOrderId,
            Merchant = FormatMerchant(order.Merchant),
            OrderAmount = order.OrderAmount,
            CommissionAmount = order.CommissionAmount,
            CashbackAmount = order.CashbackAmount,
            Status = order.Status,
            StatusName = FormatStatus(order.Status),
            CreatedAt = order.CreatedAt
        };
    }

    /// <summary>
    /// Maps an order entity to a detail DTO.
    /// </summary>
    public static OrderDetailDto ToDetailDto(Order order)
    {
        return new OrderDetailDto
        {
            Id = order.Id,
            OrderCode = order.NetworkOrderId,
            Merchant = FormatMerchant(order.Merchant),
            OrderAmount = order.OrderAmount,
            CommissionAmount = order.CommissionAmount,
            CashbackAmount = order.CashbackAmount,
            PlatformProfit = order.PlatformProfit,
            Status = order.Status,
            StatusName = FormatStatus(order.Status),
            CreatedAt = order.CreatedAt
        };
    }

    /// <summary>
    /// Formats a merchant enum value for API responses.
    /// </summary>
    private static string? FormatMerchant(MerchantType? merchant)
    {
        return merchant?.ToString();
    }

    /// <summary>
    /// Formats an order status enum value for API responses.
    /// </summary>
    private static string FormatStatus(OrderStatus status)
    {
        return status.ToString();
    }
}
