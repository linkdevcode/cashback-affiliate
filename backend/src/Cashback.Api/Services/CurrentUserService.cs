using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Cashback.Application.Interfaces;
using Cashback.Domain.Enums;

namespace Cashback.Api.Services;

/// <summary>
/// Extracts the current user identity from the HTTP context JWT claims.
/// </summary>
public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the current user service.
    /// </summary>
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc/>
    public Guid? UserId
    {
        get
        {
            var userIdValue = _httpContextAccessor.HttpContext?.User.FindFirstValue(
                JwtRegisteredClaimNames.Sub)
                ?? _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.TryParse(userIdValue, out var userId) ? userId : null;
        }
    }

    /// <inheritdoc/>
    public string? Email =>
        _httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Email)
        ?? _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

    /// <inheritdoc/>
    public UserRole? Role
    {
        get
        {
            var roleValue = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);

            return Enum.TryParse<UserRole>(roleValue, out var role) ? role : null;
        }
    }

    /// <inheritdoc/>
    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}
