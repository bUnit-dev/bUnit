namespace Bunit;

/// <summary>
/// Verifies that events raised on an element found through a child components
/// <see cref="IRenderedComponent{TComponent}"/> bubble through the DOM rendered
/// by the child's ancestor components. See issue #983.
/// </summary>
public class ChildComponentEventBubblingTest : BunitContext
{
	[Fact(DisplayName = "Clicking a child component's element bubbles to the parent component's element")]
	public void Test001()
	{
		var cut = Render<ParentDivWithChildButton>();

		cut.FindComponent<ChildWithButton>().Find("button").Click();

		cut.Instance.ClickCount.ShouldBe(1);
	}

	[Fact(DisplayName = "Clicking a child component's submit button triggers the parent component's form")]
	public void Test002()
	{
		var cut = Render<ParentFormWithChildButton>();

		cut.FindComponent<ChildWithButton>().Find("button").Click();

		cut.Instance.SubmitCount.ShouldBe(1);
	}

	[Fact(DisplayName = "Bubbling from a child component's element is identical to bubbling from the parent's view of it")]
	public void Test003()
	{
		var cut = Render<ParentDivWithChildButton>();
		var child = cut.FindComponent<ChildWithButton>();

		cut.Find("button").Click();
		var (parentAfterFirst, childAfterFirst) = (cut.Instance.ClickCount, child.Instance.ClickCount);

		child.Find("button").Click();

		cut.Instance.ClickCount.ShouldBe(parentAfterFirst + 1);
		child.Instance.ClickCount.ShouldBe(childAfterFirst + 1);
	}

	[Fact(DisplayName = "stopPropagation on a parent component's element stops bubbling from a child component's element")]
	public void Test004()
	{
		var cut = Render<GrandParentDivWithChildButton>(ps => ps.Add(p => p.StopPropagation, true));

		cut.FindComponent<ChildWithButton>().Find("button").Click();

		cut.FindComponent<ParentDivWithChildButton>().Instance.ClickCount.ShouldBe(1);
		cut.Instance.ClickCount.ShouldBe(0);
	}

	[Fact(DisplayName = "Events bubble through three levels of components")]
	public void Test005()
	{
		var cut = Render<GrandParentDivWithChildButton>();

		cut.FindComponent<ChildWithButton>().Find("button").Click();

		cut.FindComponent<ChildWithButton>().Instance.ClickCount.ShouldBe(1);
		cut.FindComponent<ParentDivWithChildButton>().Instance.ClickCount.ShouldBe(1);
		cut.Instance.ClickCount.ShouldBe(1);
	}
}
