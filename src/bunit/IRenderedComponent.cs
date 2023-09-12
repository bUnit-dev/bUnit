namespace Bunit;

/// <inheritdoc/>
public interface IRenderedComponent<out TComponent> : IRenderedFragment
	where TComponent : IComponent
{

	/// <summary>
	/// Invokes an action on the component under test inside the Dispatcher.
	/// </summary>
	void AccessInstance(Action<TComponent> action);
}
