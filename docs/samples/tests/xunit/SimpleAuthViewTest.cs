using Bunit.TestDoubles.Authorization;
using Xunit;

namespace Bunit.Docs.Samples
{
    public class SimpleAuthViewTest
	{
		[Fact(DisplayName = "SimpleAuthView with unauthenticated user")]
		public void Test001()
		{
			// Arrange
			using var ctx = new TestContext();
			ctx.Services.AddTestAuthorization();

			// Act
			var cut = ctx.RenderComponent<SimpleAuthView>();

			// Assert
			cut.MarkupMatches("Not authorized?");
		}

		[Fact(DisplayName = "SimpleAuthView with authenticated and authorized user")]
		public void Test002()
		{
			// arrange
			using var ctx = new TestContext();
			var authContext = ctx.Services.AddTestAuthorization();
			authContext.SetAuthorized("TestUser");

			// act
			var cut = ctx.RenderComponent<SimpleAuthView>();

			// assert
			cut.MarkupMatches("Authorized!");
		}

		[Fact(DisplayName = "SimpleAuthView with authenticated but unauthorized user")]
		public void Test003()
		{
			// arrange
			using var ctx = new TestContext();
			var authContext = ctx.Services.AddTestAuthorization();
			authContext.SetAuthorized("TestUser", AuthorizationState.Unauthorized);

			// act
			var cut = ctx.RenderComponent<SimpleAuthView>();

			// assert
			cut.MarkupMatches("Not authorized?");
		}

		[Fact(DisplayName = "SimpleAuthView switch from unauthorized to authorized.")]
		public void Test004()
		{
			// arrange
			using var ctx = new TestContext();
			var authContext = ctx.Services.AddTestAuthorization();

			// start off unauthenticated.
			var cut = ctx.RenderComponent<SimpleAuthView>();
			cut.MarkupMatches("Not authorized?");

			// act
			authContext.SetAuthorized("TestUser004", AuthorizationState.Authorized);

			cut.Render();

			// assert
			cut.WaitForAssertion(() => cut.MarkupMatches("Authorized!"));
		}

		[Fact(DisplayName = "SimpleAuthView switch from authorized to unauthorized.")]
		public void Test005()
		{
			// arrange
			using var ctx = new TestContext();
			var authContext = ctx.Services.AddTestAuthorization();
			authContext.SetAuthorized("TestUser005");

			// start off unauthenticated.
			var cut = ctx.RenderComponent<SimpleAuthView>();
			cut.MarkupMatches("Authorized!");

			// act
			authContext.SetNotAuthorized();

			cut.Render();

			// assert
			cut.MarkupMatches("Not authorized?");
		}

		[Fact(DisplayName = "SimpleAuthView rendering without authorization services registered")]
		public void Test006()
		{
			// arrange
			using var ctx = new TestContext();

			// act
			var ex = Assert.Throws<MissingFakeAuthorizationException>(() => ctx.RenderComponent<SimpleAuthView>());

			// assert
			Assert.Equal("AuthenticationStateProvider", ex.ServiceName);
			Assert.Equal("https://bunit.egilhansen.com/docs/test-doubles/faking-auth.html", ex.HelpLink);
		}
	}
}
