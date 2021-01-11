using System;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Components;
using Shouldly;
using Xunit;

namespace Bunit.Asserting
{
	public partial class MarkupMatchesAssertExtensionsTest : TestContext
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
	}
}
