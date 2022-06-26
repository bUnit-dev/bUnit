using System.Diagnostics;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Bunit.RenderingV2.AngleSharp;

namespace Bunit.RenderingV2;

internal class RenderedComponentV2<TComponent> : IRenderedComponent<TComponent>
	where TComponent : IComponent
{
	private readonly TestRendererV2 renderer;
	private readonly IElement parentElement;
	private readonly List<IRenderedComponent<IComponent>> children = new();
	private readonly NodeList nodes = new();
	private int latestRenderBatchId;

	public int ComponentId { get; }
	public int RenderCount { get; private set; }
	public TComponent Instance { get; }
	public INodeList Nodes => nodes;
	public IReadOnlyList<IRenderedComponent<IComponent>> Children => children;

	public RenderedComponentV2(int componentId, TComponent instance, TestRendererV2 renderer)
		: this(componentId, instance, renderer, CreateDomRoot(renderer.HtmlParser))
	{ }

	public RenderedComponentV2(int componentId, TComponent instance, TestRendererV2 renderer, IElement parentElement)
	{
		this.renderer = renderer;
		this.parentElement = parentElement;
		ComponentId = componentId;
		Instance = instance;
	}

	void IRenderedComponent.ApplyEdits(RenderTreeDiff updatedComponent, RenderBatch renderBatch, int renderBatchId)
	{
		// This prevents applying the same edits multiple times
		// if the updated component has already been processed as a child
		// component during a previous ApplyEdit from a parent.
		if (latestRenderBatchId == renderBatchId)
			return;
		latestRenderBatchId = renderBatchId;

		renderer.Dispatcher.AssertAccess();

		foreach (var edit in updatedComponent.Edits)
		{
			switch (edit.Type)
			{
				case RenderTreeEditType.PrependFrame:
				{
					ApplyPrependFrame(edit.ReferenceFrameIndex, renderBatch, parentElement);
					break;
				}
				default:
					throw new NotImplementedException($"Edit type not supported: {edit.Type}");
			}
		}
	}

	private void ApplyPrependFrame(int referenceFrameIndex, RenderBatch renderBatch, IElement parent)
	{
		ref var frame = ref renderBatch.ReferenceFrames.Array[referenceFrameIndex];
		switch (frame.FrameType)
		{
			case RenderTreeFrameType.Component:
			{
				var child = renderer.InitializeRenderedComponent(frame.ComponentId, frame.Component, parent);
				children.Add(child);
				for (var i = referenceFrameIndex + 1; i < renderBatch.UpdatedComponents.Count; i++)
				{
					var componentEdits = renderBatch.UpdatedComponents.Array[i];
					if (componentEdits.ComponentId == frame.ComponentId && componentEdits.Edits.Count > 0)
					{
						child.ApplyEdits(componentEdits, renderBatch, latestRenderBatchId);
					}
				}
				break;
			}
			case RenderTreeFrameType.Element:
			{
				var doc = (IHtmlDocument)parent.GetRoot();
				var elm = doc.CreateElement(frame.ElementName);
				nodes.Add(elm);
				parent.AppendChild(elm);
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
				var markupNodes = renderer.HtmlParser.ParseFragment(frame.MarkupContent, parentElement);
				nodes.AddRange(markupNodes);
				while (markupNodes.Length > 0)
				{
					parent.AppendChild(markupNodes[0]);
				}
				break;
			}
			case RenderTreeFrameType.Text:
			{
				parent.SetInnerText(frame.TextContent);
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
