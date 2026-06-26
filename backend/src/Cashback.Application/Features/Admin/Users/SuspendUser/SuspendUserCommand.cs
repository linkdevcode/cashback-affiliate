using Cashback.Application.Features.Admin.Users.Common;
using MediatR;

namespace Cashback.Application.Features.Admin.Users.SuspendUser;

/// <summary>
/// Command to suspend a user account.
/// </summary>
public sealed record SuspendUserCommand(Guid UserId) : IRequest<AdminUserActionResponse>;
