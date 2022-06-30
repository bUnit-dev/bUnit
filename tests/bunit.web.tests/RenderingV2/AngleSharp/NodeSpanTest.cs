using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Bunit.RenderingV2.ComponentTree;

namespace Bunit.RenderingV2.AngleSharp;

public class NodeSpanTest
{
	private readonly HtmlParser htmlParser;
	private readonly IHtmlDocument doc;

	public NodeSpanTest()
	{
		htmlParser = new HtmlParser(new HtmlParserOptions
		{
			IsAcceptingCustomElementsEverywhere = false,
			IsKeepingSourceReferences = false,
			IsNotConsumingCharacterReferences = false,
			IsNotSupportingFrames = false,
			IsPreservingAttributeNames = false,
			IsScripting = false,
			IsSupportingProcessingInstructions = false,
			IsEmbedded = true,
			IsStrictMode = false,
		});
		doc = htmlParser.ParseDocument(string.Empty);
	}

	[Fact]
	public void NodeSpan_grows_automatically_when_source_changes()
	{
		var root = doc.CreateElement("div");
		var sut = new NodeSpan(root);

		var span = doc.CreateElement("span");
		root.AppendChild(span);

		sut.Count.ShouldBe(1);
		sut[0].ShouldBeSameAs(span);
		sut.Single().ShouldBeSameAs(span);
	}

	[Fact]
	public void NodeSpan_shrinks_automatically_when_source_changes()
	{
		var root = doc.CreateElement("div");
		var span = doc.CreateElement("span");
		root.AppendChild(span);
		var sut = new NodeSpan(root);

		root.RemoveChild(span);

		sut.Count.ShouldBe(0);
	}

	[Fact]
	public void NodeSpan_starts_count_after_previous_sibling()
	{
		var root = doc.CreateElement("div");
		var span = doc.CreateElement("span");
		root.AppendChild(span);

		var sut = new NodeSpan(root, previousSibling: span);

		sut.Count.ShouldBe(0);
	}

	[Fact]
	public void NodeSpan_index_with_previous_sibling()
	{
		var root = doc.CreateElement("div");
		var span = doc.CreateElement("span");
		var p = doc.CreateElement("p");
		root.AppendChild(span);
		root.AppendChild(p);

		var sut = new NodeSpan(root, previousSibling: span);

		sut[0].ShouldBeSameAs(p);
		sut.Single().ShouldBeSameAs(p);
	}

	[Fact]
	public void NodeSpan_index_throws_out_of_range_with_previous_sibling()
	{
		var root = doc.CreateElement("div");
		var span = doc.CreateElement("span");
		root.AppendChild(span);

		var sut = new NodeSpan(root, previousSibling: span);

		Should.Throw<ArgumentOutOfRangeException>(() => sut[0]);
	}

	[Fact]
	public void NodeSpan_source_adds_sibling_before_previous_sibling()
	{
		var root = doc.CreateElement("div");
		var span = doc.CreateElement("span");
		root.AppendChild(span);
		var sut = new NodeSpan(root, previousSibling: span);

		var p = doc.CreateElement("p");
		root.InsertBefore(p, span);

		sut.Count.ShouldBe(0);
		sut.ShouldBeEmpty();
	}

	[Fact]
	public void NodeSpan_source_removes_sibling_before_previous_sibling()
	{
		var root = doc.CreateElement("div");
		var span = doc.CreateElement("span");
		var p = doc.CreateElement("p");
		root.AppendChild(p);
		root.AppendChild(span);
		var sut = new NodeSpan(root, previousSibling: span);

		root.RemoveChild(p);

		sut.Count.ShouldBe(0);
		sut.ShouldBeEmpty();
	}

	[Fact]
	public void NodeSpan_stops_count_after_next_sibling()
	{
		var root = doc.CreateElement("div");
		var span = doc.CreateElement("span");
		root.AppendChild(span);

		var sut = new NodeSpan(root, nextSibling: span);

		sut.Count.ShouldBe(0);
	}

	[Fact]
	public void NodeSpan_index_with_next_sibling()
	{
		var root = doc.CreateElement("div");
		var span = doc.CreateElement("span");
		var p = doc.CreateElement("p");
		root.AppendChild(span);
		root.AppendChild(p);

		var sut = new NodeSpan(root, nextSibling: p);

		sut[0].ShouldBeSameAs(span);
		sut.Single().ShouldBeSameAs(span);
	}

	[Fact]
	public void NodeSpan_index_with_next_sibling_throws_out_of_range()
	{
		var root = doc.CreateElement("div");
		var span = doc.CreateElement("span");
		var p = doc.CreateElement("p");
		root.AppendChild(span);
		root.AppendChild(p);

		var sut = new NodeSpan(root, nextSibling: span);

		Should.Throw<ArgumentOutOfRangeException>(() => sut[0]);		
	}

	[Fact]
	public void NodeSpan_source_adds_sibling_after_next_sibling()
	{
		var root = doc.CreateElement("div");
		var span = doc.CreateElement("span");
		root.AppendChild(span);
		var sut = new NodeSpan(root, nextSibling: span);

		var p = doc.CreateElement("p");
		root.AppendChild(p);

		sut.Count.ShouldBe(0);
		sut.ShouldBeEmpty();
	}

	[Fact]
	public void NodeSpan_source_removes_sibling_after_next_sibling()
	{
		var root = doc.CreateElement("div");
		var span = doc.CreateElement("span");
		var p = doc.CreateElement("p");
		root.AppendChild(span);
		root.AppendChild(p);
		var sut = new NodeSpan(root, nextSibling: span);

		root.RemoveChild(p);

		sut.Count.ShouldBe(0);
		sut.ShouldBeEmpty();
	}

	[Fact]
	public void NodeSpan_source_sets_previous_sibling()
	{
		var root = doc.CreateElement("div");
		var span = doc.CreateElement("span");
		root.AppendChild(span);
		var sut = new NodeSpan(root);

		sut.PreviousSibling = span;

		sut.Count.ShouldBe(0);
		sut.ShouldBeEmpty();
	}

	[Fact]
	public void NodeSpan_source_sets_next_sibling()
	{
		var root = doc.CreateElement("div");
		var span = doc.CreateElement("span");
		root.AppendChild(span);
		var sut = new NodeSpan(root);

		sut.NextSibling = span;

		sut.Count.ShouldBe(0);
		sut.ShouldBeEmpty();
	}

	[Fact]
	public void NodeSpan_same_next_sibling_and_previous_sibling()
	{
		var root = doc.CreateElement("div");
		var span = doc.CreateElement("span");
		root.AppendChild(span);
		var sut = new NodeSpan(root, span, span);

		sut.Count.ShouldBe(0);
		sut.ShouldBeEmpty();
		Should.Throw<ArgumentOutOfRangeException>(() => sut[0]);
	}
}
