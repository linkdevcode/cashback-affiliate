using Cashback.Application.Features.Admin.Users.Common;
using Cashback.Application.Interfaces;
using Cashback.Domain.Enums;
using Cashback.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Cashback.Application.Features.Admin.Users.SuspendUser;

/// <summary>
/// Handles suspension of a user account by an administrator.
/// </summary>
public sealed class SuspendUserHandler : IRequestHandler<SuspendUserCommand, AdminUserActionResponse>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<SuspendUserHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the suspend user handler.
    /// </summary>
    public SuspendUserHandler(
        ICurrentUserService currentUserService,
        IUserRepository userRepository,
        IAuditLogService auditLogService,
        ILogger<SuspendUserHandler> logger)
    {
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _auditLogService = auditLogService;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<AdminUserActionResponse> Handle(
        SuspendUserCommand request,
        CancellationToken cancellationToken)
    {
        var adminUserId = AdminAuthorization.RequireAdminUserId(_currentUserService);

        if (request.UserId == adminUserId)
        {
            throw new BusinessRuleException("Administrators cannot suspend their own account.");
        }

        var user = await _userRepository.GetByIdForUpdateAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            throw new NotFoundException("User not found.");
        }

        if (user.Status == UserStatus.Deleted)
        {
            throw new BusinessRuleException("Deleted users cannot be suspended.");
        }

        if (user.Role == UserRole.Admin)
        {
            throw new BusinessRuleException("Administrator accounts cannot be suspended.");
        }

        if (user.Status == UserStatus.Suspended)
        {
            throw new BusinessRuleException("User is already suspended.");
        }

        var previousStatus = user.Status;
        user.Suspend();

        await _userRepository.UpdateAsync(user, cancellationToken);

        await _auditLogService.LogUserStatusChangeAsync(
            adminUserId,
            user.Id,
            AuditAction.UserDisabled,
            previousStatus,
            user.Status,
            cancellationToken);

        _logger.LogInformation(
            "User {UserId} suspended by admin {AdminUserId}",
            user.Id,
            adminUserId);

        return AdminUserMapper.ToActionResponse(user);
    }
}
