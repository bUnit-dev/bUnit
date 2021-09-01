using Microsoft.AspNetCore.Components.Routing;
using Shouldly;
using Xunit;

namespace Bunit.TestDoubles
{
    public class FakeNavigationInterceptionTest
    {
		[Fact(DisplayName = "EnableNavigationInterceptionAsync returns completed task")]
		public void Test001()
		{
			new FakeNavigationInterception()
				.EnableNavigationInterceptionAsync()
				.IsCompletedSuccessfully
				.ShouldBeTrue();
		}

		[Fact(DisplayName = "FakeNavigationInterception is registered as the default INavigationInterception")]
		public void Test002()
		{
			new TestContext().Services.GetService<INavigationInterception>()
				.ShouldBeOfType<FakeNavigationInterception>();
		}
    }
}
