using Cashback.Application.Features.Admin.Users.Common;
using MediatR;

namespace Cashback.Application.Features.Admin.Users.ActivateUser;

/// <summary>
/// Command to activate a suspended user account.
/// </summary>
public sealed record ActivateUserCommand(Guid UserId) : IRequest<AdminUserActionResponse>;
