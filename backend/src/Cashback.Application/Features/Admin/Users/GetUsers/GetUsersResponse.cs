using Cashback.Application.Features.Admin.Users.Common;

namespace Cashback.Application.Features.Admin.Users.GetUsers;

/// <summary>
/// Paginated admin user list response.
/// </summary>
public sealed class GetUsersResponse
{
    /// <summary>
    /// Users for the current page.
    /// </summary>
    public IReadOnlyList<AdminUserDto> Items { get; init; } = [];

    /// <summary>
    /// Total number of users matching the query.
    /// </summary>
    public int TotalCount { get; init; }

    /// <summary>
    /// Current page number.
    /// </summary>
    public int Page { get; init; }

    /// <summary>
    /// Number of items per page.
    /// </summary>
    public int PageSize { get; init; }
}
