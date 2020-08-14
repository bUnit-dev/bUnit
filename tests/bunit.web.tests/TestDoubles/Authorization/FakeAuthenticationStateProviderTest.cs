using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Bunit.TestDoubles.Authorization
{
	public class FakeAuthenticationStateProviderTest
	{
		[Fact(DisplayName = "Create authenticated AuthenticationState")]
		public async Task Test001()
		{
			// arrange
			var roles = new List<string> { "User" };

			// act
			var authProvider = new FakeAuthenticationStateProvider("TestUser", roles);
			var authState = await authProvider.GetAuthenticationStateAsync();

			// assert
			Assert.NotNull(authState.User);
			Assert.NotNull(authState.User.Identity);
			Assert.Equal("TestUser", authState?.User?.Identity?.Name);
			Assert.True(authState?.User?.Identity?.IsAuthenticated);
		}

		[Fact(DisplayName = "Create unauthenticated AuthenticationState")]
		public async Task Test002()
		{
			// act
			var authProvider = new FakeAuthenticationStateProvider();
			var authState = await authProvider.GetAuthenticationStateAsync();

			// assert
			Assert.NotNull(authState?.User);
			Assert.NotNull(authState?.User?.Identity);
			Assert.Null(authState?.User?.Identity?.Name);
			Assert.False(authState?.User?.Identity?.IsAuthenticated);
		}

		[Fact(DisplayName = "Switch AuthenticationState from unauthenticated to authenticated.")]
		public async Task Test003()
		{
			// arrange
			var authProvider = new FakeAuthenticationStateProvider();
			var stateChangeHandled = false;
			authProvider.AuthenticationStateChanged += e => stateChangeHandled = true;

			// act
			authProvider.TriggerAuthenticationStateChanged("NewUser");
			var authState = await authProvider.GetAuthenticationStateAsync();

			// assert
			Assert.True(stateChangeHandled);
			Assert.NotNull(authState.User);
			Assert.NotNull(authState.User.Identity);
			Assert.Equal("NewUser", authState?.User?.Identity?.Name);
			Assert.True(authState?.User?.Identity?.IsAuthenticated);
		}
	}
}
