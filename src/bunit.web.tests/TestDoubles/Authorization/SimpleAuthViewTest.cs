using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Bunit.TestDoubles.Authorization
{
	public class SimpleAuthViewTest
	{
		[Fact(DisplayName = "AuthorizeView with unauthenticated user")]
		public void Test001()
		{
			// Arrange
			using var ctx = new TestContext();
			ctx.Services.AddAuthorization();

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
			ctx.Services.AddAuthorization("TestUser", true);

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
			ctx.Services.AddAuthorization("TestUser", false);

			// act
			var cut = ctx.RenderComponent<SimpleAuthView>();

			// assert
			cut.MarkupMatches("Not authorized?");
		}

		[Fact(DisplayName = "AuthorizeView switch from unauthenticated to authenticated.")]
		public void Test004()
		{
			// arrange
			using var ctx = new TestContext();
			ctx.Services.AddAuthorization();

			// start off unauthenticated.
			var cut = ctx.RenderComponent<SimpleAuthView>();
			cut.MarkupMatches("Not authorized?");

			// act
			var authProvider = (FakeAuthenticationStateProvider)ctx.Services.GetRequiredService<AuthenticationStateProvider>();
			var newState = FakeAuthenticationStateProvider.CreateAuthenticationState("TestUser004");
			authProvider.TriggerAuthenticationStateChanged(newState);

			var authService = (FakeAuthorizationService)ctx.Services.GetRequiredService<IAuthorizationService>();
			authService.NextResult = AuthorizationResult.Success();

			cut.Render();

			// assert
			cut.MarkupMatches("Authorized!");
		}
	}
}
