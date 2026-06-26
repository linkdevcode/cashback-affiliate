using Cashback.Application.Interfaces;
using Cashback.Domain.Entities;
using Cashback.Domain.Enums;

namespace Cashback.IntegrationTests.Infrastructure;

/// <summary>
/// Helper for generating JWT access tokens in integration tests.
/// </summary>
internal static class TestJwtTokenFactory
{
    /// <summary>
    /// Creates a JWT access token for the specified user role.
    /// </summary>
    public static string CreateAccessToken(IJwtTokenService tokenService, UserRole role)
    {
        var user = User.Create(
            role == UserRole.Admin ? "admin@example.com" : "user@example.com",
            role == UserRole.Admin ? "Admin User" : "Standard User",
            AuthProvider.Google,
            $"google-{role.ToString().ToLowerInvariant()}",
            role: role);

        return tokenService.GenerateAccessToken(user);
    }
}
