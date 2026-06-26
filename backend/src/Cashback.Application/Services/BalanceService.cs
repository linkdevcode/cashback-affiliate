using Cashback.Application.Interfaces;
using Cashback.Domain.Entities;
using Cashback.Domain.Enums;

namespace Cashback.Application.Services;

/// <summary>
/// Calculates balances and validates financial transaction consistency.
/// </summary>
public sealed class BalanceService : IBalanceService
{
    private readonly IEarningsService _earningsService;
    private readonly IWithdrawalRepository _withdrawalRepository;
    private readonly ITransactionRepository _transactionRepository;

    /// <summary>
    /// Initializes a new instance of the balance service.
    /// </summary>
    public BalanceService(
        IEarningsService earningsService,
        IWithdrawalRepository withdrawalRepository,
        ITransactionRepository transactionRepository)
    {
        _earningsService = earningsService;
        _withdrawalRepository = withdrawalRepository;
        _transactionRepository = transactionRepository;
    }

    /// <inheritdoc/>
    public async Task<decimal> GetAvailableBalanceAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var components = await GetBalanceComponentsAsync(userId, cancellationToken);

        return components.VerifiedAvailableBalance;
    }

    /// <inheritdoc/>
    public async Task<decimal> GetOperationalAvailableBalanceAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var components = await GetBalanceComponentsAsync(userId, cancellationToken);

        return components.OperationalAvailableBalance;
    }

    /// <inheritdoc/>
    public async Task<TransactionValidationResult> ValidateTransactionsAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var transactions = await _transactionRepository.GetOrderedByUserIdAsync(
            userId,
            cancellationToken);

        return ValidateTransactionChain(transactions);
    }

    /// <inheritdoc/>
    public async Task<BalanceAuditResult> GetBalanceAuditAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var components = await GetBalanceComponentsAsync(userId, cancellationToken);
        var transactionValidation = await ValidateTransactionsAsync(userId, cancellationToken);

        var isConsistent = transactionValidation.IsValid
            && (!transactionValidation.LedgerBalance.HasValue
                || transactionValidation.LedgerBalance.Value == components.OperationalAvailableBalance);

        var hasNegativeBalance = components.VerifiedAvailableBalance < 0
            || components.OperationalAvailableBalance < 0
            || transactionValidation.HasNegativeBalance;

        return new BalanceAuditResult
        {
            ApprovedCashback = components.ApprovedCashback,
            PendingWithdrawals = components.PendingWithdrawals,
            CompletedWithdrawals = components.CompletedWithdrawals,
            ApprovedWithdrawals = components.ApprovedWithdrawals,
            AvailableBalance = components.VerifiedAvailableBalance,
            OperationalAvailableBalance = components.OperationalAvailableBalance,
            HasNegativeBalance = hasNegativeBalance,
            IsConsistent = isConsistent,
            TransactionValidation = transactionValidation
        };
    }

    /// <summary>
    /// Validates a chronological transaction chain.
    /// </summary>
    internal static TransactionValidationResult ValidateTransactionChain(
        IReadOnlyList<Transaction> transactions)
    {
        if (transactions.Count == 0)
        {
            return new TransactionValidationResult
            {
                IsValid = true,
                HasNegativeBalance = false,
                LedgerBalance = null,
                Errors = []
            };
        }

        var errors = new List<string>();
        var hasNegativeBalance = false;
        Transaction? previousTransaction = null;

        foreach (var transaction in transactions)
        {
            if (transaction.BalanceAfter != transaction.BalanceBefore + transaction.Amount)
            {
                errors.Add(
                    $"Transaction {transaction.Id} has invalid balance transition.");
            }

            if (transaction.BalanceAfter < 0)
            {
                hasNegativeBalance = true;
                errors.Add(
                    $"Transaction {transaction.Id} resulted in a negative balance.");
            }

            if (previousTransaction is not null
                && transaction.BalanceBefore != previousTransaction.BalanceAfter)
            {
                errors.Add(
                    $"Transaction {transaction.Id} does not continue from the previous balance.");
            }

            previousTransaction = transaction;
        }

        return new TransactionValidationResult
        {
            IsValid = errors.Count == 0,
            HasNegativeBalance = hasNegativeBalance,
            LedgerBalance = previousTransaction?.BalanceAfter,
            Errors = errors
        };
    }

    /// <summary>
    /// Loads balance components used for verification and operational checks.
    /// </summary>
    private async Task<BalanceComponents> GetBalanceComponentsAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var earnings = await _earningsService.GetUserEarningsAsync(userId, cancellationToken);
        var pendingWithdrawals = await _withdrawalRepository.GetTotalAmountByUserIdAndStatusesAsync(
            userId,
            [WithdrawalStatus.Pending],
            cancellationToken);
        var approvedWithdrawals = await _withdrawalRepository.GetTotalAmountByUserIdAndStatusesAsync(
            userId,
            [WithdrawalStatus.Approved],
            cancellationToken);
        var completedWithdrawals = await _withdrawalRepository.GetTotalCompletedAmountByUserIdAsync(
            userId,
            cancellationToken);

        var verifiedAvailableBalance = earnings.ApprovedCashback
            - pendingWithdrawals
            - completedWithdrawals;

        var operationalAvailableBalance = verifiedAvailableBalance - approvedWithdrawals;

        return new BalanceComponents(
            earnings.ApprovedCashback,
            pendingWithdrawals,
            approvedWithdrawals,
            completedWithdrawals,
            verifiedAvailableBalance,
            operationalAvailableBalance);
    }

    /// <summary>
    /// Internal balance component snapshot.
    /// </summary>
    private sealed record BalanceComponents(
        decimal ApprovedCashback,
        decimal PendingWithdrawals,
        decimal ApprovedWithdrawals,
        decimal CompletedWithdrawals,
        decimal VerifiedAvailableBalance,
        decimal OperationalAvailableBalance);
}
