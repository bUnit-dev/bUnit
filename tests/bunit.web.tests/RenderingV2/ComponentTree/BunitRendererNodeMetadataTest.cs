using Bunit.RenderingV2.AngleSharp;

namespace Bunit.RenderingV2.ComponentTree;

public class BunitRendererNodeMetadataTest
{
	[Theory, AutoData]
	internal void AppendChildFrame_single_node_empty_parent(ComponentTreeNode owner)
	{
		var parent = "p".ToElement();
		var node = "span".ToElement();

		parent.AppendChildFrame(node, owner);

		parent.ChildNodes
			.ShouldHaveSingleItem()
			.ShouldBeSameAs(node);
		node.GetOwningComponent()
			.ShouldBeSameAs(owner);
	}

	[Theory, AutoData]
	internal void AppendChildFrame_multiple_nodes_empty_parent(ComponentTreeNode owner)
	{
		var parent = "p".ToElement();
		var nodes = "<span></span><p></p>text".ToNodes();

		parent.AppendChildFrame(nodes, owner);

		parent.ChildNodes.Length.ShouldBe(3);
		nodes.ShouldAllSatisfyCondition(
			n => n.GetOwningComponent().ShouldBeSameAs(owner));
	}

	[Theory, AutoData]
	internal void GetChildNodeByFrameIndex_with_single_node_frames(ComponentTreeNode owner)
	{
		var parent = "p".ToElement();
		var frame0 = "span".ToElement();
		var frame1 = "div".ToElement();
		parent.AppendChildFrame(frame0, owner);
		parent.AppendChildFrame(frame1, owner);

		var actual0 = parent.GetChildNodeByFrameIndex(0);
		var actual1 = parent.GetChildNodeByFrameIndex(1);

		actual0.ShouldBeSameAs(frame0);
		actual1.ShouldBeSameAs(frame1);
	}

	[Theory, AutoData]
	internal void GetChildNodeByFrameIndex_with_multi_node_frame_returns_first_node_from_frame(ComponentTreeNode owner)
	{
		var parent = "p".ToElement();
		var frame0 = "text<span>".ToNodes();
		var expected0 = frame0[0];
		parent.AppendChildFrame(frame0, owner);

		var actual0 = parent.GetChildNodeByFrameIndex(0);

		actual0.ShouldBeSameAs(expected0);
	}

	[Theory, AutoData]
	internal void GetChildNodeByFrameIndex_with_multi_node_frames(ComponentTreeNode owner)
	{
		var parent = "p".ToElement();
		var frame0 = "text<span>".ToNodes();
		var frame1 = "<p></p><br /><hr/>".ToNodes();
		var frame2 = "div".ToElement();
		var expected0 = frame0[0];
		var expected1 = frame1[0];
		var expected2 = frame2;
		parent.AppendChildFrame(frame0, owner);
		parent.AppendChildFrame(frame1, owner);
		parent.AppendChildFrame(frame2, owner);

		var actual0 = parent.GetChildNodeByFrameIndex(0);
		var actual1 = parent.GetChildNodeByFrameIndex(1);
		var actual2 = parent.GetChildNodeByFrameIndex(2);

		actual0.ShouldBeSameAs(expected0);
		actual1.ShouldBeSameAs(expected1);
		actual2.ShouldBeSameAs(expected2);
	}

	[Theory]
	[InlineAutoData(0, "DIV", "TABLE")]
	[InlineAutoData(1, "SPAN", "TABLE")]
	[InlineAutoData(2, "SPAN", "DIV")]
	internal void RemoveChildNodes_with_single_node_frames(
		int frameIndexToRemove,
		string expectedElementName0,
		string expectedElementName1,
		ComponentTreeNode owner)
	{
		var parent = "p".ToElement();
		parent.AppendChildFrame("span".ToElement(), owner);
		parent.AppendChildFrame("div".ToElement(), owner);
		parent.AppendChildFrame("table".ToElement(), owner);

		parent.RemoveChildFrame(frameIndexToRemove);

		parent.GetChildNodeByFrameIndex(0).ShouldBeSameAs(parent.ChildNodes[0]);
		parent.GetChildNodeByFrameIndex(1).ShouldBeSameAs(parent.ChildNodes[1]);
		parent.ChildNodes[0].NodeName.ShouldBe(expectedElementName0);
		parent.ChildNodes[1].NodeName.ShouldBe(expectedElementName1);
	}

	[Theory, AutoData]
	internal void RemoveChildFrame_first_frame_with_multi_node_frames(ComponentTreeNode owner)
	{
		var parent = "p".ToElement();
		parent.AppendChildFrame("text<span>".ToNodes(), owner);
		parent.AppendChildFrame("<p></p><br /><hr/>".ToNodes(), owner);
		parent.AppendChildFrame("div".ToElement(), owner);

		parent.RemoveChildFrame(0);

		parent.GetChildNodeByFrameIndex(0).ShouldBeSameAs(parent.ChildNodes[0]);
		parent.GetChildNodeByFrameIndex(1).ShouldBeSameAs(parent.ChildNodes[3]);
		parent.ChildNodes[0].NodeName.ShouldBe("P");
		parent.ChildNodes[3].NodeName.ShouldBe("DIV");
	}

	[Theory, AutoData]
	internal void RemoveChildFrame_middle_frame_with_multi_node_frames(ComponentTreeNode owner)
	{
		var parent = "p".ToElement();
		parent.AppendChildFrame("text<span>".ToNodes(), owner);
		parent.AppendChildFrame("<p></p><br /><hr/>".ToNodes(), owner);
		parent.AppendChildFrame("div".ToElement(), owner);

		parent.RemoveChildFrame(1);

		parent.GetChildNodeByFrameIndex(0).ShouldBeSameAs(parent.ChildNodes[0]);
		parent.GetChildNodeByFrameIndex(1).ShouldBeSameAs(parent.ChildNodes[2]);
		parent.ChildNodes[0].NodeName.ShouldBe("#text");
		parent.ChildNodes[2].NodeName.ShouldBe("DIV");
	}

	[Theory, AutoData]
	internal void RemoveChildFrame_last_frame_with_multi_node_frames(ComponentTreeNode owner)
	{
		var parent = "p".ToElement();
		parent.AppendChildFrame("text<span>".ToNodes(), owner);
		parent.AppendChildFrame("<p></p><br /><hr/>".ToNodes(), owner);
		parent.AppendChildFrame("div".ToElement(), owner);

		parent.RemoveChildFrame(2);

		parent.GetChildNodeByFrameIndex(0).ShouldBeSameAs(parent.ChildNodes[0]);
		parent.GetChildNodeByFrameIndex(1).ShouldBeSameAs(parent.ChildNodes[2]);
		parent.ChildNodes[0].NodeName.ShouldBe("#text");
		parent.ChildNodes[2].NodeName.ShouldBe("P");
	}

	[Theory, AutoData]
	internal void GetOwningComponent_throws_after_frame_removed(ComponentTreeNode owner)
	{
		var parent = "p".ToElement();		
		parent.AppendChildFrame("text<span>".ToNodes(), owner);
		var textNode = parent.ChildNodes[0];
		parent.RemoveChildFrame(0);

		Should.Throw<MissingNodeMetadataException>(() => textNode.GetOwningComponent());
	}
}
