using AngleSharp.Dom;

namespace Bunit.Asserting;

public class MarkupMatchesAssertExtensionsTest : TestContext
{
	private const string ActualMarkup = "<p>FOO</p>";
	private const string ExpectedMarkup = "<div>BAR</div>";
	private static readonly RenderFragment ActualRenderFragment = b => b.AddMarkupContent(0, ActualMarkup);
	private static readonly RenderFragment ExpectedRenderFragment = b => b.AddMarkupContent(0, ExpectedMarkup);
	private IRenderedFragment ActualRenderedFragment => Render(ActualRenderFragment);
	private IRenderedFragment ExpectedRenderedFragment => Render(ExpectedRenderFragment);
	private INodeList ActualNodeList => ActualRenderedFragment.Nodes;
	private INodeList ExpectedNodeList => ExpectedRenderedFragment.Nodes;
	private INode ActualNode => ActualNodeList[0];
	private INode ExpectedNode => ExpectedNodeList[0];

	[Fact(DisplayName = "MarkupMatches with null arguments throws ArgumentNullException")]
	public void Test001()
	{
		Should.Throw<ArgumentNullException>(() => default(string)!.MarkupMatchesAsync(ExpectedMarkup));
		Should.Throw<ArgumentNullException>(() => ActualMarkup.MarkupMatchesAsync(default(string)!));
		Should.Throw<ArgumentNullException>(() => default(string)!.MarkupMatchesAsync(default(string)!));

		Should.Throw<ArgumentNullException>(() => default(string)!.MarkupMatchesAsync(ExpectedRenderedFragment));
		Should.Throw<ArgumentNullException>(() => ActualMarkup.MarkupMatchesAsync(default(IRenderedFragment)!));
		Should.Throw<ArgumentNullException>(() => default(string)!.MarkupMatchesAsync(default(IRenderedFragment)!));

		Should.Throw<ArgumentNullException>(() => default(string)!.MarkupMatchesAsync(ExpectedNodeList));
		Should.Throw<ArgumentNullException>(() => ActualMarkup.MarkupMatchesAsync(default(INodeList)!));
		Should.Throw<ArgumentNullException>(() => default(string)!.MarkupMatchesAsync(default(INodeList)!));

		Should.Throw<ArgumentNullException>(() => default(INodeList)!.MarkupMatchesAsync(ExpectedNodeList));
		Should.Throw<ArgumentNullException>(() => ActualNodeList.MarkupMatchesAsync(default(INodeList)!));
		Should.Throw<ArgumentNullException>(() => default(INodeList)!.MarkupMatchesAsync(default(INodeList)!));

		Should.Throw<ArgumentNullException>(() => default(INodeList)!.MarkupMatchesAsync(ExpectedNode));
		Should.Throw<ArgumentNullException>(() => ActualNodeList.MarkupMatchesAsync(default(INode)!));
		Should.Throw<ArgumentNullException>(() => default(INodeList)!.MarkupMatchesAsync(default(INode)!));

		Should.Throw<ArgumentNullException>(() => default(INode)!.MarkupMatchesAsync(ExpectedNodeList));
		Should.Throw<ArgumentNullException>(() => ActualNode.MarkupMatchesAsync(default(INodeList)!));
		Should.Throw<ArgumentNullException>(() => default(INode)!.MarkupMatchesAsync(default(INodeList)!));

		Should.Throw<ArgumentNullException>(() => default(IRenderedFragment)!.MarkupMatchesAsync(ExpectedRenderFragment));
		Should.Throw<ArgumentNullException>(() => ActualRenderedFragment.MarkupMatchesAsync(default(RenderFragment)!));
		Should.Throw<ArgumentNullException>(() => default(IRenderedFragment)!.MarkupMatchesAsync(default(RenderFragment)!));

		Should.Throw<ArgumentNullException>(() => default(INode)!.MarkupMatchesAsync(ExpectedRenderFragment));
		Should.Throw<ArgumentNullException>(() => ActualNode.MarkupMatchesAsync(default(RenderFragment)!));
		Should.Throw<ArgumentNullException>(() => default(INode)!.MarkupMatchesAsync(default(RenderFragment)!));

		Should.Throw<ArgumentNullException>(() => default(INodeList)!.MarkupMatchesAsync(ExpectedRenderFragment));
		Should.Throw<ArgumentNullException>(() => ActualNodeList.MarkupMatchesAsync(default(RenderFragment)!));
		Should.Throw<ArgumentNullException>(() => default(INodeList)!.MarkupMatchesAsync(default(RenderFragment)!));
	}

	[Fact(DisplayName = "MarkupMatches(string, string) correctly diffs markup")]
	public void Test002()
		=> Should.Throw<HtmlEqualException>(() => ActualMarkup.MarkupMatchesAsync(ExpectedMarkup));

	[Fact(DisplayName = "MarkupMatches(string, IRenderedFragment) correctly diffs markup")]
	public void Test003()
		=> Should.Throw<HtmlEqualException>(() => ActualMarkup.MarkupMatchesAsync(ExpectedRenderedFragment));

	[Fact(DisplayName = "MarkupMatches(string, INodeList) correctly diffs markup")]
	public void Test004()
		=> Should.Throw<HtmlEqualException>(() => ActualMarkup.MarkupMatchesAsync(ExpectedNodeList));

	[Fact(DisplayName = "MarkupMatches(INodeList, INodeList) correctly diffs markup")]
	public void Test005()
		=> Should.Throw<HtmlEqualException>(() => ActualNodeList.MarkupMatchesAsync(ExpectedNodeList));

	[Fact(DisplayName = "MarkupMatches(INodeList, INode) correctly diffs markup")]
	public void Test006()
		=> Should.Throw<HtmlEqualException>(() => ActualNodeList.MarkupMatchesAsync(ExpectedNode));

	[Fact(DisplayName = "MarkupMatches(INode, INodeList) correctly diffs markup")]
	public void Test007()
		=> Should.Throw<HtmlEqualException>(() => ActualNode.MarkupMatchesAsync(ExpectedNodeList));

	[Fact(DisplayName = "MarkupMatches(IRenderedFragment, RenderFragment) correctly diffs markup")]
	public void Test008()
		=> Should.Throw<HtmlEqualException>(() => ActualRenderedFragment.MarkupMatchesAsync(ExpectedRenderFragment));

	[Fact(DisplayName = "MarkupMatches(INode, RenderFragment) correctly diffs markup")]
	public void Test009()
		=> Should.Throw<HtmlEqualException>(() => ActualNode.MarkupMatchesAsync(ExpectedRenderFragment));

	[Fact(DisplayName = "MarkupMatches(INodeList, RenderFragment) correctly diffs markup")]
	public void Test0010()
		=> Should.Throw<HtmlEqualException>(() => ActualNodeList.MarkupMatchesAsync(ExpectedRenderFragment));

	private IRenderedFragment FindAllRenderedFragment => Render(b => b.AddMarkupContent(0, "<div><p><strong>test</strong></p></div>"));
	private readonly string findAllExpectedRenderFragment = "<p><strong>test</strong></p>";

	[Fact(DisplayName = "MarkupMatches combination works with IRenderedFragment's FindAll extension method")]
	public void Test011()
	{
		FindAllRenderedFragment.FindAll("p").MarkupMatchesAsync(findAllExpectedRenderFragment);
	}

	[Fact(DisplayName = "MarkupMatches combination works with FindAll and FindComponents<T>")]
	public void Test012()
	{
		var cut = RenderComponent<RefToSimple1Child>();

		cut.FindAll("h1").MarkupMatches(cut.FindComponentsAsync<Simple1>());
	}

	[Fact(DisplayName = "MarkupMatches combination works with FindAll and a markup string")]
	public void Test013()
	{
		var cut = RenderComponent<NoArgs>();

		cut.FindAll("h1").MarkupMatches("<h1>Hello world</h1>");
	}

	[Fact(DisplayName = "MarkupMatches combination works with Find and FindAll")]
	public void Test014()
	{
		var cut = RenderComponent<TwoChildren>();

		cut.Find("div").MarkupMatches(cut.FindAll("div"));
	}

	[Fact(DisplayName = "MarkupMatches correctly ignores scoped CSS attributes")]
	public void Test_net5_001()
	{
		var cut = RenderComponent<ScopedCssElements>();

		cut.MarkupMatches("<h1>Hello Pink World!</h1>");
	}
}
