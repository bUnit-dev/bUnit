using Bunit.TestAssets.SampleComponents;
using Xunit;

namespace Bunit.TestDoubles.Authorization
{
	public class AuthorizationTest
	{
		[Fact(DisplayName = "AuthorizeView with unauthenticated user")]
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

		[Fact(DisplayName = "AuthorizeView with authenticated and authorized user")]
		public void Test002()
		{
			// arrange
			using var ctx = new TestContext();
			var authAdaptor = ctx.Services.AddTestAuthorization();
			authAdaptor.Authenticate("TestUser", AuthorizationState.Authorized);

			// act
			var cut = ctx.RenderComponent<SimpleAuthView>();

			// assert
			cut.MarkupMatches("Authorized!");
		}

		[Fact(DisplayName = "AuthorizeView with authenticated but unauthorized user")]
		public void Test003()
		{
			// arrange
			using var ctx = new TestContext();
			var authAdaptor = ctx.Services.AddTestAuthorization();
			authAdaptor.Authenticate("TestUser", AuthorizationState.Unauthorized);

			// act
			var cut = ctx.RenderComponent<SimpleAuthView>();

			// assert
			cut.MarkupMatches("Not authorized?");
		}

		[Fact(DisplayName = "AuthorizeView switch from unauthorized to authorized.")]
		public void Test004()
		{
			// arrange
			using var ctx = new TestContext();
			var authAdaptor = ctx.Services.AddTestAuthorization();

			// start off unauthenticated.
			var cut = ctx.RenderComponent<SimpleAuthView>();
			cut.MarkupMatches("Not authorized?");

			// act
			authAdaptor.Authenticate("TestUser004", AuthorizationState.Authorized);

			cut.Render();

			// assert
			cut.MarkupMatches("Authorized!");
		}

		[Fact(DisplayName = "AuthorizeView switch from authorized to unauthorized.")]
		public void Test005()
		{
			// arrange
			using var ctx = new TestContext();
			var authAdaptor = ctx.Services.AddTestAuthorization();
			authAdaptor.Authenticate("TestUser005", AuthorizationState.Authorized);

			// start off unauthenticated.
			var cut = ctx.RenderComponent<SimpleAuthView>();
			cut.MarkupMatches("Authorized!");

			// act
			authAdaptor.Unauthenticate();

			cut.Render();

			// assert
			cut.MarkupMatches("Not authorized?");
		}

		[Fact(DisplayName = "AuthorizeView rendering without authorization services registered")]
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
