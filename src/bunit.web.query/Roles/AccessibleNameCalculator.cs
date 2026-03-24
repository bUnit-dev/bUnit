using AngleSharp.Dom;

namespace Bunit.Roles;

internal static class AccessibleNameCalculator
{
	internal static string? GetAccessibleName(IElement element, INodeList rootNodes)
	{
		// 1. aria-labelledby — resolve each referenced ID, join with spaces
		var labelledBy = element.GetAttribute("aria-labelledby");
		if (!string.IsNullOrEmpty(labelledBy))
		{
			var ids = labelledBy.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			var parts = new List<string>();
			foreach (var id in ids)
			{
				var referenced = rootNodes.TryQuerySelector($"#{id}");
				if (referenced is not null)
				{
					var text = referenced.TextContent.Trim();
					if (!string.IsNullOrEmpty(text))
						parts.Add(text);
				}
			}

			if (parts.Count > 0)
				return string.Join(" ", parts);
		}

		// 2. aria-label attribute
		var ariaLabel = element.GetAttribute("aria-label");
		if (!string.IsNullOrEmpty(ariaLabel))
			return ariaLabel.Trim();

		// 3. For IMG, AREA, INPUT[type=image]: alt attribute
		var isImageInput = element.NodeName == "INPUT" &&
			string.Equals(element.GetAttribute("type"), "image", StringComparison.OrdinalIgnoreCase);
		if (element.NodeName is "IMG" or "AREA" || isImageInput)
		{
			var alt = element.GetAttribute("alt");
			if (!string.IsNullOrEmpty(alt))
				return alt.Trim();
		}

		// 4. For form controls: associated <label>
		if (element.NodeName is "INPUT" or "SELECT" or "TEXTAREA" or "METER" or "OUTPUT" or "PROGRESS")
		{
			var labelName = GetLabelName(element, rootNodes);
			if (labelName is not null)
				return labelName;
		}

		// 5. Text content (for buttons, links, headings, etc.)
		var textContent = element.TextContent.Trim();
		if (!string.IsNullOrEmpty(textContent))
			return textContent;

		// 6. title attribute (fallback)
		var title = element.GetAttribute("title");
		if (!string.IsNullOrEmpty(title))
			return title.Trim();

		// 7. placeholder attribute (fallback for inputs)
		if (element.NodeName is "INPUT" or "TEXTAREA")
		{
			var placeholder = element.GetAttribute("placeholder");
			if (!string.IsNullOrEmpty(placeholder))
				return placeholder.Trim();
		}

		return null;
	}

	private static string? GetLabelName(IElement element, INodeList rootNodes)
	{
		// Check for label via "for" attribute matching element's id
		var id = element.GetAttribute("id");
		if (!string.IsNullOrEmpty(id))
		{
			var labels = rootNodes.TryQuerySelectorAll($"label[for='{id}']");
			foreach (var label in labels)
			{
				var text = label.TextContent.Trim();
				if (!string.IsNullOrEmpty(text))
					return text;
			}
		}

		// Check for wrapping <label>
		var parent = element.ParentElement;
		while (parent is not null)
		{
			if (parent.NodeName == "LABEL")
			{
				var text = GetTextContentExcluding(parent, element).Trim();
				if (!string.IsNullOrEmpty(text))
					return text;
			}

			parent = parent.ParentElement;
		}

		return null;
	}

	private static string GetTextContentExcluding(IElement container, IElement excluded)
	{
		var parts = new List<string>();
		CollectTextNodes(container, excluded, parts);
		return string.Join("", parts);
	}

	private static void CollectTextNodes(INode node, IElement excluded, List<string> parts)
	{
		foreach (var child in node.ChildNodes)
		{
			if (child == excluded)
				continue;

			if (child.NodeType == NodeType.Text)
			{
				parts.Add(child.TextContent);
			}
			else if (child.NodeType == NodeType.Element)
			{
				CollectTextNodes(child, excluded, parts);
			}
		}
	}
}
