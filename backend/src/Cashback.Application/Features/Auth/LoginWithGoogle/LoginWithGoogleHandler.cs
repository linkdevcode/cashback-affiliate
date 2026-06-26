using Cashback.Application.Interfaces;
using Cashback.Domain.Entities;
using Cashback.Domain.Enums;
using Cashback.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Cashback.Application.Features.Auth.LoginWithGoogle;

/// <summary>
/// Handles Google OAuth login, user registration, and token issuance.
/// </summary>
public sealed class LoginWithGoogleHandler : IRequestHandler<LoginWithGoogleCommand, LoginWithGoogleResponse>
{
    private readonly IGoogleAuthService _googleAuthService;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger<LoginWithGoogleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the Google login handler.
    /// </summary>
    public LoginWithGoogleHandler(
        IGoogleAuthService googleAuthService,
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IJwtTokenService jwtTokenService,
        ILogger<LoginWithGoogleHandler> logger)
    {
        _googleAuthService = googleAuthService;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtTokenService = jwtTokenService;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<LoginWithGoogleResponse> Handle(
        LoginWithGoogleCommand request,
        CancellationToken cancellationToken)
    {
        var googleUser = await _googleAuthService.ValidateIdTokenAsync(
            request.IdToken,
            cancellationToken);

        if (googleUser is null)
        {
            throw new UnauthorizedException("Invalid Google token.");
        }

        var user = await FindOrCreateUserAsync(googleUser, cancellationToken);

        if (!user.CanLogin())
        {
            throw new ForbiddenException("Your account has been suspended.");
        }

        user.SyncGoogleProfile(googleUser.FullName, googleUser.AvatarUrl);
        await _userRepository.UpdateAsync(user, cancellationToken);

        var accessToken = _jwtTokenService.GenerateAccessToken(user);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();
        var refreshTokenHash = _jwtTokenService.HashRefreshToken(refreshToken);

        await _refreshTokenRepository.RevokeAllForUserAsync(user.Id, cancellationToken);

        var refreshTokenEntity = UserRefreshToken.Create(
            user.Id,
            refreshTokenHash,
            _jwtTokenService.GetRefreshTokenExpiry());

        await _refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);

        _logger.LogInformation("User {UserId} logged in via Google", user.Id);

        return new LoginWithGoogleResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            User = new UserInfoDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role
            }
        };
    }

    /// <summary>
    /// Finds an existing user or registers a new Google account.
    /// </summary>
    private async Task<User> FindOrCreateUserAsync(
        GoogleUserInfo googleUser,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByProviderUserIdAsync(
            googleUser.ProviderUserId,
            cancellationToken);

        if (user is not null)
        {
            return user;
        }

        user = await _userRepository.GetByEmailAsync(googleUser.Email, cancellationToken);

        if (user is not null)
        {
            return user;
        }

        var newUser = User.Create(
            googleUser.Email,
            googleUser.FullName,
            AuthProvider.Google,
            googleUser.ProviderUserId,
            googleUser.AvatarUrl);

        await _userRepository.AddAsync(newUser, cancellationToken);

        _logger.LogInformation("New user registered via Google: {Email}", googleUser.Email);

        return newUser;
    }
}
