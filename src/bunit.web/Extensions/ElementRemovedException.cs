using System;

namespace Bunit
{
	/// <summary>
	/// Represents an exception that is thrown when a wrapped element is no longer available in the DOM tree.
	/// </summary>
	public class ElementRemovedException : Exception
	{
		/// <inheritdoc/>
		public ElementRemovedException() : base("The DOM element you tried to access is no longer available in the DOM tree. It has probably been removed after a render.")
		{
		}
	}
}
