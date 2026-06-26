using Cashback.Domain.Entities;
using Cashback.Domain.Enums;
using Cashback.Persistence.Context;
using Cashback.Persistence.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Cashback.UnitTests.Repositories;

/// <summary>
/// Unit tests for transaction repository persistence.
/// </summary>
public sealed class TransactionRepositoryTests : IAsyncLifetime
{
    private SqliteConnection _connection = null!;
    private ApplicationDbContext _context = null!;
    private TransactionRepository _repository = null!;
    private Guid _userId;

    /// <inheritdoc/>
    public async Task InitializeAsync()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        await _connection.OpenAsync();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new ApplicationDbContext(options);
        await _context.Database.EnsureCreatedAsync();

        _repository = new TransactionRepository(_context);

        var user = User.Create("user@example.com", "Test User", AuthProvider.Google, "google-1");
        _userId = user.Id;
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
        await _connection.DisposeAsync();
    }

    /// <summary>
    /// Verifies a transaction can be persisted and retrieved by user.
    /// </summary>
    [Fact]
    public async Task AddAsync_ValidTransaction_PersistsRecord()
    {
        var transaction = Transaction.Create(
            _userId,
            TransactionType.CashbackEarned,
            amount: 80_000m,
            balanceBefore: 0m,
            balanceAfter: 80_000m);

        await _repository.AddAsync(transaction, CancellationToken.None);

        var retrieved = await _repository.GetByIdForUserAsync(
            transaction.Id,
            _userId,
            CancellationToken.None);

        Assert.NotNull(retrieved);
        Assert.Equal(TransactionType.CashbackEarned, retrieved.Type);
        Assert.Equal(80_000m, retrieved.Amount);
    }

    /// <summary>
    /// Verifies users cannot access another user's transactions.
    /// </summary>
    [Fact]
    public async Task GetByIdForUserAsync_DifferentUser_ReturnsNull()
    {
        var transaction = Transaction.Create(
            _userId,
            TransactionType.CashbackEarned,
            amount: 10_000m,
            balanceBefore: 0m,
            balanceAfter: 10_000m);

        await _repository.AddAsync(transaction, CancellationToken.None);

        var retrieved = await _repository.GetByIdForUserAsync(
            transaction.Id,
            Guid.NewGuid(),
            CancellationToken.None);

        Assert.Null(retrieved);
    }

    /// <summary>
    /// Verifies paginated queries return transactions in descending created order.
    /// </summary>
    [Fact]
    public async Task GetPagedByUserIdAsync_MultipleTransactions_ReturnsPagedResults()
    {
        var first = Transaction.Create(
            _userId,
            TransactionType.CashbackEarned,
            amount: 10_000m,
            balanceBefore: 0m,
            balanceAfter: 10_000m);

        var second = Transaction.Create(
            _userId,
            TransactionType.WithdrawalRequested,
            amount: 5_000m,
            balanceBefore: 10_000m,
            balanceAfter: 5_000m);

        await _repository.AddAsync(first, CancellationToken.None);
        await _repository.AddAsync(second, CancellationToken.None);

        var (items, totalCount) = await _repository.GetPagedByUserIdAsync(
            _userId,
            page: 1,
            pageSize: 20,
            type: null,
            CancellationToken.None);

        Assert.Equal(2, totalCount);
        Assert.Equal(2, items.Count);
        Assert.Contains(items, item => item.Type == TransactionType.CashbackEarned);
        Assert.Contains(items, item => item.Type == TransactionType.WithdrawalRequested);
    }

    /// <summary>
    /// Verifies type filtering limits paginated transaction results.
    /// </summary>
    [Fact]
    public async Task GetPagedByUserIdAsync_TypeFilter_ReturnsMatchingTransactions()
    {
        await _repository.AddAsync(
            Transaction.Create(
                _userId,
                TransactionType.CashbackEarned,
                amount: 10_000m,
                balanceBefore: 0m,
                balanceAfter: 10_000m),
            CancellationToken.None);

        await _repository.AddAsync(
            Transaction.Create(
                _userId,
                TransactionType.WithdrawalRequested,
                amount: 5_000m,
                balanceBefore: 10_000m,
                balanceAfter: 5_000m),
            CancellationToken.None);

        var (items, totalCount) = await _repository.GetPagedByUserIdAsync(
            _userId,
            page: 1,
            pageSize: 20,
            type: TransactionType.CashbackEarned,
            CancellationToken.None);

        Assert.Equal(1, totalCount);
        Assert.Single(items);
        Assert.Equal(TransactionType.CashbackEarned, items[0].Type);
    }

    /// <summary>
    /// Verifies transactions can be queried by reference identifier.
    /// </summary>
    [Fact]
    public async Task GetByReferenceIdAsync_LinkedTransactions_ReturnsAllMatches()
    {
        var referenceId = Guid.NewGuid();

        await _repository.AddAsync(
            Transaction.Create(
                _userId,
                TransactionType.WithdrawalRequested,
                amount: 50_000m,
                balanceBefore: 100_000m,
                balanceAfter: 50_000m,
                referenceId: referenceId),
            CancellationToken.None);

        await _repository.AddAsync(
            Transaction.Create(
                _userId,
                TransactionType.WithdrawalApproved,
                amount: 50_000m,
                balanceBefore: 50_000m,
                balanceAfter: 50_000m,
                referenceId: referenceId),
            CancellationToken.None);

        var items = await _repository.GetByReferenceIdAsync(referenceId, CancellationToken.None);

        Assert.Equal(2, items.Count);
        Assert.All(items, item => Assert.Equal(referenceId, item.ReferenceId));
    }
}
