using AngleSharp.Dom;

namespace Bunit.RenderingV2;

public interface IRenderedComponent<out TComponent>
	where TComponent : IComponent
{
	TComponent Instance { get; }
	INodeList Nodes { get; }
}
