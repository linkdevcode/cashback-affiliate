using Cashback.Domain.Entities;

namespace Cashback.Application.Features.Admin.Withdrawals.Common;

/// <summary>
/// Maps withdrawal entities to admin withdrawal DTOs.
/// </summary>
public static class AdminWithdrawalMapper
{
    /// <summary>
    /// Maps a withdrawal entity to a list DTO.
    /// </summary>
    public static AdminWithdrawalDto ToListItem(Withdrawal withdrawal)
    {
        return new AdminWithdrawalDto
        {
            Id = withdrawal.Id,
            User = ToUserDto(withdrawal.User),
            Amount = withdrawal.Amount,
            Status = withdrawal.Status,
            StatusName = withdrawal.Status.ToString(),
            RequestedAt = withdrawal.RequestedAt
        };
    }

    /// <summary>
    /// Maps a withdrawal entity to a detail DTO.
    /// </summary>
    public static AdminWithdrawalDetailDto ToDetail(Withdrawal withdrawal)
    {
        return new AdminWithdrawalDetailDto
        {
            Id = withdrawal.Id,
            User = ToUserDto(withdrawal.User),
            Amount = withdrawal.Amount,
            BankName = withdrawal.BankName,
            BankAccountNumber = withdrawal.BankAccountNumber,
            BankAccountHolder = withdrawal.BankAccountHolder,
            Status = withdrawal.Status,
            StatusName = withdrawal.Status.ToString(),
            RequestedAt = withdrawal.RequestedAt,
            ProcessedAt = withdrawal.ProcessedAt
        };
    }

    /// <summary>
    /// Maps a user entity to an admin withdrawal user DTO.
    /// </summary>
    private static AdminWithdrawalUserDto ToUserDto(User user)
    {
        return new AdminWithdrawalUserDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName
        };
    }
}
