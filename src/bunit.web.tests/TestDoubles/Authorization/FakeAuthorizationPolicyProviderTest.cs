using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Xunit;

namespace Bunit.TestDoubles.Authorization
{
	public class FakeAuthorizationPolicyProviderTest
	{
		[Fact(DisplayName = "Get default policy from PolicyProvider.")]
		public async Task Test001()
		{
			// arrange
			var provider = new FakeAuthorizationPolicyProvider();

			// act
			var policy = await provider.GetDefaultPolicyAsync();

			// assert
			Assert.NotNull(policy);
			Assert.Equal(0, policy.AuthenticationSchemes.Count);
			Assert.Equal(1, policy.Requirements.Count);
			Assert.IsType<DenyAnonymousAuthorizationRequirement>(policy.Requirements[0]);
		}

		[Fact(DisplayName = "Get fallback policy from PolicyProvider.")]
		public async Task Test002()
		{
			// arrange
			var provider = new FakeAuthorizationPolicyProvider();

			// act
			var policy = await provider.GetFallbackPolicyAsync().ConfigureAwait(false);

			// assert
			Assert.Null(policy);
		}

		[Fact(DisplayName = "Get policy based on name from PolicyProvider.")]
		public async Task Test003()
		{
			// arrange
			var provider = new FakeAuthorizationPolicyProvider();

			// act
			var policy = await provider.GetPolicyAsync("FooBar").ConfigureAwait(false);

			// assert
			Assert.NotNull(policy);
			Assert.Equal(1, policy.AuthenticationSchemes.Count);
			Assert.Equal("TestScheme:FooBar", policy.AuthenticationSchemes[0]);
			Assert.Equal(1, policy.Requirements.Count);
			Assert.IsType<TestPolicyRequirement>(policy.Requirements[0]);
		}
	}
}
