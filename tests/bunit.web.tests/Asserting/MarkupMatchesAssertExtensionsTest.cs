using AngleSharp.Dom;

namespace Bunit.Asserting;

public partial class MarkupMatchesAssertExtensionsTest : TestContext
{
	private const string ActualMarkup = "<p>FOO</p>";
	private const string ExpectedMarkup = "<div>BAR</div>";
	private static readonly RenderFragment ActualRenderFragment = b => b.AddMarkupContent(0, ActualMarkup);
	private static readonly RenderFragment ExpectedRenderFragment = b => b.AddMarkupContent(0, ExpectedMarkup);
	private Task<IRenderedFragment> ActualRenderedFragment => Render(ActualRenderFragment);
	private Task<IRenderedFragment> ExpectedRenderedFragment => Render(ExpectedRenderFragment);
	private async Task<INodeList> ActualNodeList() => (await ActualRenderedFragment).Nodes;
	private async Task<INodeList> ExpectedNodeList() => (await ExpectedRenderedFragment).Nodes;
	private async Task<INode> ActualNode() => (await ActualNodeList())[0];
	private async Task<INode> ExpectedNode() => (await ExpectedNodeList())[0];

	[Fact(DisplayName = "MarkupMatches with null arguments throws ArgumentNullException")]
	public async Task  Test001()
	{
		Should.Throw<ArgumentNullException>(() => default(string)!.MarkupMatches(ExpectedMarkup));
		Should.Throw<ArgumentNullException>(() => ActualMarkup.MarkupMatches(default(string)!));
		Should.Throw<ArgumentNullException>(() => default(string)!.MarkupMatches(default(string)!));

		await Should.ThrowAsync<ArgumentNullException>(async () => default(string)!.MarkupMatches(await ExpectedRenderedFragment));
		Should.Throw<ArgumentNullException>(() => ActualMarkup.MarkupMatches(default(IRenderedFragment)!));
		Should.Throw<ArgumentNullException>(() => default(string)!.MarkupMatches(default(IRenderedFragment)!));

		await Should.ThrowAsync<ArgumentNullException>(async () => default(string)!.MarkupMatches(await ExpectedNodeList()));
		Should.Throw<ArgumentNullException>(() => ActualMarkup.MarkupMatches(default(INodeList)!));
		Should.Throw<ArgumentNullException>(() => default(string)!.MarkupMatches(default(INodeList)!));

		await Should.ThrowAsync<ArgumentNullException>(async () => default(INodeList)!.MarkupMatches(await ExpectedNodeList()));
		await Should.ThrowAsync<ArgumentNullException>(async () => (await ActualNodeList()).MarkupMatches(default(INodeList)!));
		Should.Throw<ArgumentNullException>(() => default(INodeList)!.MarkupMatches(default(INodeList)!));

		await Should.ThrowAsync<ArgumentNullException>(async () => default(INodeList)!.MarkupMatches(await ExpectedNode()));
		await Should.ThrowAsync<ArgumentNullException>(async () => (await ActualNodeList()).MarkupMatches(default(INode)!));
		Should.Throw<ArgumentNullException>(() => default(INodeList)!.MarkupMatches(default(INode)!));

		await Should.ThrowAsync<ArgumentNullException>(async () => default(INode)!.MarkupMatches(await ExpectedNodeList()));
		await Should.ThrowAsync<ArgumentNullException>(async () => (await ActualNode()).MarkupMatches(default(INodeList)!));
		Should.Throw<ArgumentNullException>(() => default(INode)!.MarkupMatches(default(INodeList)!));

		await Should.ThrowAsync<ArgumentNullException>(async () => await default(IRenderedFragment)!.MarkupMatches(ExpectedRenderFragment));
		await Should.ThrowAsync<ArgumentNullException>(async () => await (await ActualRenderedFragment).MarkupMatches(default(RenderFragment)!));
		await Should.ThrowAsync<ArgumentNullException>(async () => await default(IRenderedFragment)!.MarkupMatches(default(RenderFragment)!));

		Should.Throw<ArgumentNullException>(() => default(INode)!.MarkupMatches(ExpectedRenderFragment));
		await Should.ThrowAsync<ArgumentNullException>(async () => await (await ActualNode()).MarkupMatches(default(RenderFragment)!));
		Should.Throw<ArgumentNullException>(() => default(INode)!.MarkupMatches(default(RenderFragment)!));

		Should.Throw<ArgumentNullException>(() => default(INodeList)!.MarkupMatches(ExpectedRenderFragment));
		await Should.ThrowAsync<ArgumentNullException>(async () => (await ActualNodeList()).MarkupMatches(default(RenderFragment)!));
		Should.Throw<ArgumentNullException>(() => default(INodeList)!.MarkupMatches(default(RenderFragment)!));
	}

	[Fact(DisplayName = "MarkupMatches(string, string) correctly diffs markup")]
	public void Test002()
		=> Should.Throw<HtmlEqualException>(() => ActualMarkup.MarkupMatches(ExpectedMarkup));

	[Fact(DisplayName = "MarkupMatches(string, IRenderedFragment) correctly diffs markup")]
	public Task Test003()
		=> Should.ThrowAsync<HtmlEqualException>(async () => ActualMarkup.MarkupMatches(await ExpectedRenderedFragment));

	[Fact(DisplayName = "MarkupMatches(string, INodeList) correctly diffs markup")]
	public Task Test004()
		=> Should.ThrowAsync<HtmlEqualException>(async () => ActualMarkup.MarkupMatches(await ExpectedNodeList()));

	[Fact(DisplayName = "MarkupMatches(INodeList, INodeList) correctly diffs markup")]
	public Task Test005()
		=> Should.ThrowAsync<HtmlEqualException>(async () => (await ActualNodeList()).MarkupMatches(await ExpectedNodeList()));

	[Fact(DisplayName = "MarkupMatches(INodeList, INode) correctly diffs markup")]
	public Task Test006()
		=> Should.ThrowAsync<HtmlEqualException>(async () => (await ActualNodeList()).MarkupMatches(await ExpectedNode()));

	[Fact(DisplayName = "MarkupMatches(INode, INodeList) correctly diffs markup")]
	public Task Test007()
		=> Should.ThrowAsync<HtmlEqualException>(async () => (await ActualNode()).MarkupMatches(await ExpectedNodeList()));

	[Fact(DisplayName = "MarkupMatches(IRenderedFragment, RenderFragment) correctly diffs markup")]
	public Task Test008()
		=> Should.ThrowAsync<HtmlEqualException>(async () => await (await ActualRenderedFragment).MarkupMatches(ExpectedRenderFragment));

	[Fact(DisplayName = "MarkupMatches(INode, RenderFragment) correctly diffs markup")]
	public Task Test009()
		=> Should.ThrowAsync<HtmlEqualException>(async () => await (await ActualNode()).MarkupMatches(ExpectedRenderFragment));

	[Fact(DisplayName = "MarkupMatches(INodeList, RenderFragment) correctly diffs markup")]
	public Task Test0010()
		=> Should.ThrowAsync<HtmlEqualException>(async () => (await ActualNodeList()).MarkupMatches(ExpectedRenderFragment));

	private Task<IRenderedFragment> FindAllRenderedFragment => Render(b => b.AddMarkupContent(0, "<div><p><strong>test</strong></p></div>"));
	private readonly string findAllExpectedRenderFragment = "<p><strong>test</strong></p>";

	[Fact(DisplayName = "MarkupMatches combination works with IRenderedFragment's FindAll extension method")]
	public async Task Test011()
	{
		(await FindAllRenderedFragment).FindAll("p").MarkupMatches(findAllExpectedRenderFragment);
	}

	[Fact(DisplayName = "MarkupMatches combination works with FindAll and FindComponents<T>")]
	public async Task Test012()
	{
		var cut = await RenderComponent<RefToSimple1Child>();

		cut.FindAll("h1").MarkupMatches(cut.FindComponents<Simple1>());
	}

	[Fact(DisplayName = "MarkupMatches combination works with FindAll and a markup string")]
	public async Task Test013()
	{
		var cut = await RenderComponent<NoArgs>();

		cut.FindAll("h1").MarkupMatches("<h1>Hello world</h1>");
	}

	[Fact(DisplayName = "MarkupMatches combination works with Find and FindAll")]
	public async Task Test014()
	{
		var cut = await RenderComponent<TwoChildren>();

		cut.Find("div").MarkupMatches(cut.FindAll("div"));
	}
}
