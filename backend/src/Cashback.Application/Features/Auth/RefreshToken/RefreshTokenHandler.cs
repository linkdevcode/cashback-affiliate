using Cashback.Application.Interfaces;
using Cashback.Domain.Exceptions;
using MediatR;

namespace Cashback.Application.Features.Auth.RefreshToken;

/// <summary>
/// Handles refresh token validation and access token renewal.
/// </summary>
public sealed class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;

    /// <summary>
    /// Initializes a new instance of the refresh token handler.
    /// </summary>
    public RefreshTokenHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository,
        IJwtTokenService jwtTokenService)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
    }

    /// <inheritdoc/>
    public async Task<RefreshTokenResponse> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var tokenHash = _jwtTokenService.HashRefreshToken(request.RefreshToken);

        var storedToken = await _refreshTokenRepository.GetByTokenHashAsync(
            tokenHash,
            cancellationToken);

        if (storedToken is null || !storedToken.IsActive())
        {
            throw new UnauthorizedException("Invalid or expired refresh token.");
        }

        var user = await _userRepository.GetByIdAsync(storedToken.UserId, cancellationToken);

        if (user is null || !user.CanLogin())
        {
            throw new UnauthorizedException("User account is not available.");
        }

        var accessToken = _jwtTokenService.GenerateAccessToken(user);

        return new RefreshTokenResponse
        {
            AccessToken = accessToken
        };
    }
}
