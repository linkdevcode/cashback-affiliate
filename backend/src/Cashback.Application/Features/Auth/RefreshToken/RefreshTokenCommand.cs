using MediatR;

namespace Cashback.Application.Features.Auth.RefreshToken;

/// <summary>
/// Command to renew an access token using a refresh token.
/// </summary>
public sealed record RefreshTokenCommand(string RefreshToken) : IRequest<RefreshTokenResponse>;
