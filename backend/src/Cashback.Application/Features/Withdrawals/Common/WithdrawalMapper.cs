using Cashback.Domain.Entities;
using Cashback.Domain.Enums;

namespace Cashback.Application.Features.Withdrawals.Common;

/// <summary>
/// Maps withdrawal entities to API DTOs.
/// </summary>
public static class WithdrawalMapper
{
    /// <summary>
    /// Maps a withdrawal entity to a list DTO.
    /// </summary>
    public static WithdrawalDto ToDto(Withdrawal withdrawal)
    {
        return new WithdrawalDto
        {
            Id = withdrawal.Id,
            Amount = withdrawal.Amount,
            Status = withdrawal.Status,
            StatusName = FormatStatus(withdrawal.Status),
            RequestedAt = withdrawal.RequestedAt,
            ProcessedAt = withdrawal.ProcessedAt
        };
    }

    /// <summary>
    /// Maps a withdrawal entity to a detail DTO.
    /// </summary>
    public static WithdrawalDetailDto ToDetailDto(Withdrawal withdrawal)
    {
        return new WithdrawalDetailDto
        {
            Id = withdrawal.Id,
            Amount = withdrawal.Amount,
            Status = withdrawal.Status,
            StatusName = FormatStatus(withdrawal.Status),
            RequestedAt = withdrawal.RequestedAt,
            ProcessedAt = withdrawal.ProcessedAt
        };
    }

    /// <summary>
    /// Formats a withdrawal status enum value for API responses.
    /// </summary>
    private static string FormatStatus(WithdrawalStatus status)
    {
        return status.ToString();
    }
}
