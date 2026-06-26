using Cashback.Application.Features.FinancialAudit.Common;
using MediatR;

namespace Cashback.Application.Features.FinancialAudit.GetBalanceAudit;

/// <summary>
/// Query to retrieve a balance audit report for the authenticated user.
/// </summary>
public sealed record GetBalanceAuditQuery : IRequest<BalanceAuditDto>;
