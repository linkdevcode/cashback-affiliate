using Cashback.Api.Models;
using Cashback.Api.Models.Auth;
using Cashback.Application.Features.Auth.LoginWithGoogle;
using Cashback.Application.Features.Auth.RefreshToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashback.Api.Controllers;

/// <summary>
/// Authentication endpoints.
/// </summary>
[ApiController]
[Route("api/v1/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the auth controller.
    /// </summary>
    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Authenticates a user using a Google OAuth ID token.
    /// </summary>
    [HttpPost("google")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<LoginWithGoogleResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginWithGoogle(
        [FromBody] GoogleLoginRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new LoginWithGoogleCommand(request.IdToken),
            cancellationToken);

        return Ok(ApiResponse<LoginWithGoogleResponse>.Ok(result));
    }

    /// <summary>
    /// Renews an access token using a refresh token.
    /// </summary>
    [HttpPost("refresh-token")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<RefreshTokenResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshToken(
        [FromBody] RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new RefreshTokenCommand(request.RefreshToken),
            cancellationToken);

        return Ok(ApiResponse<RefreshTokenResponse>.Ok(result));
    }
}
