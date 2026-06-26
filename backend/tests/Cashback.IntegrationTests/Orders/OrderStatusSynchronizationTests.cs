using Cashback.Application.Features.Orders.OrderApproval;
using Cashback.Application.Features.Orders.OrderRejection;
using Cashback.Application.Interfaces;
using Cashback.Application.Services;
using Cashback.Domain.Entities;
using Cashback.Domain.Enums;
using Cashback.Infrastructure.Settings;
using Cashback.Persistence.Context;
using Cashback.Persistence.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Cashback.IntegrationTests.Orders;

/// <summary>
/// Integration tests for order status synchronization from webhook events.
/// </summary>
public sealed class OrderStatusSynchronizationTests : IAsyncLifetime
{
    private SqliteConnection _connection = null!;
    private ApplicationDbContext _context = null!;
    private IUserRepository _userRepository = null!;
    private IOrderRepository _orderRepository = null!;
    private IAuditLogRepository _auditLogRepository = null!;
    private OrderApprovalHandler _approvalHandler = null!;
    private OrderRejectionHandler _rejectionHandler = null!;
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

        _userRepository = new UserRepository(_context);
        _orderRepository = new OrderRepository(_context);
        _auditLogRepository = new AuditLogRepository(_context);

        var cashbackSettings = new CashbackSettings(new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Cashback:CashbackPercentage"] = "80",
                ["Cashback:PlatformCommissionPercentage"] = "20"
            })
            .Build());

        var cashbackService = new CashbackService(cashbackSettings);

        var userResolver = new WebhookSub1UserResolver(
            _userRepository,
            new AffiliateLinkRepository(_context),
            NullLogger<WebhookSub1UserResolver>.Instance);

        var orderSynchronizationService = new OrderSynchronizationService(
            userResolver,
            _orderRepository,
            cashbackService,
            NullLogger<OrderSynchronizationService>.Instance);

        var auditLogService = new AuditLogService(_auditLogRepository);

        _approvalHandler = new OrderApprovalHandler(
            orderSynchronizationService,
            auditLogService,
            userResolver,
            NullLogger<OrderApprovalHandler>.Instance);

        _rejectionHandler = new OrderRejectionHandler(
            orderSynchronizationService,
            auditLogService,
            userResolver,
            NullLogger<OrderRejectionHandler>.Instance);

        var user = User.Create(
            "test@example.com",
            "Test User",
            AuthProvider.Google,
            "google-test-id");

        _userId = user.Id;
        await _userRepository.AddAsync(user, CancellationToken.None);
    }

    /// <inheritdoc/>
    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
        await _connection.DisposeAsync();
    }

    /// <summary>
    /// Verifies approval creates an order when one does not already exist.
    /// </summary>
    [Fact]
    public async Task OrderApprovalHandler_CreatesOrder_WhenOrderDoesNotExist()
    {
        var webhookEventId = Guid.NewGuid();
        var sub1 = $"USER_{_userId}";

        var result = await _approvalHandler.Handle(
            new OrderApprovalCommand("ORDER-100", sub1, 100_000m, webhookEventId),
            CancellationToken.None);

        Assert.True(result.WasCreated);
        Assert.False(result.WasUpdated);
        Assert.Equal(OrderStatus.Approved, result.Status);

        var order = await _orderRepository.GetByNetworkOrderIdAsync("ORDER-100", CancellationToken.None);
        Assert.NotNull(order);
        Assert.Equal(_userId, order.UserId);
        Assert.Equal(100_000m, order.CommissionAmount);
        Assert.Equal(80_000m, order.CashbackAmount);
        Assert.Equal(20_000m, order.PlatformProfit);
        Assert.Equal(OrderStatus.Approved, order.Status);

        var auditLogs = await _context.AuditLogs.ToListAsync();
        Assert.Single(auditLogs);
        Assert.Equal(AuditAction.OrderCreated, auditLogs[0].Action);
        Assert.Equal(_userId, auditLogs[0].UserId);
    }

    /// <summary>
    /// Verifies approval updates an existing pending order.
    /// </summary>
    [Fact]
    public async Task OrderApprovalHandler_UpdatesOrder_WhenOrderExists()
    {
        var webhookEventId = Guid.NewGuid();
        var sub1 = $"USER_{_userId}";
        var pendingOrder = Order.CreateFromWebhook(
            _userId,
            null,
            "ORDER-200",
            50_000m,
            40_000m,
            10_000m,
            OrderStatus.Pending);

        await _orderRepository.AddAsync(pendingOrder, CancellationToken.None);

        var result = await _approvalHandler.Handle(
            new OrderApprovalCommand("ORDER-200", sub1, 80_000m, webhookEventId),
            CancellationToken.None);

        Assert.True(result.WasUpdated);
        Assert.Equal(OrderStatus.Approved, result.Status);
        Assert.Equal(OrderStatus.Pending, result.PreviousStatus);

        var order = await _orderRepository.GetByNetworkOrderIdAsync("ORDER-200", CancellationToken.None);
        Assert.NotNull(order);
        Assert.Equal(80_000m, order.CommissionAmount);
        Assert.Equal(64_000m, order.CashbackAmount);
        Assert.Equal(16_000m, order.PlatformProfit);
        Assert.Equal(OrderStatus.Approved, order.Status);

        var auditLogs = await _context.AuditLogs.ToListAsync();
        Assert.Single(auditLogs);
        Assert.Equal(AuditAction.OrderUpdated, auditLogs[0].Action);
    }

    /// <summary>
    /// Verifies rejection creates an order when one does not already exist.
    /// </summary>
    [Fact]
    public async Task OrderRejectionHandler_CreatesOrder_WhenOrderDoesNotExist()
    {
        var webhookEventId = Guid.NewGuid();
        var sub1 = $"USER_{_userId}";

        var result = await _rejectionHandler.Handle(
            new OrderRejectionCommand("ORDER-300", sub1, 40_000m, webhookEventId),
            CancellationToken.None);

        Assert.True(result.WasCreated);
        Assert.Equal(OrderStatus.Rejected, result.Status);

        var order = await _orderRepository.GetByNetworkOrderIdAsync("ORDER-300", CancellationToken.None);
        Assert.NotNull(order);
        Assert.Equal(OrderStatus.Rejected, order.Status);
    }

    /// <summary>
    /// Verifies rejection updates an existing pending order.
    /// </summary>
    [Fact]
    public async Task OrderRejectionHandler_UpdatesPendingOrder_ToRejected()
    {
        var webhookEventId = Guid.NewGuid();
        var sub1 = $"USER_{_userId}";
        var pendingOrder = Order.CreateFromWebhook(
            _userId,
            null,
            "ORDER-400",
            60_000m,
            48_000m,
            12_000m,
            OrderStatus.Pending);

        await _orderRepository.AddAsync(pendingOrder, CancellationToken.None);

        var result = await _rejectionHandler.Handle(
            new OrderRejectionCommand("ORDER-400", sub1, 60_000m, webhookEventId),
            CancellationToken.None);

        Assert.True(result.WasUpdated);
        Assert.Equal(OrderStatus.Rejected, result.Status);

        var order = await _orderRepository.GetByNetworkOrderIdAsync("ORDER-400", CancellationToken.None);
        Assert.NotNull(order);
        Assert.Equal(OrderStatus.Rejected, order.Status);
    }
}
