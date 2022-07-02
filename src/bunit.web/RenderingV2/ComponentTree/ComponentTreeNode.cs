using AngleSharp.Dom;

namespace Bunit.RenderingV2.ComponentTree;

internal class ComponentTreeNode
{
	private readonly List<ComponentTreeNode> children;

	public INode ParentElement { get; }

	public int ComponentId { get; }

	public IComponent Component { get; }

	public IReadOnlyList<ComponentTreeNode> Children => children;

	public ComponentTreeNode(
		int componentId,
		IComponent component,
		INode parent)
	{
		ComponentId = componentId;
		Component = component;
		ParentElement = parent;
		children = new List<ComponentTreeNode>();
	}

	public void AddChild(ComponentTreeNode child)
		=> children.Add(child);
}

