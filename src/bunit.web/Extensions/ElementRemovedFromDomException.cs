using System;

namespace Bunit
{
	/// <summary>
	/// Represents an exception that is thrown when trying to access an element
	/// that was previous found in the DOM.
	/// </summary>
	public class ElementRemovedFromDomException : ElementNotFoundException
	{
		/// <inheritdoc/>
		public ElementRemovedFromDomException()
		{
		}

		/// <inheritdoc/>
		public ElementRemovedFromDomException(string cssSelector)
			: base($"The DOM element you tried to access, which you previously found with the CSS selector \"{cssSelector}\", is no longer available in the DOM tree. It has probably been removed after a render.", cssSelector)
		{
		}

		/// <inheritdoc/>
		public ElementRemovedFromDomException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
