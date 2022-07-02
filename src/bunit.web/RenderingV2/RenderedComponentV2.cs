using AngleSharp.Dom;
using Bunit.RenderingV2.ComponentTree;

namespace Bunit.RenderingV2;

internal class RenderedComponentV2<TComponent> : IRenderedComponent<TComponent>
	where TComponent : IComponent
{
	private readonly ComponentTreeNode componentTreeNode;

	public int ComponentId => componentTreeNode.ComponentId;

	public TComponent Instance { get; }

	// TOOD: update to only reference a range of child nodes
	public INodeList Nodes => componentTreeNode.ParentElement.ChildNodes;
	
	public RenderedComponentV2(ComponentTreeNode componentTreeNode)
	{
		if (componentTreeNode.Component is not TComponent instance)
		{
			throw new ArgumentException($"The {nameof(ComponentTreeNode)} does not point to a component of type {typeof(TComponent)}", nameof(componentTreeNode));
		}

		this.componentTreeNode = componentTreeNode;
		Instance = instance;
	}
}
