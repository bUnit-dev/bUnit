using Bunit.TestAssets.SampleComponents;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Shouldly;
using Xunit;

namespace Bunit
{
	public class TestContextTest : TestContext
	{
		[Fact(DisplayName = "The placeholder IJSRuntime is overridden by a supplied mock and does not throw")]
		public void Test022()
		{
			JSInterop.Mode = JSRuntimeMode.Loose;
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

		[Fact(DisplayName = "Render() renders fragment inside RenderTreee")]
		public void Test030()
		{
			RenderTree.Add<CascadingValue<string>>(ps => ps.Add(p => p.Value, "FOO"));
			var cut = Render(b =>
			{
				b.OpenComponent<ReceivesCascadinValue>(0);
				b.CloseComponent();
			});

			cut.FindComponent<ReceivesCascadinValue>()
				.Instance
				.Value
				.ShouldBe("FOO");
		}

		[Fact(DisplayName = "Render<TComponent>() renders fragment inside RenderTreee")]
		public void Test031()
		{
			RenderTree.Add<CascadingValue<string>>(ps => ps.Add(p => p.Value, "FOO"));
			var cut = Render<ReceivesCascadinValue>(b =>
			{
				b.OpenComponent<ReceivesCascadinValue>(0);
				b.CloseComponent();
			});

			cut.Instance
				.Value
				.ShouldBe("FOO");
		}

		[Fact(DisplayName = "RenderComponent<TComponent>(builder) renders TComponent inside RenderTreee")]
		public void Test032()
		{
			RenderTree.Add<CascadingValue<string>>(ps => ps.Add(p => p.Value, "FOO"));
			var cut = RenderComponent<ReceivesCascadinValue>(ps => ps.Add(p => p.Dummy, null));

			cut.Instance
				.Value
				.ShouldBe("FOO");
		}

		[Fact(DisplayName = "RenderComponent<TComponent>(factories) renders TComponent inside RenderTreee")]
		public void Test033()
		{
			RenderTree.Add<CascadingValue<string>>(ps => ps.Add(p => p.Value, "FOO"));
			var cut = RenderComponent<ReceivesCascadinValue>(("Dummy", null));

			cut.Instance
				.Value
				.ShouldBe("FOO");
		}

		class ReceivesCascadinValue : ComponentBase
		{
			[CascadingParameter] public string? Value { get; set; }

			[Parameter] public object? Dummy { get; set; }
		}
	}
}
