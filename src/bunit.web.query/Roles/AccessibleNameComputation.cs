using AngleSharp.Dom;

namespace Bunit;

/// <summary>
/// Provides methods to compute the accessible name of an element.
/// </summary>
/// <remarks>
/// This is a simplified implementation of the Accessible Name and Description Computation algorithm.
/// See https://www.w3.org/TR/accname-1.1/ for the full specification.
/// </remarks>
[SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Using lowercase for comparison with lowercase constant values.")]
internal static class AccessibleNameComputation
{
	/// <summary>
	/// Computes the accessible name of an element.
	/// </summary>
	public static string? GetAccessibleName(IElement element)
	{
		return GetNameFromAriaLabelledBy(element)
			?? GetNameFromAriaLabel(element)
			?? GetNameFromAssociatedLabelOrNativeContent(element)
			?? GetNameFromTitleAttribute(element)
			?? GetNameFromPlaceholder(element);
	}

	private static string? GetNameFromAriaLabelledBy(IElement element)
	{
		var labelledBy = element.GetAttribute("aria-labelledby");
		if (string.IsNullOrWhiteSpace(labelledBy))
			return null;

		return GetTextFromReferencedElements(element, labelledBy);
	}

	private static string? GetNameFromAriaLabel(IElement element)
	{
		var ariaLabel = element.GetAttribute("aria-label");
		return string.IsNullOrWhiteSpace(ariaLabel) ? null : ariaLabel;
	}

	private static string? GetNameFromAssociatedLabelOrNativeContent(IElement element)
	{
		var tagName = element.TagName.ToUpperInvariant();

		if (tagName is "INPUT" or "SELECT" or "TEXTAREA")
		{
			return GetNameFromLinkedLabel(element)
				?? GetNameFromInputButtonValue(element);
		}

		if (tagName == "IMG")
		{
			return GetNameFromAltAttribute(element);
		}

		if (tagName is "BUTTON" or "A" or "H1" or "H2" or "H3" or "H4" or "H5" or "H6")
		{
			return GetNonEmptyTextContent(element);
		}

		return null;
	}

	private static string? GetNameFromTitleAttribute(IElement element)
	{
		var title = element.GetAttribute("title");
		return string.IsNullOrWhiteSpace(title) ? null : title;
	}

	private static string? GetNameFromPlaceholder(IElement element)
	{
		var tagName = element.TagName.ToUpperInvariant();
		if (tagName is not ("INPUT" or "TEXTAREA"))
			return null;

		var placeholder = element.GetAttribute("placeholder");
		return string.IsNullOrWhiteSpace(placeholder) ? null : placeholder;
	}

	private static string? GetTextFromReferencedElements(IElement element, string spaceDelimitedIds)
	{
		var ids = spaceDelimitedIds.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		var texts = new List<string>();
		var root = GetRootElement(element);

		foreach (var id in ids)
		{
			var referencedElement = element.Owner?.GetElementById(id) ?? root?.QuerySelector($"#{id}");
			if (referencedElement == null)
				continue;

			var text = referencedElement.TextContent.Trim();
			if (!string.IsNullOrWhiteSpace(text))
				texts.Add(text);
		}

		return texts.Count > 0 ? string.Join(" ", texts) : null;
	}

	private static IElement? GetRootElement(IElement element)
	{
		var current = element;
		while (current.ParentElement != null)
		{
			current = current.ParentElement;
		}
		return current;
	}

	private static string? GetNameFromLinkedLabel(IElement element)
	{
		var id = element.GetAttribute("id");
		if (!string.IsNullOrWhiteSpace(id))
		{
			var linkedLabel = FindLabelWithForAttribute(element, id);
			if (linkedLabel != null)
			{
				return linkedLabel.TextContent.Trim();
			}
		}

		var wrappingLabel = element.Closest("label");
		if (wrappingLabel != null)
		{
			return GetTextContentExcludingElement(wrappingLabel, element);
		}

		return null;
	}

	private static IElement? FindLabelWithForAttribute(IElement element, string id)
	{
		var label = element.Owner?.QuerySelector($"label[for='{id}']");
		if (label != null)
			return label;

		var root = GetRootElement(element);
		return root?.QuerySelector($"label[for='{id}']");
	}

	private static string? GetNameFromInputButtonValue(IElement element)
	{
		if (!element.TagName.Equals("INPUT", StringComparison.OrdinalIgnoreCase))
			return null;

		var inputType = element.GetAttribute("type")?.ToLowerInvariant();
		if (inputType is not ("button" or "submit" or "reset"))
			return null;

		var value = element.GetAttribute("value");
		return string.IsNullOrWhiteSpace(value) ? null : value;
	}

	private static string? GetNameFromAltAttribute(IElement element)
	{
		var alt = element.GetAttribute("alt");
		return string.IsNullOrWhiteSpace(alt) ? null : alt;
	}

	private static string? GetNonEmptyTextContent(IElement element)
	{
		var textContent = element.TextContent.Trim();
		return string.IsNullOrWhiteSpace(textContent) ? null : textContent;
	}

	private static string GetTextContentExcludingElement(IElement container, IElement excludeElement)
	{
		var texts = new List<string>();
		CollectTextNodesExcluding(container, excludeElement, texts);
		return string.Join(" ", texts).Trim();
	}

	private static void CollectTextNodesExcluding(INode node, IElement excludeElement, List<string> texts)
	{
		foreach (var child in node.ChildNodes)
		{
			if (child == excludeElement)
				continue;

			if (child.NodeType == NodeType.Text)
			{
				var text = child.TextContent.Trim();
				if (!string.IsNullOrWhiteSpace(text))
					texts.Add(text);
			}
			else if (child.NodeType == NodeType.Element)
			{
				CollectTextNodesExcluding(child, excludeElement, texts);
			}
		}
	}
}
