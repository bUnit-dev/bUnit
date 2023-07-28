using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Bunit.TestDoubles.Authorization;

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
		Assert.Empty(policy.AuthenticationSchemes);
		Assert.Single(policy.Requirements);
		Assert.IsType<DenyAnonymousAuthorizationRequirement>(policy.Requirements[0]);
	}

	[Fact(DisplayName = "Get fallback policy from PolicyProvider.")]
	public async Task Test002()
	{
		// arrange
		var provider = new FakeAuthorizationPolicyProvider();

		// act
		var policy = await provider.GetFallbackPolicyAsync();

		// assert
		Assert.Null(policy);
	}

	[Fact(DisplayName = "Get policy based on name from PolicyProvider.")]
	public async Task Test003()
	{
		// arrange
		var provider = new FakeAuthorizationPolicyProvider();
		provider.SetPolicyScheme("TestScheme");

		// act
		var policy = await provider.GetPolicyAsync("FooBar");

		// assert
		Assert.NotNull(policy);
		Assert.Equal(1, policy?.AuthenticationSchemes?.Count);
		Assert.Equal("TestScheme:FooBar", policy?.AuthenticationSchemes?[0]);
		Assert.Equal(1, policy?.Requirements?.Count);
		Assert.IsType<TestPolicyRequirement>(policy?.Requirements?[0]);
	}

	[Fact(DisplayName = "Get policy based on name not in the PolicyProvider.")]
	public async Task Test004()
	{
		// arrange
		var provider = new FakeAuthorizationPolicyProvider();
		provider.SetPolicyScheme("TestScheme");

		// act
		var policy = await provider.GetPolicyAsync("OtherPolicy");

		// assert
		Assert.NotNull(policy);
		Assert.Equal(1, policy?.AuthenticationSchemes?.Count);
		Assert.Equal("TestScheme:OtherPolicy", policy?.AuthenticationSchemes?[0]);
		Assert.Equal(1, policy?.Requirements?.Count);
		Assert.IsType<TestPolicyRequirement>(policy?.Requirements?[0]);
	}

	[Fact(DisplayName = "Set Policies with empty scheme name.")]
	public void Test006()
	{
		// arrange
		var provider = new FakeAuthorizationPolicyProvider();

		// assert
		Assert.Throws<ArgumentNullException>(
			() => provider.SetPolicyScheme(string.Empty));
	}
}
