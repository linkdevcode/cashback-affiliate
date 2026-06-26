using Cashback.Application.Interfaces;
using Cashback.Application.Features.FinancialAudit.Common;

namespace Cashback.Application.Features.FinancialAudit.Common;

/// <summary>
/// Maps financial audit results to API DTOs.
/// </summary>
public static class FinancialAuditMapper
{
    /// <summary>
    /// Maps a balance audit result to an API DTO.
    /// </summary>
    public static BalanceAuditDto ToDto(BalanceAuditResult result)
    {
        return new BalanceAuditDto
        {
            ApprovedCashback = result.ApprovedCashback,
            PendingWithdrawals = result.PendingWithdrawals,
            CompletedWithdrawals = result.CompletedWithdrawals,
            ApprovedWithdrawals = result.ApprovedWithdrawals,
            AvailableBalance = result.AvailableBalance,
            OperationalAvailableBalance = result.OperationalAvailableBalance,
            HasNegativeBalance = result.HasNegativeBalance,
            IsConsistent = result.IsConsistent,
            TransactionValidation = ToDto(result.TransactionValidation)
        };
    }

    /// <summary>
    /// Maps a transaction entity to an audit list DTO.
    /// </summary>
    public static TransactionAuditDto ToDto(Domain.Entities.Transaction transaction)
    {
        return new TransactionAuditDto
        {
            Id = transaction.Id,
            Type = transaction.Type,
            TypeName = transaction.Type.ToString(),
            Amount = transaction.Amount,
            BalanceBefore = transaction.BalanceBefore,
            BalanceAfter = transaction.BalanceAfter,
            ReferenceId = transaction.ReferenceId,
            Description = transaction.Description,
            CreatedAt = transaction.CreatedAt
        };
    }

    /// <summary>
    /// Maps a transaction validation result to an API DTO.
    /// </summary>
    private static TransactionValidationDto ToDto(TransactionValidationResult result)
    {
        return new TransactionValidationDto
        {
            IsValid = result.IsValid,
            HasNegativeBalance = result.HasNegativeBalance,
            LedgerBalance = result.LedgerBalance,
            Errors = result.Errors
        };
    }
}
