using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Bunit.RenderingV2.ComponentTree;
using Xunit;

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
		var span = doc.CreateElement("span");
		var sut = new NodeSpan(root);

		root.AppendChild(span);

		sut.Count.ShouldBe(1);
		sut.Length.ShouldBe(1);
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
		sut.Length.ShouldBe(0);
	}
}
