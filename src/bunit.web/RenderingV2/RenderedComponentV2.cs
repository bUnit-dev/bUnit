using AngleSharp.Dom;
using Bunit.RenderingV2.ComponentTree;

namespace Bunit.RenderingV2;

internal class RenderedComponentV2<TComponent> : IRenderedComponent<TComponent>
	where TComponent : IComponent
{
	private readonly ComponentAdapter componentTreeNode;

	public int ComponentId => componentTreeNode.ComponentId;

	public TComponent Instance { get; }

	public INodeList Nodes => componentTreeNode.NodeSpan;
	
	public RenderedComponentV2(in ComponentAdapter componentTreeNode)
	{
		if (componentTreeNode.Component is not TComponent instance)
		{
			throw new ArgumentException($"The {nameof(ComponentAdapter)} does not point to a component of type {typeof(TComponent)}", nameof(componentTreeNode));
		}

		this.componentTreeNode = componentTreeNode;
		Instance = instance;
	}
}
