using System;

namespace Bunit
{
	/// <summary>
	/// Represents a rendered fragment.
	/// </summary>
	public interface IRenderedFragmentBase
	{
		/// <summary>
		/// Gets the id of the rendered component or fragment.
		/// </summary>
		int ComponentId { get; }

		/// <summary>
		/// Gets the total number times the fragment has been through its render life-cycle.
		/// </summary>
		int RenderCount { get; }

		/// <summary>
		/// Adds or removes an event handler that will be triggered after each render of this <see cref="IRenderedFragmentBase"/>.
		/// </summary>
		event Action OnAfterRender;

		/// <summary>
		/// Gets the <see cref="IServiceProvider"/> used when rendering the component.
		/// </summary>
		IServiceProvider Services { get; }
	}
}
