using Cashback.Application.Features.Admin.Users.Common;
using Cashback.Domain.Enums;
using MediatR;

namespace Cashback.Application.Features.Admin.Users.GetUsers;

/// <summary>
/// Query to retrieve paginated users for admin management.
/// </summary>
public sealed record GetUsersQuery(
    int Page = 1,
    int PageSize = 20,
    string? Email = null,
    string? Name = null,
    UserStatus? Status = null)
    : IRequest<GetUsersResponse>;