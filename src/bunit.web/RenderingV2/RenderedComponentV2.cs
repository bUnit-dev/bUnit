using AngleSharp.Dom;
using Bunit.RenderingV2.ComponentTree;

namespace Bunit.RenderingV2;

internal class RenderedComponentV2<TComponent> : IRenderedComponent<TComponent>
	where TComponent : IComponent
{
	private readonly ComponentTreeNode componentTreeNode;

	public int ComponentId => componentTreeNode.ComponentId;
	public TComponent Instance { get; }
	public INodeList Nodes => componentTreeNode.NodeSpan;
	
	public RenderedComponentV2(in ComponentTreeNode componentTreeNode)
	{
		if (componentTreeNode.Component is not TComponent instance)
		{
			throw new ArgumentException($"The {nameof(ComponentTreeNode)} does not point to a component of type {typeof(TComponent)}", nameof(componentTreeNode));
		}

		this.componentTreeNode = componentTreeNode;
		Instance = instance;
	}
}
