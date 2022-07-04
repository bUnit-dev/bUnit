// This file is a port of the EventFieldInfo.ts in https://github.dev/dotnet/aspnetcore
// Version ported: https://github.dev/dotnet/aspnetcore/blob/231cc287ef848dfc90dab9b3a1eeb7bdbf243d57/src/Components/Web.JS/src/Rendering/Events/EventFieldInfo.ts#L30
using AngleSharp.Dom;
using AngleSharp.Dom.Events;
using AngleSharp.Html.Dom;

namespace Bunit.RenderingPort.Events;

internal record class EventFieldInfo(int ComponentId, object FieldValue) // FieldValue is restricted to either string or boolean in TS.
{
	public static EventFieldInfo? FromEvent(int componentId, Event @event)
	{
		var node = @event.CurrentTarget;
		if (node is IElement element)
		{
			var fieldData = GetFormFieldData(element);
			if (fieldData is not null)
			{
				return new EventFieldInfo(componentId, fieldData);
			}
		}

		// This event isn't happening on a form field that we can reverse-map back to some incoming attribute
		return null;
	}

	private static object? GetFormFieldData(IElement elem)
	{
		// The logic in here should be the inverse of the logic in BrowserRenderer's TryApplySpecialProperty.
		// That is, we're doing the reverse mapping, starting from an HTML property and reconstructing which
		// "special" attribute would have been mapped to that property.
		if (elem is IHtmlInputElement inputElement)
		{
			return inputElement.Type.ToLowerInvariant() == "checkbox"
				? inputElement.IsChecked
				: inputElement.Value;
		}

		if (elem is IHtmlSelectElement selectElm)
		{
			return selectElm.Value;
		}

		if (elem is IHtmlTextAreaElement textArea)
		{
			return textArea.Value;
		}

		return null;
	}
}
