using System;
using System.Collections.Generic;
using System.Text;

namespace Bunit.Rendering
{
	/// <summary>
	/// Represents an exception that is thrown when a <see cref="IRenderedFragmentBase"/>'s
	/// properties is accessed after the underlying component has been dispsoed by the renderer.
	/// </summary>
	public class ComponentDisposedException : Exception
	{
		/// <summary>
		/// Creates an instance of the <see cref="ComponentDisposedException"/>.
		/// </summary>
		/// <param name="componentId">Id of the disposed component.</param>
		public ComponentDisposedException(int componentId)
			: base($"The component has been removed from the render tree by the renderer and is no longer available for inspection. ComponentId = {componentId}.")
		{

		}
	}
}
