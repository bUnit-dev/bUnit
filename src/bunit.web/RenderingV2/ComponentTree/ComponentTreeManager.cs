using System.Diagnostics;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace Bunit.RenderingV2.ComponentTree;

internal class ComponentTreeManager
{
	private readonly IHtmlDocument dom;
	private readonly ComponentTreeNode root;
	private readonly HtmlParser htmlParser;

	public ComponentTreeManager(int componentId, RootComponent component, HtmlParser htmlParser)
	{
		dom = htmlParser.ParseDocument(string.Empty);
		Debug.Assert(dom.Body is not null, "Body in an empty document should not be null.");

		root = new ComponentTreeNode(
			componentId,
			component,
			new HtmlNodeSpan(
				source: dom.Body.ChildNodes,
				offset: 0));
		this.htmlParser = htmlParser;
	}

	private void ApplyEdits(in RenderTreeDiff updatedComponent, in RenderBatch renderBatch)
		=> ApplyEdits(updatedComponent, renderBatch, root);

	private void ApplyEdits(in RenderTreeDiff updatedComponent, in RenderBatch renderBatch, in ComponentTreeNode owner)
	{
		foreach (var edit in updatedComponent.Edits)
		{
			switch (edit.Type)
			{
				case RenderTreeEditType.PrependFrame:
				{
					ApplyPrependFrame(edit.ReferenceFrameIndex, renderBatch, owner);
					break;
				}
				default:
					throw new NotImplementedException($"Edit type not supported: {edit.Type}");
			}
		}
	}

	private void ApplyPrependFrame(int referenceFrameIndex, in RenderBatch renderBatch, in ComponentTreeNode owner)
	{
		ref var frame = ref renderBatch.ReferenceFrames.Array[referenceFrameIndex];
		switch (frame.FrameType)
		{
			case RenderTreeFrameType.Component:
			{
				var child = new ComponentTreeNode(frame.ComponentId, frame.Component);
				owner.AddChild(child);
				for (var i = referenceFrameIndex + 1; i < renderBatch.UpdatedComponents.Count; i++)
				{
					var componentEdits = renderBatch.UpdatedComponents.Array[i];
					if (componentEdits.ComponentId == frame.ComponentId && componentEdits.Edits.Count > 0)
					{
						ApplyEdits(componentEdits, renderBatch, child);
					}
				}
				break;
			}
			case RenderTreeFrameType.Element:
			{
				var elm = dom.CreateElement(frame.ElementName);

				owner.AppendChild(elm);
				var endIndexExcl = referenceFrameIndex + frame.ElementSubtreeLength;
				for (var descendantIndex = referenceFrameIndex + 1; descendantIndex < endIndexExcl; descendantIndex++)
				{
					ref var candidateFrame = ref renderBatch.ReferenceFrames.Array[descendantIndex];
					if (candidateFrame.FrameType == RenderTreeFrameType.Attribute)
					{
						ApplySetAttribute(ref candidateFrame, elm);
					}
					else
					{
						// As soon as we see a non-attribute child, all the subsequent child frames are
						// not attributes, so bail out and insert the remnants recursively
						InsertFrameRange(descendantIndex, endIndexExcl, renderBatch, elm);
						break;
					}
				}
				break;
			}
			case RenderTreeFrameType.Markup:
			{
				var markupNodes = htmlParser.ParseFragment(frame.MarkupContent, owner.ParentElement);
				// TODO: change nodespace to point to first node and then a number of siblings.
				//       then previous nodes removing their nodes does not affect this.
				owner.NodeSpan = new HtmlNodeSpan(owner.NodeSpan.Source, owner.NodeSpan.Offset, owner.NodeSpan.Count + markupNodes.Length);
				while (markupNodes.Length > 0)
				{
					owner.AppendChild(markupNodes[0]);
				}
				break;
			}
			case RenderTreeFrameType.Text:
			{
				owner.SetInnerText(frame.TextContent);
				break;
			}
			default:
				throw new NotImplementedException($"Frame type not supported: {frame.FrameType}");
		}
	}

	private void InsertFrameRange(int startIndex, int endIndexExcl, RenderBatch batch, IElement parent)
	{
		for (var frameIndex = startIndex; frameIndex < endIndexExcl; frameIndex++)
		{
			ApplyPrependFrame(frameIndex, batch, parent);

			// Skip over any descendants, since they are already dealt with recursively
			ref var frame = ref batch.ReferenceFrames.Array[frameIndex];
			frameIndex += CountDescendantFrames(frame);
		}
	}

	private static int CountDescendantFrames(RenderTreeFrame frame) => frame.FrameType switch
	{
		// The following frame types have a subtree length. Other frames may use that memory slot
		// to mean something else, so we must not read it. We should consider having nominal subtypes
		// of RenderTreeFramePointer that prevent access to non-applicable fields.
		RenderTreeFrameType.Component => frame.ComponentSubtreeLength - 1,
		RenderTreeFrameType.Element => frame.ElementSubtreeLength - 1,
		RenderTreeFrameType.Region => frame.RegionSubtreeLength - 1,
		_ => 0,
	};

	private static void ApplySetAttribute(ref RenderTreeFrame attributeFrame, IElement element)
	{
		element.SetAttribute(attributeFrame.AttributeName, attributeFrame.AttributeValue?.ToString() ?? string.Empty);
	}

	private static IElement CreateDomRoot(HtmlParser htmlParser)
	{
		var domRoot = htmlParser.ParseDocument(string.Empty).Body;
		Debug.Assert(domRoot is not null, "Body in an empty document should not be null.");
		return domRoot;
	}
}

