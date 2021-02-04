using System;
using System.Linq;
using System.Runtime.Serialization;
using AngleSharp.Dom;

namespace Bunit
{
	/// <summary>
	/// Represents an exception that is thrown when triggering an event handler failed because it wasn't available on the targeted <see cref="IElement"/>.
	/// </summary>
	[Serializable]
	public sealed class MissingEventHandlerException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MissingEventHandlerException"/> class.
		/// </summary>
		public MissingEventHandlerException(IElement element, string missingEventName)
			: base(CreateErrorMessage(element, missingEventName))
		{
		}

		private static string CreateErrorMessage(IElement element, string missingEventName)
		{
			var result = $"The element does not have an event handler for the event '{missingEventName}'";
			var eventHandlers = element.Attributes?
				.Where(x => x.Name.StartsWith(Htmlizer.BlazorAttrPrefix + "on", StringComparison.Ordinal))
				.Select(x => $"'{x.Name.Remove(0, Htmlizer.BlazorAttrPrefix.Length)}'")
				.ToArray() ?? Array.Empty<string>();

			var suggestAlternatives = ", nor any other events.";

			if (eventHandlers.Length > 1)
				suggestAlternatives = $". It does however have event handlers for these events, {string.Join(", ", eventHandlers)}.";
			if (eventHandlers.Length == 1)
				suggestAlternatives = $". It does however have an event handler for the {eventHandlers[0]} event.";

			return $"{result}{suggestAlternatives}";
		}

		private MissingEventHandlerException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext) { }
	}
}
