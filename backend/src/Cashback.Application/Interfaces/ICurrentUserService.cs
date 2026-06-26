using Cashback.Domain.Enums;

namespace Cashback.Application.Interfaces;

/// <summary>
/// Provides access to the currently authenticated user from the request context.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Authenticated user identifier from the JWT.
    /// </summary>
    Guid? UserId { get; }

    /// <summary>
    /// Authenticated user email from the JWT.
    /// </summary>
    string? Email { get; }

    /// <summary>
    /// Authenticated user role from the JWT.
    /// </summary>
    UserRole? Role { get; }

    /// <summary>
    /// Indicates whether the current request is authenticated.
    /// </summary>
    bool IsAuthenticated { get; }
}
