using Cashback.Application.Interfaces;
using Cashback.Domain.Entities;

namespace Cashback.Application.Features.Admin.Users.Common;

/// <summary>
/// Maps user entities to admin user DTOs.
/// </summary>
public static class AdminUserMapper
{
    /// <summary>
    /// Maps a user entity to a list item DTO.
    /// </summary>
    public static AdminUserDto ToListItem(User user)
    {
        return new AdminUserDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role,
            Status = user.Status,
            CreatedAt = user.CreatedAt
        };
    }

    /// <summary>
    /// Maps a user entity to a profile DTO.
    /// </summary>
    public static AdminUserProfileDto ToProfile(User user)
    {
        return new AdminUserProfileDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            AvatarUrl = user.AvatarUrl,
            Role = user.Role,
            Status = user.Status,
            AvailableBalance = user.AvailableBalance,
            PendingBalance = user.PendingBalance,
            LifetimeCashback = user.LifetimeCashback,
            LastLoginAt = user.LastLoginAt,
            CreatedAt = user.CreatedAt
        };
    }

    /// <summary>
    /// Maps order statistics to an admin order summary DTO.
    /// </summary>
    public static AdminUserOrderSummaryDto ToOrderSummary(OrderUserSummary summary)
    {
        return new AdminUserOrderSummaryDto
        {
            TotalOrders = summary.TotalOrders,
            PendingOrders = summary.PendingOrders,
            ApprovedOrders = summary.ApprovedOrders,
            RejectedOrders = summary.RejectedOrders,
            TotalCommission = summary.TotalCommission,
            TotalCashback = summary.TotalCashback
        };
    }

    /// <summary>
    /// Maps withdrawal statistics to an admin withdrawal summary DTO.
    /// </summary>
    public static AdminUserWithdrawalSummaryDto ToWithdrawalSummary(WithdrawalUserSummary summary)
    {
        return new AdminUserWithdrawalSummaryDto
        {
            TotalWithdrawals = summary.TotalWithdrawals,
            PendingWithdrawals = summary.PendingWithdrawals,
            ApprovedWithdrawals = summary.ApprovedWithdrawals,
            RejectedWithdrawals = summary.RejectedWithdrawals,
            CompletedWithdrawals = summary.CompletedWithdrawals,
            TotalWithdrawn = summary.TotalWithdrawn
        };
    }

    /// <summary>
    /// Maps a user entity to a status action response.
    /// </summary>
    public static AdminUserActionResponse ToActionResponse(User user)
    {
        return new AdminUserActionResponse
        {
            Id = user.Id,
            Status = (int)user.Status,
            StatusName = user.Status.ToString()
        };
    }
}
