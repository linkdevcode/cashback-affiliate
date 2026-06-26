using System.Security.Claims;
using Cashback.Api.Constants;
using Cashback.Api.Models;
using Cashback.Domain.Enums;

namespace Cashback.Api.Middleware;

/// <summary>
/// Middleware that enforces administrator access for admin API routes.
/// </summary>
public sealed class AdminOnlyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AdminOnlyMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the admin-only middleware.
    /// </summary>
    public AdminOnlyMiddleware(RequestDelegate next, ILogger<AdminOnlyMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokes the middleware pipeline.
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        if (!IsAdminRoute(context.Request.Path))
        {
            await _next(context);
            return;
        }

        if (context.User.Identity?.IsAuthenticated != true)
        {
            _logger.LogWarning(
                "Unauthenticated request blocked for admin route {Path}",
                context.Request.Path);
            await WriteResponseAsync(
                context,
                StatusCodes.Status401Unauthorized,
                "Authentication is required.");
            return;
        }

        if (!HasAdminRole(context.User))
        {
            _logger.LogWarning(
                "Non-admin request blocked for admin route {Path} by user {UserId}",
                context.Request.Path,
                context.User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? context.User.FindFirstValue(ClaimTypes.Name));
            await WriteResponseAsync(
                context,
                StatusCodes.Status403Forbidden,
                "Administrator access is required.");
            return;
        }

        await _next(context);
    }

    /// <summary>
    /// Determines whether the request targets an admin API route.
    /// </summary>
    private static bool IsAdminRoute(PathString path)
    {
        return path.StartsWithSegments(
            AdminRouteConstants.PathPrefix,
            StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Validates that the current principal has the administrator role.
    /// </summary>
    private static bool HasAdminRole(ClaimsPrincipal user)
    {
        return user.IsInRole(nameof(UserRole.Admin));
    }

    /// <summary>
    /// Writes a standardized API error response.
    /// </summary>
    private static async Task WriteResponseAsync(
        HttpContext context,
        int statusCode,
        string message)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = ApiResponse<object>.Fail(message);
        await context.Response.WriteAsJsonAsync(response);
    }
}
