using System;
using Bunit.Rendering;
using Microsoft.AspNetCore.Components;

namespace Bunit
{
	/// <summary>
	/// Represents a rendered <see cref="RenderFragment"/>.
	/// </summary>
	public interface IRenderedFragmentBase : IDisposable
	{
		/// <summary>
		/// Gets the total number times the fragment has been through its render life-cycle.
		/// </summary>
		int RenderCount { get; }

		/// <summary>
		/// Gets a value indicating whether the rendered component or fragment has been disposed by the <see cref="ITestRenderer"/>.
		/// </summary>
		bool IsDisposed { get; }

		/// <summary>
		/// Gets the id of the rendered component or fragment.
		/// </summary>
		int ComponentId { get; }

		/// <summary>
		/// Called by the owning <see cref="ITestRenderer"/> when it finishes a render.
		/// </summary>
		/// <param name="renderEvent">A <see cref="RenderEvent"/> that represents a render.</param>
		void OnRender(RenderEvent renderEvent);

		/// <summary>
		/// Gets the <see cref="IServiceProvider"/> used when rendering the component.
		/// </summary>
		IServiceProvider Services { get; }

		/// <summary>
		/// Adds or removes an event handler that will be triggered after each render of this <see cref="IRenderedFragmentBase"/>.
		/// </summary>
		event EventHandler OnAfterRender;
	}
}
