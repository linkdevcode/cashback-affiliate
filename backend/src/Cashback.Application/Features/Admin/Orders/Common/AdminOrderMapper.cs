using Cashback.Domain.Entities;

namespace Cashback.Application.Features.Admin.Orders.Common;

/// <summary>
/// Maps order entities to admin order DTOs.
/// </summary>
public static class AdminOrderMapper
{
    /// <summary>
    /// Maps an order entity to a list DTO.
    /// </summary>
    public static AdminOrderDto ToListItem(Order order)
    {
        return new AdminOrderDto
        {
            Id = order.Id,
            OrderCode = order.NetworkOrderId,
            User = ToUserDto(order.User),
            CommissionAmount = order.CommissionAmount,
            CashbackAmount = order.CashbackAmount,
            Status = order.Status,
            StatusName = order.Status.ToString(),
            CreatedAt = order.CreatedAt
        };
    }

    /// <summary>
    /// Maps an order entity to a detail DTO.
    /// </summary>
    public static AdminOrderDetailDto ToDetail(Order order)
    {
        return new AdminOrderDetailDto
        {
            Id = order.Id,
            OrderCode = order.NetworkOrderId,
            User = ToUserDto(order.User),
            Merchant = order.Merchant?.ToString(),
            OrderAmount = order.OrderAmount,
            CommissionAmount = order.CommissionAmount,
            CashbackAmount = order.CashbackAmount,
            PlatformProfit = order.PlatformProfit,
            Status = order.Status,
            StatusName = order.Status.ToString(),
            CreatedAt = order.CreatedAt
        };
    }

    /// <summary>
    /// Maps a user entity to an admin order user DTO.
    /// </summary>
    private static AdminOrderUserDto ToUserDto(User user)
    {
        return new AdminOrderUserDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName
        };
    }
}
