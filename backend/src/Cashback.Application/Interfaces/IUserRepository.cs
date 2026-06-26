using Cashback.Domain.Entities;

namespace Cashback.Application.Interfaces;

/// <summary>
/// Persistence abstraction for user accounts.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Gets a user by identifier.
    /// </summary>
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a user by email address.
    /// </summary>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a user by external provider user identifier.
    /// </summary>
    Task<User?> GetByProviderUserIdAsync(string providerUserId, CancellationToken cancellationToken);

    /// <summary>
    /// Checks whether a user exists with the given email address.
    /// </summary>
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken);

    /// <summary>
    /// Persists a new user account.
    /// </summary>
    Task AddAsync(User user, CancellationToken cancellationToken);

    /// <summary>
    /// Persists changes to an existing user account.
    /// </summary>
    Task UpdateAsync(User user, CancellationToken cancellationToken);
}
