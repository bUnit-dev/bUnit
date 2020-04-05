using Microsoft.AspNetCore.Components;

namespace Bunit
{
	/// <inheritdoc/>
	public interface IRenderedComponent<TComponent> : IRenderedComponentCore<TComponent>, IRenderedFragment
		where TComponent : IComponent
	{
	}
}
