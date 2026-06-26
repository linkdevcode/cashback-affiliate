using Cashback.Domain.Enums;
using MediatR;

namespace Cashback.Application.Features.Auth.LoginWithGoogle;

/// <summary>
/// Command to authenticate a user with a Google ID token.
/// </summary>
public sealed record LoginWithGoogleCommand(string IdToken) : IRequest<LoginWithGoogleResponse>;
