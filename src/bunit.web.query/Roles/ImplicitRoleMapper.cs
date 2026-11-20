using AngleSharp.Dom;

namespace Bunit;

/// <summary>
/// Maps HTML elements to their implicit ARIA roles.
/// </summary>
/// <remarks>
/// See https://www.w3.org/TR/html-aria/ for the full specification.
/// </remarks>
internal static class ImplicitRoleMapper
{
	private static readonly string[] SectioningElements = ["ARTICLE", "ASIDE", "MAIN", "NAV", "SECTION"];

	/// <summary>
	/// Gets the implicit ARIA role for an HTML element.
	/// </summary>
	public static AriaRole? GetImplicitRole(IElement element)
	{
		var tagName = element.TagName.ToUpperInvariant();

		return tagName switch
		{
			"A" when element.HasAttribute("href") => AriaRole.Link,
			"AREA" when element.HasAttribute("href") => AriaRole.Link,
			"ARTICLE" => AriaRole.Article,
			"ASIDE" => AriaRole.Complementary,
			"BUTTON" => AriaRole.Button,
			"DATALIST" => AriaRole.Listbox,
			"DETAILS" => AriaRole.Group,
			"DIALOG" => AriaRole.Dialog,
			"FIGURE" => AriaRole.Figure,
			"FORM" => GetFormRoleWhenHasAccessibleName(element),
			"FOOTER" => GetFooterRoleWhenNotInSectioningContent(element),
			"H1" or "H2" or "H3" or "H4" or "H5" or "H6" => AriaRole.Heading,
			"HEADER" => GetHeaderRoleWhenNotInSectioningContent(element),
			"HR" => AriaRole.Separator,
			"IMG" => GetImgRoleBasedOnAltAttribute(element),
			"INPUT" => GetInputRole(element),
			"LI" => AriaRole.Listitem,
			"MAIN" => AriaRole.Main,
			"MATH" => AriaRole.Math,
			"MENU" => AriaRole.List,
			"METER" => AriaRole.Meter,
			"NAV" => AriaRole.Navigation,
			"OL" => AriaRole.List,
			"OPTGROUP" => AriaRole.Group,
			"OPTION" => AriaRole.Option,
			"OUTPUT" => AriaRole.Status,
			"PROGRESS" => AriaRole.Progressbar,
			"SECTION" => GetSectionRoleWhenHasAccessibleName(element),
			"SELECT" => GetSelectRoleBasedOnMultipleOrSize(element),
			"SUMMARY" => AriaRole.Button,
			"TABLE" => AriaRole.Table,
			"TBODY" or "TFOOT" or "THEAD" => AriaRole.RowGroup,
			"TD" => AriaRole.Cell,
			"TEXTAREA" => AriaRole.Textbox,
			"TH" => GetThRoleBasedOnScopeAndContext(element),
			"TR" => AriaRole.Row,
			"UL" => AriaRole.List,
			_ => null
		};
	}

	/// <summary>
	/// Gets the heading level for heading elements.
	/// </summary>
	public static int? GetHeadingLevel(IElement element)
	{
		var explicitAriaLevel = element.GetAttribute("aria-level");
		if (!string.IsNullOrWhiteSpace(explicitAriaLevel) && int.TryParse(explicitAriaLevel, out var level))
		{
			return level;
		}

		return element.TagName.ToUpperInvariant() switch
		{
			"H1" => 1,
			"H2" => 2,
			"H3" => 3,
			"H4" => 4,
			"H5" => 5,
			"H6" => 6,
			_ => null
		};
	}

	private static AriaRole? GetFormRoleWhenHasAccessibleName(IElement element) =>
		HasAccessibleName(element) ? AriaRole.Form : null;

	private static AriaRole? GetFooterRoleWhenNotInSectioningContent(IElement element) =>
		IsDescendantOfSectioningContent(element) ? null : AriaRole.ContentInfo;

	private static AriaRole? GetHeaderRoleWhenNotInSectioningContent(IElement element) =>
		IsDescendantOfSectioningContent(element) ? null : AriaRole.Banner;

	private static AriaRole? GetImgRoleBasedOnAltAttribute(IElement element)
	{
		var alt = element.GetAttribute("alt");
		var hasEmptyAltForDecorativeImage = alt is { Length: 0 };
		return hasEmptyAltForDecorativeImage ? AriaRole.Presentation : AriaRole.Img;
	}

	private static AriaRole? GetInputRole(IElement element)
	{
		var type = element.GetAttribute("type")?.ToUpperInvariant() ?? "TEXT";

		return type switch
		{
			"BUTTON" or "IMAGE" or "RESET" or "SUBMIT" => AriaRole.Button,
			"CHECKBOX" => AriaRole.Checkbox,
			"EMAIL" or "TEL" or "TEXT" or "URL" => GetTextInputRoleBasedOnDatalist(element, AriaRole.Textbox),
			"NUMBER" => AriaRole.Spinbutton,
			"RADIO" => AriaRole.Radio,
			"RANGE" => AriaRole.Slider,
			"SEARCH" => GetTextInputRoleBasedOnDatalist(element, AriaRole.Searchbox),
			_ => null
		};
	}

	private static AriaRole GetTextInputRoleBasedOnDatalist(IElement element, AriaRole defaultRole)
	{
		var listAttributeId = element.GetAttribute("list");
		if (string.IsNullOrWhiteSpace(listAttributeId))
			return defaultRole;

		var hasAssociatedDatalist = element.Owner?.GetElementById(listAttributeId) != null;
		return hasAssociatedDatalist ? AriaRole.Combobox : defaultRole;
	}

	private static AriaRole? GetSectionRoleWhenHasAccessibleName(IElement element) =>
		HasAccessibleName(element) ? AriaRole.Region : null;

	private static AriaRole GetSelectRoleBasedOnMultipleOrSize(IElement element)
	{
		if (element.HasAttribute("multiple"))
			return AriaRole.Listbox;

		var size = element.GetAttribute("size");
		var hasMultipleVisibleOptions = !string.IsNullOrWhiteSpace(size)
			&& int.TryParse(size, out var sizeValue)
			&& sizeValue > 1;

		return hasMultipleVisibleOptions ? AriaRole.Listbox : AriaRole.Combobox;
	}

	private static AriaRole GetThRoleBasedOnScopeAndContext(IElement element)
	{
		var scope = element.GetAttribute("scope")?.ToUpperInvariant();
		if (scope == "ROW")
		{
			return AriaRole.RowHeader;
		}

		var isInTableHeader = element.Closest("thead") != null;
		return isInTableHeader ? AriaRole.ColumnHeader : AriaRole.Cell;
	}

	private static bool HasAccessibleName(IElement element)
	{
		return !string.IsNullOrWhiteSpace(element.GetAttribute("aria-label"))
			|| !string.IsNullOrWhiteSpace(element.GetAttribute("aria-labelledby"))
			|| !string.IsNullOrWhiteSpace(element.GetAttribute("title"));
	}

	private static bool IsDescendantOfSectioningContent(IElement element)
	{
		var parent = element.ParentElement;
		while (parent != null)
		{
			if (SectioningElements.Contains(parent.TagName.ToUpperInvariant()))
				return true;
			parent = parent.ParentElement;
		}

		return false;
	}
}
