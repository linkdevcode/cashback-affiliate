using System.Net;
using System.Net.Http.Headers;
using Cashback.Application.Interfaces;
using Cashback.Domain.Enums;
using Cashback.IntegrationTests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cashback.IntegrationTests.Admin;

/// <summary>
/// Integration tests for admin authorization policies and route protection.
/// </summary>
public sealed class AdminAuthorizationTests : IClassFixture<CashbackWebApplicationFactory>, IAsyncLifetime
{
    private const string AdminWithdrawalApprovePathTemplate = "/api/v1/admin/withdrawals/{0}/approve";

    private readonly CashbackWebApplicationFactory _factory;
    private readonly HttpClient _client;

    /// <summary>
    /// Initializes a new instance of the admin authorization integration tests.
    /// </summary>
    public AdminAuthorizationTests(CashbackWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    /// <inheritdoc/>
    public async Task InitializeAsync()
    {
        await _factory.InitializeDatabaseAsync();
    }

    /// <inheritdoc/>
    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Verifies unauthenticated requests to admin routes are rejected.
    /// </summary>
    [Fact]
    public async Task AdminRoute_WithoutToken_ReturnsUnauthorized()
    {
        var response = await _client.PostAsync(
            string.Format(AdminWithdrawalApprovePathTemplate, Guid.NewGuid()),
            null);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    /// <summary>
    /// Verifies authenticated non-admin users cannot access admin routes.
    /// </summary>
    [Fact]
    public async Task AdminRoute_WithUserToken_ReturnsForbidden()
    {
        using var scope = _factory.Services.CreateScope();
        var tokenService = scope.ServiceProvider.GetRequiredService<IJwtTokenService>();
        var token = TestJwtTokenFactory.CreateAccessToken(tokenService, UserRole.User);

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            string.Format(AdminWithdrawalApprovePathTemplate, Guid.NewGuid()));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    /// <summary>
    /// Verifies authenticated administrators can access admin routes.
    /// </summary>
    [Fact]
    public async Task AdminRoute_WithAdminToken_AllowsAccess()
    {
        using var scope = _factory.Services.CreateScope();
        var tokenService = scope.ServiceProvider.GetRequiredService<IJwtTokenService>();
        var token = TestJwtTokenFactory.CreateAccessToken(tokenService, UserRole.Admin);

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            string.Format(AdminWithdrawalApprovePathTemplate, Guid.NewGuid()));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);

        Assert.NotEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.NotEqual(HttpStatusCode.Forbidden, response.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    /// <summary>
    /// Verifies non-admin routes remain accessible to standard users.
    /// </summary>
    [Fact]
    public async Task UserRoute_WithUserToken_DoesNotRequireAdminRole()
    {
        using var scope = _factory.Services.CreateScope();
        var tokenService = scope.ServiceProvider.GetRequiredService<IJwtTokenService>();
        var token = TestJwtTokenFactory.CreateAccessToken(tokenService, UserRole.User);

        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/dashboard");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);

        Assert.NotEqual(HttpStatusCode.Forbidden, response.StatusCode);
        Assert.NotEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
