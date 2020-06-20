using Bunit.TestDoubles.Authorization;
using System;
using Xunit;

namespace Bunit.Docs.Samples
{
    public class SimpleAuthViewWithPolicyTest
    {
		[Fact(DisplayName = "AuthorizeView with set policy with authenticated and authorized user")]
		public void Test001()
		{
			// arrange
			using var ctx = new TestContext();
			var authContext = ctx.Services.AddTestAuthorization();
			authContext.SetAuthorized("TestUser", AuthorizationState.Authorized);
			authContext.SetAuthorizationPolicy("ContentViewer");

			// act
			var cut = ctx.RenderComponent<SimpleAuthViewWithPolicy>();

			// assert
			cut.MarkupMatches("Authorized for content viewer.");
		}

		[Fact(DisplayName = "AuthorizeView without set policy but authenticated and authorized user")]
		public void Test002()
		{
			// arrange
			using var ctx = new TestContext();
			var authContext = ctx.Services.AddTestAuthorization();
			authContext.SetAuthorized("TestUser", AuthorizationState.Authorized);
			authContext.SetAuthorizationPolicy("OtherPolicy");

			// act
			var cut = ctx.RenderComponent<SimpleAuthViewWithPolicy>();

			// assert
			Assert.DoesNotContain(cut.Markup, "Authorized for content viewer", StringComparison.InvariantCultureIgnoreCase);
		}
	}
}
