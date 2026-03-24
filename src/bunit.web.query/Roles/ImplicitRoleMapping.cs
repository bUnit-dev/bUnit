using AngleSharp.Dom;

namespace Bunit.Roles;

internal static class ImplicitRoleMapping
{
	internal static AriaRole? GetImplicitRole(IElement element) => element.NodeName switch
	{
		"A" => element.HasAttribute("href") ? AriaRole.Link : AriaRole.Generic,
		"AREA" => element.HasAttribute("href") ? AriaRole.Link : AriaRole.Generic,
		"ARTICLE" => AriaRole.Article,
		"ASIDE" => AriaRole.Complementary,
		"BUTTON" => AriaRole.Button,
		"DATALIST" => AriaRole.Listbox,
		"DETAILS" => AriaRole.Group,
		"DIALOG" => AriaRole.Dialog,
		"FIELDSET" => AriaRole.Group,
		"FIGURE" => AriaRole.Figure,
		"FOOTER" => AriaRole.ContentInfo,
		"FORM" => HasAccessibleName(element) ? AriaRole.Form : null,
		"H1" or "H2" or "H3" or "H4" or "H5" or "H6" => AriaRole.Heading,
		"HEADER" => AriaRole.Banner,
		"HR" => AriaRole.Separator,
		"IMG" => GetImgRole(element),
		"INPUT" => GetInputRole(element),
		"LI" => AriaRole.Listitem,
		"MAIN" => AriaRole.Main,
		"MATH" => AriaRole.Math,
		"MENU" or "OL" or "UL" => AriaRole.List,
		"METER" => AriaRole.Meter,
		"NAV" => AriaRole.Navigation,
		"OPTION" => AriaRole.Option,
		"OPTGROUP" => AriaRole.Group,
		"OUTPUT" => AriaRole.Status,
		"PROGRESS" => AriaRole.Progressbar,
		"SEARCH" => AriaRole.Search,
		"SECTION" => HasAccessibleName(element) ? AriaRole.Region : null,
		"SELECT" => element.HasAttribute("multiple") ? AriaRole.Listbox : AriaRole.Combobox,
		"TABLE" => AriaRole.Table,
		"TBODY" or "THEAD" or "TFOOT" => AriaRole.Rowgroup,
		"TD" => AriaRole.Cell,
		"TEXTAREA" => AriaRole.Textbox,
		"TH" => AriaRole.ColumnHeader,
		"TR" => AriaRole.Row,
		_ => null,
	};

	internal static int? GetHeadingLevel(IElement element)
	{
		// Check aria-level first
		var ariaLevel = element.GetAttribute("aria-level");
		if (ariaLevel is not null && int.TryParse(ariaLevel, out var level))
			return level;

		return element.NodeName switch
		{
			"H1" => 1,
			"H2" => 2,
			"H3" => 3,
			"H4" => 4,
			"H5" => 5,
			"H6" => 6,
			_ => null,
		};
	}

	private static AriaRole? GetInputRole(IElement element)
	{
		var type = element.GetAttribute("type")?.ToUpperInvariant();
		return type switch
		{
			null or "" or "TEXT" or "EMAIL" or "TEL" or "URL" or "PASSWORD" => AriaRole.Textbox,
			"CHECKBOX" => AriaRole.Checkbox,
			"RADIO" => AriaRole.Radio,
			"RANGE" => AriaRole.Slider,
			"NUMBER" => AriaRole.Spinbutton,
			"SEARCH" => AriaRole.Searchbox,
			"BUTTON" or "SUBMIT" or "RESET" or "IMAGE" => AriaRole.Button,
			"HIDDEN" => null,
			_ => AriaRole.Textbox,
		};
	}

	private static AriaRole? GetImgRole(IElement element)
	{
		var alt = element.GetAttribute("alt");
		return alt == string.Empty ? AriaRole.Presentation : AriaRole.Img;
	}

	private static bool HasAccessibleName(IElement element)
	{
		return element.HasAttribute("aria-label") || element.HasAttribute("aria-labelledby");
	}
}
