using Cashback.Application.Features.Admin.Users.Common;
using Cashback.Application.Interfaces;
using MediatR;

namespace Cashback.Application.Features.Admin.Users.GetUsers;

/// <summary>
/// Handles retrieval of paginated users for admin management.
/// </summary>
public sealed class GetUsersHandler : IRequestHandler<GetUsersQuery, GetUsersResponse>
{
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes a new instance of the get users handler.
    /// </summary>
    public GetUsersHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <inheritdoc/>
    public async Task<GetUsersResponse> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _userRepository.GetPagedForAdminAsync(
            request.Page,
            request.PageSize,
            request.Email,
            request.Name,
            request.Status,
            cancellationToken);

        return new GetUsersResponse
        {
            Items = items.Select(AdminUserMapper.ToListItem).ToList(),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
