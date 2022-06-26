using AngleSharp.Dom;

namespace Bunit.RenderingV2;

public interface IRenderedComponent<out TComponent> : IRenderedComponent
	where TComponent : IComponent
{
	TComponent Instance { get; }
}
