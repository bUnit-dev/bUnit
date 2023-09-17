using System.Security.Claims;

namespace Bunit.TestDoubles.Authorization;

public class AuthorizationTest : TestContext
{
	public AuthorizationTest(ITestOutputHelper outputHelper)
	{
		Services.AddXunitLogger(outputHelper);
	}

	[Fact(DisplayName = "AuthorizeView with unauthenticated user")]
	public void Test001()
	{
		// Arrange
		AddAuthorization();

		// Act
		var cut = Render<SimpleAuthView>();

		// Assert
		cut.MarkupMatches("Not authorized?");
	}

	[Fact(DisplayName = "AuthorizeView with authenticated and authorized user")]
	public void Test002()
	{
		// arrange
		var authContext = AddAuthorization();
		authContext.SetAuthorized("TestUser", AuthorizationState.Authorized);

		// act
		var cut = Render<SimpleAuthView>();

		// assert
		cut.MarkupMatches("Authorized!");
	}

	[Fact(DisplayName = "AuthorizeView with authenticated but unauthorized user")]
	public void Test003()
	{
		// arrange
		var authContext = AddAuthorization();
		authContext.SetAuthorized("TestUser", AuthorizationState.Unauthorized);

		// act
		var cut = Render<SimpleAuthView>();

		// assert
		cut.MarkupMatches("Not authorized?");
	}

	[Fact(DisplayName = "AuthorizeView switch from unauthorized to authorized.")]
	public async Task Test004()
	{
		// arrange
		var authContext = AddAuthorization();

		// start off unauthenticated.
		var cut = Render<SimpleAuthView>();
		cut.MarkupMatches("Not authorized?");

		// act
		authContext.SetAuthorized("TestUser004", AuthorizationState.Authorized);

		cut.Render();

		// assert
		await cut.WaitForAssertionAsync(() => cut.MarkupMatches("Authorized!"));
	}

	[Fact(DisplayName = "AuthorizeView switch from authorized to unauthorized.")]
	public void Test005()
	{
		// arrange
		var authContext = AddAuthorization();
		authContext.SetAuthorized("TestUser005", AuthorizationState.Authorized);

		// start off unauthenticated.
		var cut = Render<SimpleAuthView>();
		cut.MarkupMatches("Authorized!");

		// act
		authContext.SetNotAuthorized();

		cut.Render();

		// assert
		cut.MarkupMatches("Not authorized?");
	}

	[Fact(DisplayName = "AuthorizeView rendering without authorization services registered")]
	public void Test006()
	{
		// act
		var ex = Assert.Throws<MissingAuthorizationHandlerException>(() => Render<SimpleAuthView>());

		// assert
		Assert.Equal("AuthenticationStateProvider", ex.ServiceName);
		Assert.Equal("https://bunit.egilhansen.com/docs/test-doubles/faking-auth", ex.HelpLink);
	}

	[Fact(DisplayName = "AuthorizeView with set policy with authenticated and authorized user")]
	public void Test007()
	{
		// arrange
		var authContext = AddAuthorization();
		authContext.SetAuthorized("TestUser").SetPolicies("ContentViewer");

		// act
		var cut = Render<SimpleAuthViewWithPolicy>();

		// assert
		cut.MarkupMatches("Authorized for content viewers.");
	}

	[Fact(DisplayName = "AuthorizeView without policy set")]
	public void Test008()
	{
		// arrange
		var authContext = AddAuthorization();
		authContext.SetAuthorized("TestUser");

		// act
		var cut = Render<SimpleAuthViewWithPolicy>();

		// assert
		cut.MarkupMatches(string.Empty);
	}

	[Fact(DisplayName = "AuthorizeView with wrong policy set")]
	public void Test0081()
	{
		// arrange
		var authContext = AddAuthorization();
		authContext.SetAuthorized("TestUser").SetPolicies("OtherPolicy");

		// act
		var cut = Render<SimpleAuthViewWithPolicy>();

		// assert
		cut.MarkupMatches(string.Empty);
	}

	[Fact(DisplayName = "SimpleAuthViewWithRole with set role")]
	public void Test009()
	{
		// arrange
		var authContext = AddAuthorization();
		authContext.SetAuthorized("TestUser").SetRoles("Admin");

		// act
		var cut = Render<SimpleAuthViewWithRole>();

		// assert
		cut.MarkupMatches("Authorized content for admins.");
	}

	[Fact(DisplayName = "AuthorizeView without set role")]
	public void Test010()
	{
		// arrange
		var authContext = AddAuthorization();
		authContext.SetAuthorized("TestUser");

		// act
		var cut = Render<SimpleAuthViewWithRole>();

		// assert
		cut.MarkupMatches(string.Empty);
	}

	[Fact(DisplayName = "AuthorizeView with wrong role set")]
	public void Test011()
	{
		// arrange
		var authContext = AddAuthorization();
		authContext.SetAuthorized("TestUser").SetRoles("NotAdmin");

		// act
		var cut = Render<SimpleAuthViewWithRole>();

		// assert
		cut.MarkupMatches(string.Empty);
	}

	[Fact(DisplayName = "AuthorizeView in authorizing state")]
	public void Test012()
	{
		// arrange
		var authContext = AddAuthorization();
		authContext.SetAuthorizing();

		// act
		var cut = Render<SimpleAuthView>();

		// assert
		cut.MarkupMatches("Authorizing...");
	}

	[Fact(DisplayName = "AuthorizeView with claims")]
	public void Test013()
	{
		// arrange
		var userId = new Guid("{5d5fa9c1-abf9-4ed6-8fb0-3365382b629c}");
		var authContext = AddAuthorization();
		var emailClaim = new Claim(ClaimTypes.Email, "user@test.com");
		var uuidClaim = new Claim(ClaimTypes.Sid, userId.ToString());
		authContext.SetAuthorized("TestUser").SetClaims(uuidClaim, emailClaim);

		// act
		var cut = Render<SimpleAuthViewWithClaims>();

		// assert
		cut.MarkupMatches(@$"<div>Authorized!</div>
								<div>Name: TestUser</div>
								<div>Email: user@test.com</div>
								<div>Id: {userId}</div>");
	}

	[Fact(DisplayName = "AuthorizeView without defined claims")]
	public void Test014()
	{
		// arrange
		var authContext = AddAuthorization();
		authContext.SetAuthorized("TestUser");

		// act
		var cut = Render<SimpleAuthViewWithClaims>();

		// assert
		cut.MarkupMatches(@"<div>Authorized!</div>
								<div>Name: TestUser</div>");
	}

	[Fact(DisplayName = "IsInRole can resolve role assigned to auth context")]
	public void Test020()
	{
		var role = "myTestRole";
		var authCtx = AddAuthorization();
		authCtx.SetAuthorized("FooBar");
		authCtx.SetRoles(role);

		var cut = Render<AuthCascading>(ps => ps.Add(p => p.ExpectedRole, role));

		cut.MarkupMatches("<p>True</p>");
	}

	[Fact(DisplayName = "SimpleAuthViewWithCustomAuthType with set auth type")]
	public void Test021()
	{
		// arrange
		var authContext = AddAuthorization();
		authContext.SetAuthorized("TestUser");
		authContext.SetAuthenticationType("custom-auth-type");

		// act
		var cut = Render<SimpleAuthViewWithCustomAuthType>();

		// assert
		cut.MarkupMatches("<p>Authorized content with custom auth type.</p>");
	}

	[Fact(DisplayName = "SimpleAuthViewWithCustomAuthType without set auth type")]
	public void Test022()
	{
		// arrange
		var authContext = AddAuthorization();
		authContext.SetAuthorized("TestUser");

		// act
		var cut = Render<SimpleAuthViewWithCustomAuthType>();

		// assert
		cut.MarkupMatches(string.Empty);
	}
}
