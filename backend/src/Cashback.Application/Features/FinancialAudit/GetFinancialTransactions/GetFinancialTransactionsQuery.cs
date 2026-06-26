using Cashback.Application.Features.FinancialAudit.Common;
using Cashback.Domain.Enums;
using MediatR;

namespace Cashback.Application.Features.FinancialAudit.GetFinancialTransactions;

/// <summary>
/// Query to retrieve paginated financial transactions for the authenticated user.
/// </summary>
public sealed record GetFinancialTransactionsQuery(
    int Page = 1,
    int PageSize = 20,
    TransactionType? Type = null)
    : IRequest<GetFinancialTransactionsResponse>;
