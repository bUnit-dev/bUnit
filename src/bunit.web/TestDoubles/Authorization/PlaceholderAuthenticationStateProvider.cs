using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace Bunit.TestDoubles.Authorization
{
	internal class PlaceholderAuthenticationStateProvider : AuthenticationStateProvider
	{
		public override Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			throw new MissingFakeAuthorizationException(nameof(AuthenticationStateProvider));
		}
	}
}
