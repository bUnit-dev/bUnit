using System;
using System.Runtime.Serialization;

namespace Bunit.Rendering
{
	/// <summary>
	/// Represents an exception that is thrown when a <see cref="Bunit.IRenderedFragmentBase"/>'s
	/// properties is accessed after the underlying component has been disposed by the renderer.
	/// </summary>
	[Serializable]
	public sealed class ComponentDisposedException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ComponentDisposedException"/> class.
		/// </summary>
		/// <param name="componentId">Id of the disposed component.</param>
		public ComponentDisposedException(int componentId)
			: base($"The component has been removed from the render tree by the renderer and is no longer available for inspection. ComponentId = {componentId}.")
		{
		}

		private ComponentDisposedException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext) { }
	}
}
