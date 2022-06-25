using System.Reflection.Metadata;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace Bunit.RenderingV2;

internal class RenderedComponentV2<TComponent> : IRenderedComponent<TComponent>
	where TComponent : IComponent
{
	private readonly IHtmlParser htmlParser;
	private readonly IElement parentElement;

	public int ComponentId { get; }
	public TComponent Instance { get; }
	public INodeList Nodes { get; private set; }

	public RenderedComponentV2(int componentId, TComponent instance, IHtmlParser htmlParser, IElement parentElement)
	{
		this.htmlParser = htmlParser;
		this.parentElement = parentElement;
		ComponentId = componentId;
		Instance = instance;
		Nodes = parentElement.ChildNodes;
	}

	internal void ApplyRender(in RenderTreeDiff updatedComponent, in RenderBatch renderBatch)
	{
		foreach (var edit in updatedComponent.Edits)
		{
			switch (edit.Type)
			{
				case RenderTreeEditType.PrependFrame:
				{
					ref var frame = ref renderBatch.ReferenceFrames.Array[edit.ReferenceFrameIndex];
					Nodes = htmlParser.ParseFragment(frame.MarkupContent, parentElement);
					break;
				}
			}
		}
	}
}
