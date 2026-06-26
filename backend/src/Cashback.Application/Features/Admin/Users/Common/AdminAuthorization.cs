using Cashback.Application.Interfaces;
using Cashback.Domain.Enums;
using Cashback.Domain.Exceptions;

namespace Cashback.Application.Features.Admin.Users.Common;

/// <summary>
/// Shared helpers for admin user management handlers.
/// </summary>
internal static class AdminAuthorization
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
