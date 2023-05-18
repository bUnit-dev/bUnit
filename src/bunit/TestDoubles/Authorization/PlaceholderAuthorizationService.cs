using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Bunit.TestDoubles;

internal sealed class PlaceholderAuthorizationService : IAuthorizationService
{
	public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, IEnumerable<IAuthorizationRequirement> requirements)
	{
		throw new MissingAuthorizationHandlerException(nameof(IAuthorizationService));
	}

	public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, string policyName)
	{
		throw new MissingAuthorizationHandlerException(nameof(IAuthorizationService));
	}
}
