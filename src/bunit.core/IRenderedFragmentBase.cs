using System;
using System.Threading.Tasks;
using Bunit.Rendering;

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
		
		/// <summary>
		/// Invokes the given <paramref name="callback"/> in the context of the associated <see cref="ITestRenderer"/>.
		/// </summary>
		/// <param name="callback"></param>
		/// <returns>A <see cref="Task"/> that will be completed when the action has finished executing.</returns>
		Task InvokeAsync(Action callback);
	}
}
