using MediatR;

namespace Cashback.Application.Features.Admin.Users.GetUserDetail;

/// <summary>
/// Query to retrieve detailed user information for admin management.
/// </summary>
public sealed record GetUserDetailQuery(Guid UserId) : IRequest<GetUserDetailResponse>;
