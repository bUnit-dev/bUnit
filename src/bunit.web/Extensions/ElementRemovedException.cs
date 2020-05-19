using System;
using System.Diagnostics.CodeAnalysis;

namespace Bunit
{
	/// <summary>
	/// Represents an exception that is thrown when a wrapped element is no longer available in the DOM tree.
	/// </summary>
	[SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "<Pending>")]
	public class ElementRemovedException : Exception
	{
		/// <inheritdoc/>
		public ElementRemovedException() : base("The DOM element you tried to access is no longer available in the DOM tree. It has probably been removed after a render.")
		{
		}
	}
}
