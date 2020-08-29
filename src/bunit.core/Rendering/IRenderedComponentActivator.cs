
using Microsoft.AspNetCore.Components;

namespace Bunit.Rendering
{
	public interface IRenderedComponentActivator
	{
		IRenderedFragmentBase CreateRenderedComponent(int componentId);

		IRenderedComponentBase<TComponent> CreateRenderedComponent<TComponent>(int componentId)
			where TComponent : IComponent;

		IRenderedComponentBase<TComponent> CreateRenderedComponent<TComponent>(int componentId, TComponent component, RenderTreeFrameCollection componentFrames)
			where TComponent : IComponent;
	}
}
