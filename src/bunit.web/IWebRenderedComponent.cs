using Microsoft.AspNetCore.Components;

namespace Bunit
{
	/// <inheritdoc/>
	public interface IWebRenderedComponent<TComponent> : IRenderedComponent<TComponent>, IWebRenderedFragment
		where TComponent : IComponent
	{

	}
}
