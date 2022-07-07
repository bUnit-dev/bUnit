using AngleSharp.Dom;

namespace Bunit.RenderingPort;

public interface IRenderedComponent<out TComponent> : IRenderedComponent
	where TComponent : IComponent
{
	TComponent Instance { get; }

	public string Markup { get; }
}
