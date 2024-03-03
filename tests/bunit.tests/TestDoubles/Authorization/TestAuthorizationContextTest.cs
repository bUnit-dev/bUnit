namespace Bunit.TestDoubles.Authorization;

public class TestAuthorizationContextTest : TestContext
{
	[Fact(DisplayName = "Register Authorization services with unauthenticated user.")]
	public void Test003()
	{
		// act
		var authContext = this.AddAuthorization();

		// assert
		Assert.False(authContext.IsAuthenticated);
		Assert.Equal(AuthorizationState.Unauthorized, authContext.State);
		Assert.Empty(authContext.Roles);
		Assert.Empty(authContext.Policies);
	}

	[Fact(DisplayName = "Register Authorization services with authorizing state.")]
	public void Test0031()
	{
		// act
		var authContext = this.AddAuthorization();
		authContext.SetAuthorizing();

		// assert
		Assert.False(authContext.IsAuthenticated);
		Assert.Equal(AuthorizationState.Authorizing, authContext.State);
		Assert.Empty(authContext.Roles);
		Assert.Empty(authContext.Policies);
	}

	[Fact(DisplayName = "Register Authorization services with authenticated but unauthorized user.")]
	public void Test002()
	{
		// act
		var authContext = this.AddAuthorization();
		authContext.SetAuthorized("DarthPedro", AuthorizationState.Unauthorized);

		// assert
		Assert.True(authContext.IsAuthenticated);
		Assert.Equal(AuthorizationState.Unauthorized, authContext.State);
		Assert.Empty(authContext.Roles);
		Assert.Empty(authContext.Policies);
	}

	[Fact(DisplayName = "Register Authorization services with authenticated and authorized user")]
	public void Test0010()
	{
		// act
		var authContext = this.AddAuthorization();
		authContext.SetAuthorized("DarthPedro");

		// assert
		Assert.True(authContext.IsAuthenticated);
		Assert.Equal(AuthorizationState.Authorized, authContext.State);
		Assert.Empty(authContext.Roles);
		Assert.Empty(authContext.Policies);
	}

	[Fact(DisplayName = "Register Authorization services with authenticated and authorized user and role.")]
	public void Test001()
	{
		// act
		var authContext = this.AddAuthorization();
		authContext.SetAuthorized("DarthPedro");
		authContext.SetRoles("some-role");

		// assert
		Assert.True(authContext.IsAuthenticated);
		Assert.Equal(AuthorizationState.Authorized, authContext.State);
		Assert.Equal(["some-role"], authContext.Roles);
		Assert.Empty(authContext.Policies);
	}

	[Fact(DisplayName = "Register Authorization services with authenticated and authorized user and policy.")]
	public void Test0011()
	{
		// act
		var authContext = this.AddAuthorization();
		authContext.SetAuthorized("DarthPedro");
		authContext.SetPolicies("TestPolicy", "Other");

		// assert
		Assert.True(authContext.IsAuthenticated);
		Assert.Equal(AuthorizationState.Authorized, authContext.State);
		Assert.Equal(["TestPolicy", "Other"], authContext.Policies);
		Assert.Empty(authContext.Roles);
	}
}
