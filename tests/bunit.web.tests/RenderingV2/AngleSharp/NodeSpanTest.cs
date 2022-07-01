using System;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Bunit.RenderingV2.ComponentTree;
using Xunit;

namespace Bunit.RenderingV2.AngleSharp;

public class NodeSpanTest
{
	[Fact]
	public void NodeSpan_grows_automatically_when_source_changes()
	{
		var root = "div".ToElement();
		var sut = new NodeSpan(root);

		var span = "span".ToElement();
		root.AppendChild(span);

		sut.Count.ShouldBe(1);
		sut[0].ShouldBeSameAs(span);
		sut.Single().ShouldBeSameAs(span);
	}

	[Fact]
	public void NodeSpan_shrinks_automatically_when_source_changes()
	{
		var root = "div".ToElement();
		var span = "span".ToElement();
		root.AppendChild(span);
		var sut = new NodeSpan(root);

		root.RemoveChild(span);

		sut.Count.ShouldBe(0);
	}

	[Fact]
	public void NodeSpan_ctor_starts_empty()
	{
		var source = "<hr/> text <i>".ToNodesWithParent("div");

		var sut = new NodeSpan2(source);

		sut.Count.ShouldBe(0);
		sut.ShouldBeEmpty();
	}

	//[Fact]
	//public void NodeSpan_Append()
	//{
	//	var source = "<p>text</p>".ToNodesWithParent("div");
	//	var span = "span".ToElement();
	//	var sut = new NodeSpan2(source);

	//	sut.Append(span);

	//	sut[0].ShouldBeSameAs(span);
	//	sut.Count.ShouldBe(1);
	//	sut.ShouldContain(span);
	//}
}
