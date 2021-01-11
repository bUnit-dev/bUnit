using System;
using System.Runtime.Serialization;

namespace Bunit
{
	/// <summary>
	/// Represents an exception that is thrown when trying to access an element
	/// that was previous found in the DOM.
	/// </summary>
	[Serializable]
	public sealed class ElementRemovedFromDomException : ElementNotFoundException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ElementRemovedFromDomException"/> class.
		/// </summary>
		public ElementRemovedFromDomException(string cssSelector)
			: base($"The DOM element you tried to access, which you previously found with the CSS selector \"{cssSelector}\", is no longer available in the DOM tree. It has probably been removed after a render.", cssSelector)
		{
		}

		private ElementRemovedFromDomException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext) { }
	}
}
