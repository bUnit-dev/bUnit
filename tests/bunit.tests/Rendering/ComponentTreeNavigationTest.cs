namespace Bunit.Rendering;

/// <summary>
/// Verifies navigation of the component tree. See issue #1180.
/// </summary>
public class ComponentTreeNavigationTest : BunitContext
{
	[Fact(DisplayName = "Parent returns the component that rendered this component")]
	public void Test001()
	{
		var cut = Render<ParentDivWithChildButton>();

		var parent = cut.FindComponent<ChildWithButton>().Parent();

		parent.ShouldNotBeNull().Instance.ShouldBeOfType<ParentDivWithChildButton>();
	}

	[Fact(DisplayName = "Parent of the outermost rendered component is null")]
	public void Test002()
	{
		var cut = Render<ParentDivWithChildButton>();

		cut.Parent().ShouldBeNull();
	}

	[Fact(DisplayName = "Root returns the outermost rendered component")]
	public void Test003()
	{
		var cut = Render<GrandParentDivWithChildButton>();

		var root = cut.FindComponent<ChildWithButton>().Root();

		root.ShouldBeSameAs(cut);
	}

	[Fact(DisplayName = "GetChildren returns direct child components only, in render order")]
	public void Test004()
	{
		var cut = Render<SiblingChildren>();

		var children = cut.GetChildren();

		children.Count.ShouldBe(2);
		children[0].Instance.ShouldBeOfType<ChildWithButton>();
		children[1].Instance.ShouldBeOfType<CounterChild>();
	}

	[Fact(DisplayName = "GetChildren does not return grandchildren")]
	public void Test005()
	{
		var cut = Render<GrandParentDivWithChildButton>();

		var children = cut.GetChildren();

		children.Count.ShouldBe(1);
		children[0].Instance.ShouldBeOfType<ParentDivWithChildButton>();
	}

	[Fact(DisplayName = "GetChildren<T> returns only direct children of that type")]
	public void Test006()
	{
		var cut = Render<SiblingChildren>();

		var children = cut.GetChildren<CounterChild>();

		children.Count.ShouldBe(1);
		children[0].ShouldBeSameAs(cut.FindComponent<CounterChild>());
	}

	[Fact(DisplayName = "GetAncestors returns components from the closest parent to the root")]
	public void Test007()
	{
		var cut = Render<GrandParentDivWithChildButton>();

		var ancestors = cut.FindComponent<ChildWithButton>().GetAncestors().ToArray();

		ancestors.Length.ShouldBe(2);
		ancestors[0].Instance.ShouldBeOfType<ParentDivWithChildButton>();
		ancestors[1].Instance.ShouldBeOfType<GrandParentDivWithChildButton>();
	}
}
