using Cashback.Domain.Enums;
using MediatR;

namespace Cashback.Application.Features.Users.GetCurrentUser;

/// <summary>
/// Query to retrieve the authenticated user's profile.
/// </summary>
public sealed record GetCurrentUserQuery : IRequest<CurrentUserResponse>;
