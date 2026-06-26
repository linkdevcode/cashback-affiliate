namespace Cashback.Application.Interfaces;

/// <summary>
/// Aggregated user statistics for the admin dashboard.
/// </summary>
public sealed record AdminUserStatistics(
    int TotalUsers,
    int ActiveUsers,
    int SuspendedUsers);
