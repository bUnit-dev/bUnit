using AngleSharp.Dom;

namespace Bunit.RenderingPort;

public interface IRenderedComponent<out TComponent>
	where TComponent : IComponent
{
	INodeList Nodes { get; }

	TComponent Instance { get; }
}
