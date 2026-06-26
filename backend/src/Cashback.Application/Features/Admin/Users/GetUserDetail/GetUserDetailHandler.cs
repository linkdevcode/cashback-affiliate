using Cashback.Application.Features.Admin.Users.Common;
using Cashback.Application.Interfaces;
using Cashback.Domain.Exceptions;
using MediatR;

namespace Cashback.Application.Features.Admin.Users.GetUserDetail;

/// <summary>
/// Handles retrieval of detailed user information for admin management.
/// </summary>
public sealed class GetUserDetailHandler : IRequestHandler<GetUserDetailQuery, GetUserDetailResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IWithdrawalRepository _withdrawalRepository;

    /// <summary>
    /// Initializes a new instance of the get user detail handler.
    /// </summary>
    public GetUserDetailHandler(
        IUserRepository userRepository,
        IOrderRepository orderRepository,
        IWithdrawalRepository withdrawalRepository)
    {
        _userRepository = userRepository;
        _orderRepository = orderRepository;
        _withdrawalRepository = withdrawalRepository;
    }

    /// <inheritdoc/>
    public async Task<GetUserDetailResponse> Handle(
        GetUserDetailQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            throw new NotFoundException("User not found.");
        }

        var orderSummaryTask = _orderRepository.GetUserSummaryAsync(request.UserId, cancellationToken);
        var withdrawalSummaryTask = _withdrawalRepository.GetUserSummaryAsync(request.UserId, cancellationToken);

        await Task.WhenAll(orderSummaryTask, withdrawalSummaryTask);

        return new GetUserDetailResponse
        {
            Profile = AdminUserMapper.ToProfile(user),
            OrderSummary = AdminUserMapper.ToOrderSummary(await orderSummaryTask),
            WithdrawalSummary = AdminUserMapper.ToWithdrawalSummary(await withdrawalSummaryTask)
        };
    }
}
