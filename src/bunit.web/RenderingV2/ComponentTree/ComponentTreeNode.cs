using AngleSharp.Dom;

namespace Bunit.RenderingV2.ComponentTree;

internal struct ComponentTreeNode
{
	private readonly List<ComponentTreeNode> children;

	public readonly int ComponentId { get; }
	public readonly IComponent Component { get; }
	public IElement ParentElement { get; }
	public HtmlNodeSpan NodeSpan { get; set; }
	public IReadOnlyList<ComponentTreeNode> Children => children;

	public ComponentTreeNode(int componentId, IComponent component, IElement parentElement)
		: this(componentId, component, parentElement, HtmlNodeSpan.Empty)
	{
	}

	public ComponentTreeNode(int componentId, IComponent component, IElement parentElement, HtmlNodeSpan nodeSpan)
	{
		ComponentId = componentId;
		Component = component;
		ParentElement = parentElement;
		NodeSpan = nodeSpan;
		children = new List<ComponentTreeNode>();
	}

	public void AddChild(ComponentTreeNode child)
		=> children.Add(child);
}

