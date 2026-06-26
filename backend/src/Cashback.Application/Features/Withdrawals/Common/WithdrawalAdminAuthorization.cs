using Cashback.Application.Interfaces;
using Cashback.Domain.Enums;
using Cashback.Domain.Exceptions;

namespace Cashback.Application.Features.Withdrawals.Common;

/// <summary>
/// Shared helpers for admin withdrawal command handlers.
/// </summary>
internal static class WithdrawalAdminAuthorization
{
    /// <summary>
    /// Ensures the current user is an authenticated administrator.
    /// </summary>
    public static Guid RequireAdminUserId(ICurrentUserService currentUserService)
    {
        if (!currentUserService.IsAuthenticated || currentUserService.UserId is null)
        {
            throw new UnauthorizedException("Authentication is required.");
        }

        if (currentUserService.Role != UserRole.Admin)
        {
            throw new ForbiddenException("Administrator access is required.");
        }

        return currentUserService.UserId.Value;
    }
}
