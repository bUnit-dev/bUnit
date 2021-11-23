namespace Bunit;

/// <inheritdoc/>
public interface IRenderedComponent<out TComponent> : IRenderedComponentBase<TComponent>, IRenderedFragment
	where TComponent : IComponent
{
}
