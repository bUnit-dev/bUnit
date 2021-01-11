using Bunit.Rendering;
using Bunit.TestAssets.SampleComponents;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Bunit
{
	public class RenderedFragmentTest : TestContext
	{
		public RenderedFragmentTest(ITestOutputHelper output)
		{
			Services.AddXunitLogger(output, minimumLogLevel: LogLevel.Debug);
		}

		[Fact(DisplayName = "Find throws an exception if no element matches the css selector")]
		public void Test001()
		{
			var cut = RenderComponent<Wrapper>();
			Should.Throw<ElementNotFoundException>(() => cut.Find("div"));
		}

		[Fact(DisplayName = "Find returns expected element that matches the css selector")]
		public void Test002()
		{
			var cut = RenderComponent<Wrapper>(x => x.AddChildContent("<div>"));
			var result = cut.Find("div");
			result.ShouldNotBeNull();
		}

		[Fact(DisplayName = "Nodes should return new instance " +
							"when a event handler trigger has caused changes to DOM tree")]
		public void Test006()
		{
			var cut = RenderComponent<ClickCounter>();
			var initialNodes = cut.Nodes;

			cut.Find("button").Click();

			Assert.NotSame(initialNodes, cut.Nodes);
		}

		[Fact(DisplayName = "Nodes should return new instance " +
							"when a nested component has caused the DOM tree to change")]
		public void Test007()
		{
			var cut = RenderComponent<CascadingValue<string>>(builder => builder
				.Add(p => p.Value, "FOO")
				.AddChildContent<ClickCounter>());

			var initialNodes = cut.Nodes;

			cut.Find("button").Click();

			Assert.NotSame(initialNodes, cut.Nodes);
		}

		[Fact(DisplayName = "Nodes should return the same instance " +
							"when a re-render does not causes the DOM to change")]
		public void Test008()
		{
			var cut = RenderComponent<RenderOnClick>();
			var initialNodes = cut.Nodes;

			cut.Find("button").Click();

			cut.Instance.RenderCount.ShouldBe(2);
			Assert.Same(initialNodes, cut.Nodes);
		}

		[Fact(DisplayName = "Changes to event handler should return a new instance of DOM tree")]
		public void Test009()
		{
			var cut = RenderComponent<ToggleClickHandler>();
			cut.Find("#btn").Click();

			cut.Instance.Counter.ShouldBe(1);

			cut.SetParametersAndRender((nameof(ToggleClickHandler.HandleClicks), false));

			cut.Find("#btn").Click();

			cut.Instance.Counter.ShouldBe(1);
		}

		[Fact(DisplayName = "FindComponent<TComponent> returns component from first branch of tree in first depth first search")]
		public void Test100()
		{
			var wrapper = RenderComponent<TwoComponentWrapper>(builder => builder
				.Add<Wrapper>(p => p.First, wrapper => wrapper
					.AddChildContent<Simple1>(simple1 => simple1
						.Add(p => p.Header, "First")))
				.Add<Simple1>(p => p.Second, simple1 => simple1
					.Add(p => p.Header, "Second")));

			var cut = wrapper.FindComponent<Simple1>();

			cut.Instance.Header.ShouldBe("First");
		}

		[Fact(DisplayName = "FindComponent<TComponent> finds components when first tree branch is empty")]
		public void Test101()
		{
			var wrapper = RenderComponent<TwoComponentWrapper>(builder => builder
				.Add<Wrapper>(p => p.First)
				.Add<Simple1>(p => p.Second, simple1 => simple1
					.Add(p => p.Header, "Second")));

			var cut = wrapper.FindComponent<Simple1>();

			cut.Instance.Header.ShouldBe("Second");
		}

		[Fact(DisplayName = "GetComponent throws when component of requested type is not in the render tree")]
		public void Test102()
		{
			var wrapper = RenderComponent<Wrapper>();

			Should.Throw<ComponentNotFoundException>(() => wrapper.FindComponent<Simple1>());
		}

		[Fact(DisplayName = "GetComponents returns all components of requested type using a depth first order")]
		public void Test103()
		{
			var wrapper = RenderComponent<TwoComponentWrapper>(builder => builder
				.Add<Wrapper>(p => p.First, wrapper => wrapper
					.AddChildContent<Simple1>(simple1 => simple1
						.Add(p => p.Header, "First")))
				.Add<Simple1>(p => p.Second, simple1 => simple1
					.Add(p => p.Header, "Second")));

			var cuts = wrapper.FindComponents<Simple1>();

			cuts.Count.ShouldBe(2);
			cuts[0].Instance.Header.ShouldBe("First");
			cuts[1].Instance.Header.ShouldBe("Second");
		}

		[Fact(DisplayName = "Render events for non-rendered sub components are not emitted")]
		public void Test010()
		{
			var wrapper = RenderComponent<TwoComponentWrapper>(parameters => parameters
				.Add<Simple1>(p => p.First)
				.Add<Simple1>(p => p.Second));
			var cuts = wrapper.FindComponents<Simple1>();
			var first = cuts[0];
			var second = cuts[1];

			wrapper.RenderCount.ShouldBe(1);
			first.RenderCount.ShouldBe(1);
			second.RenderCount.ShouldBe(1);

			first.Render();

			wrapper.RenderCount.ShouldBe(2);
			first.RenderCount.ShouldBe(2);
			second.RenderCount.ShouldBe(1);

			second.Render();

			wrapper.RenderCount.ShouldBe(3);
			first.RenderCount.ShouldBe(2);
			second.RenderCount.ShouldBe(2);
		}

		[Fact(DisplayName = "Getting Markup from a RenderedFragment based on a disposed component throws")]
		public void Test020()
		{
			var cut = RenderComponent<ToggleChildComponent>(ps => ps.Add(p => p.ShowChild, true));
			var target = cut.FindComponent<Simple1>();

			// Disposes of <Simple1 />
			cut.SetParametersAndRender(ps => ps.Add(p => p.ShowChild, false));

			Should.Throw<ComponentDisposedException>(() => target.Markup);
		}
	}
}
