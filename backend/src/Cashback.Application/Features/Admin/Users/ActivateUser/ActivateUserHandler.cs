using Cashback.Application.Features.Admin.Users.Common;
using Cashback.Application.Interfaces;
using Cashback.Domain.Enums;
using Cashback.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Cashback.Application.Features.Admin.Users.ActivateUser;

/// <summary>
/// Handles activation of a suspended user account by an administrator.
/// </summary>
public sealed class ActivateUserHandler : IRequestHandler<ActivateUserCommand, AdminUserActionResponse>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<ActivateUserHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the activate user handler.
    /// </summary>
    public ActivateUserHandler(
        ICurrentUserService currentUserService,
        IUserRepository userRepository,
        IAuditLogService auditLogService,
        ILogger<ActivateUserHandler> logger)
    {
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _auditLogService = auditLogService;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<AdminUserActionResponse> Handle(
        ActivateUserCommand request,
        CancellationToken cancellationToken)
    {
        var adminUserId = AdminAuthorization.RequireAdminUserId(_currentUserService);

        var user = await _userRepository.GetByIdForUpdateAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            throw new NotFoundException("User not found.");
        }

        if (user.Status == UserStatus.Deleted)
        {
            throw new BusinessRuleException("Deleted users cannot be activated.");
        }

        if (user.Status == UserStatus.Active)
        {
            throw new BusinessRuleException("User is already active.");
        }

        var previousStatus = user.Status;
        user.Activate();

        await _userRepository.UpdateAsync(user, cancellationToken);

        await _auditLogService.LogUserStatusChangeAsync(
            adminUserId,
            user.Id,
            AuditAction.UserUpdated,
            previousStatus,
            user.Status,
            cancellationToken);

        _logger.LogInformation(
            "User {UserId} activated by admin {AdminUserId}",
            user.Id,
            adminUserId);

        return AdminUserMapper.ToActionResponse(user);
    }
}
