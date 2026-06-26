using Cashback.Api.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashback.Api.Controllers;

/// <summary>
/// Base controller for administrator-only API endpoints.
/// </summary>
[ApiController]
[Authorize(Policy = AuthorizationPolicies.AdminOnly)]
public abstract class AdminControllerBase : ControllerBase
{
}
