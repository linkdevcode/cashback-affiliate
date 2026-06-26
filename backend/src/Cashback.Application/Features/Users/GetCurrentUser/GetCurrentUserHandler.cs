using Cashback.Application.Interfaces;
using Cashback.Domain.Enums;
using Cashback.Domain.Exceptions;
using MediatR;

namespace Cashback.Application.Features.Users.GetCurrentUser;

/// <summary>
/// Handles retrieval of the authenticated user's profile.
/// </summary>
public sealed class GetCurrentUserHandler : IRequestHandler<GetCurrentUserQuery, CurrentUserResponse>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes a new instance of the get current user handler.
    /// </summary>
    public GetCurrentUserHandler(
        ICurrentUserService currentUserService,
        IUserRepository userRepository)
    {
        _currentUserService = currentUserService;
        _userRepository = userRepository;
    }

    /// <inheritdoc/>
    public async Task<CurrentUserResponse> Handle(
        GetCurrentUserQuery request,
        CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated || _currentUserService.UserId is null)
        {
            throw new UnauthorizedException("Authentication is required.");
        }

        var user = await _userRepository.GetByIdAsync(
            _currentUserService.UserId.Value,
            cancellationToken);

        if (user is null)
        {
            throw new NotFoundException("User not found.");
        }

        if (user.Status == UserStatus.Suspended)
        {
            throw new ForbiddenException("Your account has been suspended.");
        }

        return new CurrentUserResponse
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            AvatarUrl = user.AvatarUrl,
            Role = user.Role,
            Status = user.Status
        };
    }
}
