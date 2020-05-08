using System.Threading.Tasks;

namespace Bunit.Rendering
{
	/// <summary>
	/// Represents a type that handle <see cref="RenderEvent"/>
	/// from a <see cref="TestRenderer"/> or one of the components it
	/// has rendered.
	/// </summary>
	public interface IRenderEventHandler
	{
		/// <summary>
		/// A handler for <see cref="RenderEvent"/>s.
		/// Must return a completed task when it is done processing the render event.
		/// </summary>
		/// <param name="renderEvent">The render event to process</param>
		/// <returns>A <see cref="Task"/> that completes when the render event has been processed.</returns>
		Task Handle(RenderEvent renderEvent);
	}
}
