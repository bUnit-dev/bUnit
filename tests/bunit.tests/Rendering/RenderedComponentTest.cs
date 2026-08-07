using AngleSharp.Dom;
using AngleSharp.Html.Dom;
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

		wrapper.RenderCount.ShouldBe(2);
		first.RenderCount.ShouldBe(2);
		second.RenderCount.ShouldBe(1);

		second.Render();

		wrapper.RenderCount.ShouldBe(3);
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
	
	[Fact(DisplayName = "Searching first for derived component and then base component finds correct (#1691)")]
	public void Test023()
	{
		var cut = Render<Wrapper>(
			ps => ps.AddChildContent<BaseComponent>()
				.AddChildContent<DerivedComponent>());

		Should.NotThrow(() =>
		{
			cut.FindComponents<BaseComponent>();
			cut.FindComponents<DerivedComponent>();
		});
	}
	
	[Fact(DisplayName = "Using relative units in style attribute can be retrieved")]
	public void Test024()
	{
		var cut = Render<ComponentWithRelativeUnitAsWidth>();
		
		var text = cut.Find(".my-component").GetInnerText();

		text.ShouldNotBeNull();
	}

	[Fact(DisplayName = "Form submission with DataAnnotationsValidator and model recreation works correctly")]
	public void Test025()
	{
		var cut = Render<FormWithValidation>();
		cut.Find("#title").Change("title");

		cut.Find("form").Submit();

		cut.Instance.Invoked.ShouldBeTrue();
	}

	[Fact(DisplayName = "Find<TElement> returns element of specified type when it matches")]
	public void Test026()
	{
		var cut = Render<Wrapper>(x => x.AddChildContent("<input type='text' id='myInput' />"));

		var result = cut.Find<Wrapper, IHtmlInputElement>("#myInput");

		result.ShouldNotBeNull();
		result.Type.ShouldBe("text");
	}

	[Fact(DisplayName = "Find<TElement> throws ElementNotFoundException when no element matches selector")]
	public void Test027()
	{
		var cut = Render<Wrapper>(x => x.AddChildContent("<div></div>"));

		Should.Throw<ElementNotFoundException>(() => cut.Find<Wrapper, IHtmlInputElement>("#nonexistent"));
	}

	[Fact(DisplayName = "FindAll<TElement> returns only elements of specified type")]
	public void Test028()
	{
		var cut = Render<Wrapper>(x => x.AddChildContent("<input type='text' /><div></div><input type='checkbox' />"));

		var results = cut.FindAll<Wrapper, IHtmlInputElement>("*");

		results.Count.ShouldBe(2);
		results.ShouldAllBe(e => e is IHtmlInputElement);
	}

	[Fact(DisplayName = "FindAll<TElement> returns empty list when no elements match the type")]
	public void Test029()
	{
		var cut = Render<Wrapper>(x => x.AddChildContent("<div></div><span></span>"));

		var results = cut.FindAll<Wrapper, IHtmlInputElement>("*");

		results.ShouldBeEmpty();
	}

	private class BaseComponent : ComponentBase
	{
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.AddContent(0, "base");
		}
	}

	private sealed class DerivedComponent : BaseComponent
	{
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.AddContent(0, "derived");
		}
	}

	[Fact(DisplayName = "FindComponent child button click triggers parent form onsubmit (#983)")]
	public void Test030()
	{
		var cut = Render<FormWrapperWithChildSubmitButton>();
		var child = cut.FindComponent<ChildSubmitButton>();

		child.Find("#child-submit-button").Click();

		cut.Instance.FormSubmitted.ShouldBeTrue();
	}

	[Fact(DisplayName = "FindComponent child nodes have correct parent form element (#983)")]
	public void Test031()
	{
		var cut = Render<FormWrapperWithChildSubmitButton>();
		var child = cut.FindComponent<ChildSubmitButton>();

		var button = child.Find("#child-submit-button");
		var parentForm = button.Closest("form");

		parentForm.ShouldNotBeNull();
		parentForm.ShouldBeAssignableTo<IHtmlFormElement>();
	}

	[Fact(DisplayName = "FindComponent child Markup only contains child own HTML")]
	public void Test032()
	{
		var cut = Render<FormWrapperWithChildSubmitButton>();
		var child = cut.FindComponent<ChildSubmitButton>();

		child.Markup.ShouldNotContain("<form");
		child.Markup.ShouldContain("child-submit-button");
	}

	[Fact(DisplayName = "FindComponent root Nodes still work correctly")]
	public void Test033()
	{
		var cut = Render<FormWrapperWithChildSubmitButton>();

		var form = cut.Find("form");

		form.ShouldNotBeNull();
		form.ShouldBeAssignableTo<IHtmlFormElement>();
	}

	[Fact(DisplayName = "FindComponent child nodes have parent context after parent re-render")]
	public void Test034()
	{
		var cut = Render<FormWrapperWithChildSubmitButton>();
		var child = cut.FindComponent<ChildSubmitButton>();

		child.Find("#child-submit-button").Click();
		cut.Instance.FormSubmitted.ShouldBeTrue();

		var button = child.Find("#child-submit-button");
		button.Closest("form").ShouldNotBeNull();
	}

	[Fact(DisplayName = "FindComponent direct Find on parent still triggers form submit")]
	public void Test035()
	{
		var cut = Render<FormWrapperWithChildSubmitButton>();

		cut.Find("#child-submit-button").Click();

		cut.Instance.FormSubmitted.ShouldBeTrue();
	}

	[Fact(DisplayName = "Nested FindComponent preserves root DOM context")]
	public void Test036()
	{
		var wrapper = Render<TwoComponentWrapper>(builder => builder
			.Add<Wrapper>(p => p.First, wrapper => wrapper
				.AddChildContent<Simple1>(simple1 => simple1
					.Add(p => p.Header, "nested")))
			.Add<Simple1>(p => p.Second, simple1 => simple1
				.Add(p => p.Header, "Second")));

		var wrapperComponent = wrapper.FindComponent<Wrapper>();
		var simple1 = wrapperComponent.FindComponent<Simple1>();

		simple1.Instance.Header.ShouldBe("nested");
		simple1.Find("h1").TextContent.ShouldBe("nested");
	}

	[Fact(DisplayName = "FindComponent child Nodes length matches isolated rendering")]
	public void Test037()
	{
		var cut = Render<FormWrapperWithChildSubmitButton>();
		var child = cut.FindComponent<ChildSubmitButton>();

		child.Nodes.Length.ShouldBeGreaterThan(0);
		child.Find("#child-submit-button").ShouldNotBeNull();
	}

	[Fact(DisplayName = "Multiple FindComponent calls return components with correct shared DOM")]
	public void Test038()
	{
		var wrapper = Render<TwoComponentWrapper>(builder => builder
			.Add<Simple1>(p => p.First, s => s.Add(p => p.Header, "First"))
			.Add<Simple1>(p => p.Second, s => s.Add(p => p.Header, "Second")));

		var components = wrapper.FindComponents<Simple1>();

		components.Count.ShouldBe(2);
		components[0].Find("h1").TextContent.ShouldBe("First");
		components[1].Find("h1").TextContent.ShouldBe("Second");
	}

	[Fact(DisplayName = "FindComponent child has parent element from root DOM tree")]
	public void Test039()
	{
		var wrapper = Render<TwoComponentWrapper>(builder => builder
			.Add<Simple1>(p => p.First, s => s.Add(p => p.Header, "InDiv"))
			.Add<Simple1>(p => p.Second));

		var child = wrapper.FindComponent<Simple1>();
		var heading = child.Find("h1");

		heading.ParentElement.ShouldNotBeNull();
		heading.ParentElement!.LocalName.ShouldBe("div");
	}
}
