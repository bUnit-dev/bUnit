using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Bunit.TestDoubles
{
	internal sealed class PlaceholderAuthorizationService : IAuthorizationService
	{
		public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, IEnumerable<IAuthorizationRequirement> requirements)
		{
			throw new MissingFakeAuthorizationException(nameof(IAuthorizationService));
		}

		public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, string policyName)
		{
			throw new MissingFakeAuthorizationException(nameof(IAuthorizationService));
		}
	}
}
