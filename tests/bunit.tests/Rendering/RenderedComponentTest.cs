using Bunit.Rendering;

namespace Bunit;

public class RenderedComponentTest : BunitContext
{
	[Fact(DisplayName = "Call to Render() results in a render of component")]
	public void Test001()
	{
		var cut = Render<Wrapper>(parameters => parameters.AddChildContent("<div>"));
		var initialRenderCount = cut.RenderCount;

		cut.Render();

		cut.RenderCount.ShouldBe(initialRenderCount + 1);
	}

	[Fact(DisplayName = "Call to Render(builder) provides the parameters to component")]
	public void Test004()
	{
		var cut = Render<Wrapper>(parameters => parameters.AddChildContent("<div>"));

		cut.Render(parameters => parameters.AddChildContent("<p>"));

		cut.Find("p").ShouldNotBeNull();
	}

	[Fact(DisplayName = "Call to Render(params) provides the parameters to component")]
	public void Test0041()
	{
		var cut = Render<Wrapper>(parameters => parameters.AddChildContent("<div>"));

		cut.Render(ps => ps.AddChildContent("<p>"));

		cut.Find("p").ShouldNotBeNull();
	}

	[Fact(DisplayName = "Trying to set CascadingValue during Render throws")]
	public void Test003()
	{
		// arrange
		var cut = Render<AllTypesOfParams<string>>();

		// assert
		Should.Throw<InvalidOperationException>(() => cut.Render(ps => ps.Add(p => p.UnnamedCascadingValue, 42)));
		Should.Throw<InvalidOperationException>(() => cut.Render(ps => ps.Add(p => p.NamedCascadingValue, 1337)));
	}

	[Fact(DisplayName = "Getting Instance from a RenderedComponent based on a disposed component throws")]
	public void Test020()
	{
		var cut = Render<ToggleChildComponent>(ps => ps.Add(p => p.ShowChild, true));
		var target = cut.FindComponent<Simple1>();

		// Disposes of <Simple1 />
		cut.Render(ps => ps.Add(p => p.ShowChild, false));

		Should.Throw<ComponentDisposedException>(() => target.Instance);
	}

	[Fact(DisplayName = "Find throws an exception if no element matches the css selector")]
	public void Test021()
	{
		var cut = Render<Wrapper>();
		Should.Throw<ElementNotFoundException>(() => cut.Find("div"));
	}

	[Fact(DisplayName = "Find returns expected element that matches the css selector")]
	public void Test002()
	{
		var cut = Render<Wrapper>(x => x.AddChildContent("<div>"));
		var result = cut.Find("div");
		result.ShouldNotBeNull();
	}

	[Fact(DisplayName = "Nodes should return new instance " +
						"when a event handler trigger has caused changes to DOM tree")]
	public void Test006()
	{
		var cut = Render<ClickCounter>();
		var initialNodes = cut.Nodes;

		cut.Find("button").Click();

		Assert.NotSame(initialNodes, cut.Nodes);
	}

	[Fact(DisplayName = "Nodes should return new instance " +
						"when a nested component has caused the DOM tree to change")]
	public void Test007()
	{
		var cut = Render<CascadingValue<string>>(builder => builder
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
		var cut = Render<RenderOnClick>();
		var initialNodes = cut.Nodes;

		cut.Find("button").Click();

		cut.Instance.RenderCount.ShouldBe(2);
		Assert.Same(initialNodes, cut.Nodes);
	}

	[Fact(DisplayName = "Changes to event handler should return a new instance of DOM tree")]
	public void Test009()
	{
		var cut = Render<ToggleClickHandler>();
		cut.Find("#btn").Click();

		cut.Instance.Counter.ShouldBe(1);

		cut.Render(ps => ps.Add(p => p.HandleClicks, false));

		cut.Find("#btn").Click();

		cut.Instance.Counter.ShouldBe(1);
	}

	[Fact(DisplayName = "FindComponent<TComponent> returns component from first branch of tree in first depth first search")]
	public void Test100()
	{
		var wrapper = Render<TwoComponentWrapper>(builder => builder
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
		var wrapper = Render<TwoComponentWrapper>(builder => builder
			.Add<Wrapper>(p => p.First)
			.Add<Simple1>(p => p.Second, simple1 => simple1
				.Add(p => p.Header, "Second")));

		var cut = wrapper.FindComponent<Simple1>();

		cut.Instance.Header.ShouldBe("Second");
	}

	[Fact(DisplayName = "GetComponent throws when component of requested type is not in the render tree")]
	public void Test102()
	{
		var wrapper = Render<Wrapper>();

		Should.Throw<ComponentNotFoundException>(() => wrapper.FindComponent<Simple1>());
	}

	[Fact(DisplayName = "GetComponents returns all components of requested type using a depth first order")]
	public void Test103()
	{
		var wrapper = Render<TwoComponentWrapper>(builder => builder
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
		var wrapper = Render<TwoComponentWrapper>(parameters => parameters
			.Add<Simple1>(p => p.First)
			.Add<Simple1>(p => p.Second));
		var cuts = wrapper.FindComponents<Simple1>();
		var first = cuts[0];
		var second = cuts[1];

		wrapper.RenderCount.ShouldBe(1);
		first.RenderCount.ShouldBe(1);
		second.RenderCount.ShouldBe(1);

		first.Render();

		wrapper.RenderCount.ShouldBe(1);
		first.RenderCount.ShouldBe(2);
		second.RenderCount.ShouldBe(1);

		second.Render();

		wrapper.RenderCount.ShouldBe(1);
		first.RenderCount.ShouldBe(2);
		second.RenderCount.ShouldBe(2);
	}

	[Fact(DisplayName = "Getting Markup from a RenderedComponent based on a disposed component throws")]
	public void Test022()
	{
		var cut = Render<ToggleChildComponent>(ps => ps.Add(p => p.ShowChild, true));
		var target = cut.FindComponent<Simple1>();

		// Disposes of <Simple1 />
		cut.Render(ps => ps.Add(p => p.ShowChild, false));

		Should.Throw<ComponentDisposedException>(() => target.Markup);
	}
}
