using AngleSharp.Dom;
using Bunit.Rendering;

namespace Bunit.Extensions;

/// <summary>
/// Verifies that a DOM node can be traced back to the component that
/// rendered it. See issue #153.
/// </summary>
public class ElementOwningComponentTest : BunitContext
{
	[Fact(DisplayName = "GetOwningComponent returns the component that rendered the element")]
	public void Test001()
	{
		var cut = Render<ParentDivWithChildButton>();

		var owner = cut.Find("button").GetOwningComponent();

		owner.Instance.ShouldBeOfType<ChildWithButton>();
	}

	[Fact(DisplayName = "GetOwningComponent returns the same rendered component as FindComponent")]
	public void Test002()
	{
		var cut = Render<ParentDivWithChildButton>();
		var child = cut.FindComponent<ChildWithButton>();

		var owner = cut.Find("button").GetOwningComponent();

		owner.ShouldBeSameAs(child);
	}

	[Fact(DisplayName = "GetOwningComponent returns the parent component for an element it rendered")]
	public void Test003()
	{
		var cut = Render<ParentDivWithChildButton>();

		var owner = cut.Find("#parent-div").GetOwningComponent();

		owner.Instance.ShouldBeOfType<ParentDivWithChildButton>();
	}

	[Fact(DisplayName = "GetOwningComponent<T> walks outwards to the nearest component of that type")]
	public void Test004()
	{
		var cut = Render<GrandParentDivWithChildButton>();

		var owner = cut.Find("button").GetOwningComponent<ParentDivWithChildButton>();

		owner.ShouldBeSameAs(cut.FindComponent<ParentDivWithChildButton>());
	}

	[Fact(DisplayName = "GetOwningComponent<T> throws when no ancestor of that type rendered the element")]
	public void Test005()
	{
		var cut = Render<ParentDivWithChildButton>();

		Should.Throw<ComponentNotFoundException>(
			() => cut.Find("button").GetOwningComponent<CounterChild>());
	}

	[Fact(DisplayName = "GetOwningComponent resolves a descendant node to the component that rendered its element")]
	public void Test006()
	{
		var cut = Render<TableWithChildRows>();

		var owner = cut.Find("#child-row td").GetOwningComponent();

		owner.Instance.ShouldBeOfType<ChildTableRow>();
	}

	[Fact(DisplayName = "GetOwningComponent never returns a bUnit infrastructure component")]
	public void Test007()
	{
		var cut = Render<Simple1>();

		var owner = cut.Find("h1").GetOwningComponent();

		owner.Instance.ShouldBeOfType<Simple1>();
	}

	[Fact(DisplayName = "GetOwningComponent throws for a node that was not rendered by bUnit")]
	public void Test008()
	{
		using var parser = new Bunit.Rendering.BunitHtmlParser();
		var nodes = parser.Parse("<p>foo</p>");

		Should.Throw<InvalidOperationException>(() => nodes[0].GetOwningComponent());
	}

	[Fact(DisplayName = "GetOwningComponent resolves an element of a component that has its own document")]
	public void Test010()
	{
		// Markup starting with text cannot be located in the shared document, so the
		// component is parsed into one of its own - both routes must still resolve.
		var cut = Render<ParentWithLeadingTextChild>();
		var child = cut.FindComponent<LeadingTextChild>();

		child.Nodes[0].Owner.ShouldNotBeSameAs(cut.Nodes[0].Owner);

		child.Find("b").GetOwningComponent().ShouldBeSameAs(child);
		cut.Find("b").GetOwningComponent().ShouldBeSameAs(child);
	}

	[Fact(DisplayName = "GetOwningComponent resolves elements from separate render trees to their own component")]
	public void Test009()
	{
		var first = Render<ParentDivWithChildButton>();
		var second = Render<GrandParentDivWithChildButton>();

		first.Find("button").GetOwningComponent()
			.ShouldBeSameAs(first.FindComponent<ChildWithButton>());
		second.Find("button").GetOwningComponent()
			.ShouldBeSameAs(second.FindComponent<ChildWithButton>());
	}
}
