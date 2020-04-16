using Microsoft.AspNetCore.Components;

namespace Bunit
{
	/// <inheritdoc/>
	public interface IRenderedComponent<TComponent> : IRenderedComponentBase<TComponent>, IRenderedFragment
		where TComponent : IComponent
	{
	}
}
