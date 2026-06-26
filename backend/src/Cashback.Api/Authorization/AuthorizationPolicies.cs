namespace Cashback.Api.Authorization;

/// <summary>
/// Named authorization policy identifiers.
/// </summary>
public static class AuthorizationPolicies
{
    /// <summary>
    /// Policy requiring an authenticated administrator.
    /// </summary>
    public const string AdminOnly = "AdminOnly";
}
