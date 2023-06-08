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
		Should.Throw<ArgumentNullException>(() => default(string)!.MarkupMatches(ExpectedMarkup));
		Should.Throw<ArgumentNullException>(() => ActualMarkup.MarkupMatches(default(string)!));
		Should.Throw<ArgumentNullException>(() => default(string)!.MarkupMatches(default(string)!));

		Should.Throw<ArgumentNullException>(() => default(string)!.MarkupMatches(ExpectedRenderedFragment));
		Should.Throw<ArgumentNullException>(() => ActualMarkup.MarkupMatches(default(IRenderedFragment)!));
		Should.Throw<ArgumentNullException>(() => default(string)!.MarkupMatches(default(IRenderedFragment)!));

		Should.Throw<ArgumentNullException>(() => default(string)!.MarkupMatches(ExpectedNodeList));
		Should.Throw<ArgumentNullException>(() => ActualMarkup.MarkupMatches(default(INodeList)!));
		Should.Throw<ArgumentNullException>(() => default(string)!.MarkupMatches(default(INodeList)!));

		Should.Throw<ArgumentNullException>(() => default(INodeList)!.MarkupMatches(ExpectedNodeList));
		Should.Throw<ArgumentNullException>(() => ActualNodeList.MarkupMatches(default(INodeList)!));
		Should.Throw<ArgumentNullException>(() => default(INodeList)!.MarkupMatches(default(INodeList)!));

		Should.Throw<ArgumentNullException>(() => default(INodeList)!.MarkupMatches(ExpectedNode));
		Should.Throw<ArgumentNullException>(() => ActualNodeList.MarkupMatches(default(INode)!));
		Should.Throw<ArgumentNullException>(() => default(INodeList)!.MarkupMatches(default(INode)!));

		Should.Throw<ArgumentNullException>(() => default(INode)!.MarkupMatches(ExpectedNodeList));
		Should.Throw<ArgumentNullException>(() => ActualNode.MarkupMatches(default(INodeList)!));
		Should.Throw<ArgumentNullException>(() => default(INode)!.MarkupMatches(default(INodeList)!));

		Should.Throw<ArgumentNullException>(() => default(IRenderedFragment)!.MarkupMatches(ExpectedRenderFragment));
		Should.Throw<ArgumentNullException>(() => ActualRenderedFragment.MarkupMatches(default(RenderFragment)!));
		Should.Throw<ArgumentNullException>(() => default(IRenderedFragment)!.MarkupMatches(default(RenderFragment)!));

		Should.Throw<ArgumentNullException>(() => default(INode)!.MarkupMatches(ExpectedRenderFragment));
		Should.Throw<ArgumentNullException>(() => ActualNode.MarkupMatches(default(RenderFragment)!));
		Should.Throw<ArgumentNullException>(() => default(INode)!.MarkupMatches(default(RenderFragment)!));

		Should.Throw<ArgumentNullException>(() => default(INodeList)!.MarkupMatches(ExpectedRenderFragment));
		Should.Throw<ArgumentNullException>(() => ActualNodeList.MarkupMatches(default(RenderFragment)!));
		Should.Throw<ArgumentNullException>(() => default(INodeList)!.MarkupMatches(default(RenderFragment)!));
	}

	[Fact(DisplayName = "MarkupMatches(string, string) correctly diffs markup")]
	public void Test002()
		=> Should.Throw<HtmlEqualException>(() => ActualMarkup.MarkupMatches(ExpectedMarkup));

	[Fact(DisplayName = "MarkupMatches(string, IRenderedFragment) correctly diffs markup")]
	public void Test003()
		=> Should.Throw<HtmlEqualException>(() => ActualMarkup.MarkupMatches(ExpectedRenderedFragment));

	[Fact(DisplayName = "MarkupMatches(string, INodeList) correctly diffs markup")]
	public void Test004()
		=> Should.Throw<HtmlEqualException>(() => ActualMarkup.MarkupMatches(ExpectedNodeList));

	[Fact(DisplayName = "MarkupMatches(INodeList, INodeList) correctly diffs markup")]
	public void Test005()
		=> Should.Throw<HtmlEqualException>(() => ActualNodeList.MarkupMatches(ExpectedNodeList));

	[Fact(DisplayName = "MarkupMatches(INodeList, INode) correctly diffs markup")]
	public void Test006()
		=> Should.Throw<HtmlEqualException>(() => ActualNodeList.MarkupMatches(ExpectedNode));

	[Fact(DisplayName = "MarkupMatches(INode, INodeList) correctly diffs markup")]
	public void Test007()
		=> Should.Throw<HtmlEqualException>(() => ActualNode.MarkupMatches(ExpectedNodeList));

	[Fact(DisplayName = "MarkupMatches(IRenderedFragment, RenderFragment) correctly diffs markup")]
	public void Test008()
		=> Should.Throw<HtmlEqualException>(() => ActualRenderedFragment.MarkupMatches(ExpectedRenderFragment));

	[Fact(DisplayName = "MarkupMatches(INode, RenderFragment) correctly diffs markup")]
	public void Test009()
		=> Should.Throw<HtmlEqualException>(() => ActualNode.MarkupMatches(ExpectedRenderFragment));

	[Fact(DisplayName = "MarkupMatches(INodeList, RenderFragment) correctly diffs markup")]
	public void Test0010()
		=> Should.Throw<HtmlEqualException>(() => ActualNodeList.MarkupMatches(ExpectedRenderFragment));

	private IRenderedFragment FindAllRenderedFragment => Render(b => b.AddMarkupContent(0, "<div><p><strong>test</strong></p></div>"));
	private readonly string findAllExpectedRenderFragment = "<p><strong>test</strong></p>";

	[Fact(DisplayName = "MarkupMatches combination works with IRenderedFragment's FindAll extension method")]
	public void Test011()
	{
		FindAllRenderedFragment.FindAll("p").MarkupMatches(findAllExpectedRenderFragment);
	}

	[Fact(DisplayName = "MarkupMatches combination works with FindAll and FindComponents<T>")]
	public void Test012()
	{
		var cut = Render<RefToSimple1Child>();

		cut.FindAll("h1").MarkupMatches(cut.FindComponents<Simple1>());
	}

	[Fact(DisplayName = "MarkupMatches combination works with FindAll and a markup string")]
	public void Test013()
	{
		var cut = Render<NoArgs>();

		cut.FindAll("h1").MarkupMatches("<h1>Hello world</h1>");
	}

	[Fact(DisplayName = "MarkupMatches combination works with Find and FindAll")]
	public void Test014()
	{
		var cut = Render<TwoChildren>();

		cut.Find("div").MarkupMatches(cut.FindAll("div"));
	}

	[Fact(DisplayName = "MarkupMatches correctly ignores scoped CSS attributes")]
	public void Test_net5_001()
	{
		var cut = Render<ScopedCssElements>();

		cut.MarkupMatches("<h1>Hello Pink World!</h1>");
	}

	[Fact(DisplayName = "Handles HtmlUnknownElement when comparing elements")]
	public void Test015()
	{
		var chart = Render<SimpleSvg>();

		// the path will be returned as a SvgElement since it is
		// parsed in the context of a <svg> element.
		var path = chart.Find("path");

		// path will be parsed as an HtmlUnknownElement because it is not
		// in a known context (e.g. <svg> or <foreignObject>)
		path.MarkupMatches("<path />");
	}

	[Fact(DisplayName = "Handles custom elements with attributes")]
	public void Test016()
	{
		const string expectedMarkup = @"
		<div class=""header"">
			<div>Custom Metadata Definitions</div>
			<zui-button diff:ignoreAttributes></zui-button>
		</div>";

		var cut = Render<CustomElement>();

		cut.MarkupMatches(expectedMarkup);
	}
}
