using System;
using Bunit.Rendering.RenderEvents;

namespace Bunit
{
	/// <summary>
	/// Represents a rendered fragment.
	/// </summary>
	public interface IRenderedFragment
	{
		/// <summary>
		/// Gets the id of the rendered component or fragment.
		/// </summary>
		int ComponentId { get; }

		/// <summary>
		/// Gets an <see cref="IObservable{RenderEvent}"/> which will provide subscribers with <see cref="RenderEvent"/>s 
		/// whenever the <see cref="IRenderedFragment"/> is rendered.
		/// </summary>
		IObservable<RenderEvent> RenderEvents { get; }

		/// <summary>
		/// Gets the <see cref="IServiceProvider"/> used when rendering the component.
		/// </summary>
		IServiceProvider Services { get; }
	}
}
