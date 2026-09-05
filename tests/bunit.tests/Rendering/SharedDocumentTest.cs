using AngleSharp.Dom;
using Bunit.Web.AngleSharp;

namespace Bunit.Rendering;

/// <summary>
/// Verifies that all rendered components under the same root share one
/// AngleSharp document, and that components whose markup cannot be located
/// in it fall back to their own document.
/// </summary>
public class SharedDocumentTest : BunitContext
{
	[Fact(DisplayName = "A child component's element is the same instance as the parent component's view of it")]
	public void Test001()
	{
		var cut = Render<ParentDivWithChildButton>();

		var fromParent = cut.Find("button").Unwrap();
		var fromChild = cut.FindComponent<ChildWithButton>().Find("button").Unwrap();

		fromChild.ShouldBeSameAs(fromParent);
	}

	[Fact(DisplayName = "A child component's element has the parent component's element as its parent")]
	public void Test002()
	{
		var cut = Render<ParentFormWithChildButton>();

		var button = cut.FindComponent<ChildWithButton>().Find("button");

		button.ParentElement.ShouldNotBeNull().Id.ShouldBe("parent-form");
	}

	[Fact(DisplayName = "Nodes of parent and child components come from the same document")]
	public void Test003()
	{
		var cut = Render<ParentDivWithChildButton>();

		var child = cut.FindComponent<ChildWithButton>();

		child.Nodes[0].Owner.ShouldBeSameAs(cut.Nodes[0].Owner);
	}

	[Fact(DisplayName = "Find on a child component only matches elements rendered by that component")]
	public void Test004()
	{
		var cut = Render<ParentDivWithChildButton>();

		var child = cut.FindComponent<ChildWithButton>();

		Should.Throw<ElementNotFoundException>(() => child.Find("#parent-div"));
	}

	[Fact(DisplayName = "Markup of a child component is unaffected by the shared document")]
	public void Test005()
	{
		var cut = Render<ParentDivWithChildButton>();

		var child = cut.FindComponent<ChildWithButton>();

		child.Markup.ShouldStartWith("<button");
		child.Markup.ShouldContain("id=\"child-button\"");
		child.Markup.ShouldNotContain("parent-div");
	}

	[Fact(DisplayName = "Nodes is the same instance when a render does not change the DOM")]
	public void Test006()
	{
		var cut = Render<ParentDivWithChildButton>();
		var child = cut.FindComponent<ChildWithButton>();
		var initialNodes = child.Nodes;

		child.Nodes.ShouldBeSameAs(initialNodes);
	}

	[Fact(DisplayName = "A component whose sibling re-renders gets its element wrapper refreshed")]
	public void Test007()
	{
		var cut = Render<SiblingChildren>();
		var button = cut.FindComponent<ChildWithButton>().Find("button");

		cut.Find("#counter-button").Click();

		// The wrapper must re-resolve into the new document instead of
		// exposing a node detached from the previous parse.
		button.Unwrap().Owner.ShouldBeSameAs(cut.Nodes[0].Owner);
	}

	[Fact(DisplayName = "A child component rendering a <tr> inside the parent's <table> is found")]
	public void Test010()
	{
		var cut = Render<TableWithChildRows>();

		var rows = cut.FindComponents<ChildTableRow>();

		rows.Count.ShouldBe(2);
		rows[0].Find("td").TextContent.ShouldBe("one");
		rows[1].Find("td").TextContent.ShouldBe("two");
	}

	[Fact(DisplayName = "A child component rendering an <option> inherits the parent's <select> value")]
	public void Test011()
	{
		var cut = Render<SelectWithChildOptions>();

		var options = cut.FindComponents<ChildOption>();

		options[0].Markup.ShouldNotContain("selected");
		options[1].Markup.ShouldContain("selected");
	}

	[Fact(DisplayName = "A child component rendering only text still exposes its markup")]
	public void Test020()
	{
		var cut = Render<ParentWithTextOnlyChild>();

		var child = cut.FindComponent<TextOnlyChild>();

		child.Markup.ShouldBe("child-text");
		cut.Find("#parent-p").TextContent.ShouldBe("Hello child-text world");
	}

	[Fact(DisplayName = "A child component rendering nothing has empty markup and no nodes")]
	public void Test021()
	{
		var cut = Render<ParentWithEmptyChild>();

		var child = cut.FindComponent<EmptyChild>();

		child.Markup.ShouldBe(string.Empty);
		child.Nodes.Length.ShouldBe(0);
	}

	[Fact(DisplayName = "Components rendered from separate Render calls do not share a document")]
	public void Test030()
	{
		var first = Render<ParentDivWithChildButton>();
		var second = Render<ParentDivWithChildButton>();

		first.Nodes[0].Owner.ShouldNotBeSameAs(second.Nodes[0].Owner);
	}
}
