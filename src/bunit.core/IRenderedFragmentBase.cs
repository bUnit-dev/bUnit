using System;

using Bunit.Rendering;

namespace Bunit
{
	public interface IRenderedFragmentBase : IDisposable
	{
		int RenderCount { get; }

		bool IsDisposed { get; }

		int ComponentId { get; }

		void OnRender(RenderEvent renderEvent);

		/// <summary>
		/// Gets the <see cref="IServiceProvider"/> used when rendering the component.
		/// </summary>
		IServiceProvider Services { get; }

		/// <summary>
		/// Adds or removes an event handler that will be triggered after each render of this <see cref="IRenderedFragmentBase"/>.
		/// </summary>
		event Action? OnAfterRender;
	}
}
