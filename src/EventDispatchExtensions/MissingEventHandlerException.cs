using System;
using System.Linq;
using System.Runtime.Serialization;
using AngleSharp.Dom;

namespace Bunit
{
    /// <summary>
    /// Represents an exception that is thrown when triggering an event handler failed because it wasn't available on the targeted <see cref="IElement"/>.
    /// </summary>
    internal class MissingEventHandlerException : Exception
    {
        public MissingEventHandlerException(IElement element, string missingEventName) : base(CreateErrorMessage(element, missingEventName))
        {
        }

        private static string CreateErrorMessage(IElement element, string missingEventName)
        {
            var result = $"The element does not have an event handler for the event '{missingEventName}";
            var eventHandlers = element.Attributes?
                .Where(x => x.Name.StartsWith(Htmlizer.BLAZOR_ATTR_PREFIX, StringComparison.Ordinal) && !x.Name.StartsWith(Htmlizer.ELEMENT_REFERENCE_ATTR_NAME, StringComparison.Ordinal))
                .Select(x => $"'{x.Name.Remove(0, Htmlizer.BLAZOR_ATTR_PREFIX.Length)}'")
                .ToArray() ?? Array.Empty<string>();

            var suggestAlternatives = ", nor any other events.";

            if (eventHandlers.Length > 1)
                suggestAlternatives = $". The element has event handlers for these events, {string.Join(", ", eventHandlers)}, that you can try instead.";
            if (eventHandlers.Length == 1)
                suggestAlternatives = $". The element has an event handler for {eventHandlers[0]} event, that you can try instead.";

            return $"{result}{suggestAlternatives}";
        }

    }
}