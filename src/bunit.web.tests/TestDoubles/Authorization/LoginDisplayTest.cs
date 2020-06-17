using System;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Bunit.TestDoubles.Authorization
{
	public class LoginDisplayTest
	{
		private readonly Mock<NavigationManager> navManager = new Mock<NavigationManager>();

		[Fact(DisplayName = "Login AuthorizeView with unauthenticated user")]
		public void Test001()
		{
			// Arrange
			using var ctx = new TestContext();
			ctx.Services.AddAuthorization();
			ctx.Services.AddSingleton<NavigationManager>(this.navManager.Object);

			// Act
			var cut = ctx.RenderComponent<LoginDisplay>();

			// Assert
			cut.MarkupMatches(@"<a href=""authentication/login"">Log in</a>");
		}

		[Fact(DisplayName = "Login AuthorizeView with authenticated and authorized user")]
		public void Test002()
		{
			// arrange
			using var ctx = new TestContext();
			ctx.Services.AddAuthorization("TestUser", true);

			// act
			var cut = ctx.RenderComponent<LoginDisplay>();

			// assert
			Assert.Contains("Hello, TestUser!", cut.Markup, StringComparison.InvariantCulture);
		}

		[Fact(DisplayName = "Login AuthorizeView with authenticated but unauthorized user")]
		public void Test003()
		{
			// arrange
			using var ctx = new TestContext();
			ctx.Services.AddAuthorization("TestUser", false);
			ctx.Services.AddSingleton<NavigationManager>(this.navManager.Object);

			// act
			var cut = ctx.RenderComponent<LoginDisplay>();

			// assert
			cut.MarkupMatches(@"<a href=""authentication/login"">Log in</a>");
		}
	}
}
