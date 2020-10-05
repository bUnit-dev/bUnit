using System;
using Bunit.TestAssets.SampleComponents;
using Bunit.TestDoubles;
using Shouldly;
using Xunit;

namespace Bunit
{
	public class TestContextTest : TestContext
	{
		[Fact(DisplayName = "The test service provider should register a placeholder IJSRuntime which throws exceptions")]
		public void Test021()
		{
			var ex = Should.Throw<AggregateException>(() => RenderComponent<SimpleWithJSRuntimeDep>());
			ex.InnerException.ShouldBeOfType<MissingMockJSRuntimeException>();
		}

		[Fact(DisplayName = "The placeholder IJSRuntime is overridden by a supplied mock and does not throw")]
		public void Test022()
		{
			Services.AddMockJSRuntime();
			RenderComponent<SimpleWithJSRuntimeDep>();
		}

		[Fact(DisplayName = "The test service provider should register a placeholder NavigationManager which throws exceptions")]
		public void Test023()
		{
			Should.Throw<MissingMockNavigationManagerException>(() => RenderComponent<SimpleNavigation>());
		}

		[Fact(DisplayName = "The test service provider should register a placeholder HttpClient which throws exceptions")]
		public void Test024()
		{
			Should.Throw<MissingMockHttpClientException>(() => RenderComponent<SimpleWithHttpClient>());
		}

		[Fact(DisplayName = "The test service provider should register a placeholder IStringLocalizer which throws exceptions")]
		public void Test026()
		{
			Should.Throw<MissingMockStringLocalizationException>(() => RenderComponent<SimpleUsingLocalizer>());
		}
	}
}
