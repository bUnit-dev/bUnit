namespace Bunit.Rendering
{
	/// <summary>
	/// Represents a producer of <see cref="RenderEvent"/>s.
	/// </summary>
	public interface IRenderEventProducer
	{
		/// <summary>
		/// Adds a <see cref="IRenderEventHandler"/> to this renderer,
		/// which will be triggered when the renderer has finished rendering
		/// a render cycle.
		/// </summary>
		/// <param name="handler">The handler to add.</param>
		void AddRenderEventHandler(IRenderEventHandler handler);

		/// <summary>
		/// Removes a <see cref="IRenderEventHandler"/> from this renderer.
		/// </summary>
		/// <param name="handler">The handler to remove.</param>
		void RemoveRenderEventHandler(IRenderEventHandler handler);
	}
}
