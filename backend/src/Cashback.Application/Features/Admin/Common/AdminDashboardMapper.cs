using Cashback.Application.Features.Admin.GetAdminDashboard;
using Cashback.Domain.Entities;

namespace Cashback.Application.Features.Admin.Common;

/// <summary>
/// Maps admin dashboard DTOs to API responses.
/// </summary>
public static class AdminDashboardMapper
{
    /// <summary>
    /// Maps an admin dashboard summary DTO to its response model.
    /// </summary>
    public static GetAdminDashboardResponse ToResponse(AdminDashboardSummaryDto summary)
    {
        return new GetAdminDashboardResponse
        {
            TotalUsers = summary.TotalUsers,
            ActiveUsers = summary.ActiveUsers,
            SuspendedUsers = summary.SuspendedUsers,
            TotalOrders = summary.TotalOrders,
            PendingOrders = summary.PendingOrders,
            ApprovedOrders = summary.ApprovedOrders,
            RejectedOrders = summary.RejectedOrders,
            TotalWithdrawals = summary.TotalWithdrawals,
            PendingWithdrawals = summary.PendingWithdrawals,
            CompletedWithdrawals = summary.CompletedWithdrawals,
            TotalCommission = summary.TotalCommission,
            TotalCashbackPaid = summary.TotalCashbackPaid,
            PlatformRevenue = summary.PlatformRevenue,
            OrdersByMonth = summary.OrdersByMonth,
            RevenueByMonth = summary.RevenueByMonth,
            RecentUsers = summary.RecentUsers,
            RecentOrders = summary.RecentOrders,
            RecentWithdrawals = summary.RecentWithdrawals
        };
    }

    /// <summary>
    /// Maps a user entity to a recent user DTO.
    /// </summary>
    public static AdminRecentUserDto ToRecentUser(User user)
    {
        return new AdminRecentUserDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Status = user.Status,
            StatusName = user.Status.ToString(),
            CreatedAt = user.CreatedAt
        };
    }

    /// <summary>
    /// Maps an order entity to a recent order DTO.
    /// </summary>
    public static AdminRecentOrderDto ToRecentOrder(Order order)
    {
        return new AdminRecentOrderDto
        {
            Id = order.Id,
            OrderCode = order.NetworkOrderId,
            UserEmail = order.User.Email,
            Status = order.Status,
            StatusName = order.Status.ToString(),
            CashbackAmount = order.CashbackAmount,
            CreatedAt = order.CreatedAt
        };
    }

    /// <summary>
    /// Maps a withdrawal entity to a recent withdrawal DTO.
    /// </summary>
    public static AdminRecentWithdrawalDto ToRecentWithdrawal(Withdrawal withdrawal)
    {
        return new AdminRecentWithdrawalDto
        {
            Id = withdrawal.Id,
            UserEmail = withdrawal.User.Email,
            Amount = withdrawal.Amount,
            Status = withdrawal.Status,
            StatusName = withdrawal.Status.ToString(),
            RequestedAt = withdrawal.RequestedAt
        };
    }
}
