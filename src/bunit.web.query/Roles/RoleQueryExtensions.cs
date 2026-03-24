using AngleSharp.Dom;
using Bunit.Roles;
using Bunit.Web.AngleSharp;

namespace Bunit;

/// <summary>
/// Extension methods for querying <see cref="IRenderedComponent{TComponent}"/> by ARIA role.
/// </summary>
public static class RoleQueryExtensions
{
	/// <summary>
	/// Returns the first element with the specified ARIA role.
	/// </summary>
	/// <param name="renderedComponent">The rendered fragment to search.</param>
	/// <param name="role">The ARIA role to search for.</param>
	/// <param name="configureOptions">Method used to override the default behavior of FindByRole.</param>
	public static IElement FindByRole(this IRenderedComponent<IComponent> renderedComponent, AriaRole role, Action<ByRoleOptions>? configureOptions = null)
	{
		var options = ByRoleOptions.Default;
		if (configureOptions is not null)
		{
			options = options with { };
			configureOptions.Invoke(options);
		}

		return FindByRoleInternal(renderedComponent, role, options) ?? throw new RoleNotFoundException(role, options);
	}

	/// <summary>
	/// Returns all elements with the specified ARIA role.
	/// </summary>
	/// <param name="renderedComponent">The rendered fragment to search.</param>
	/// <param name="role">The ARIA role to search for.</param>
	/// <param name="configureOptions">Method used to override the default behavior of FindAllByRole.</param>
	/// <returns>A read-only collection of elements matching the role. Returns an empty collection if no matches are found.</returns>
	public static IReadOnlyList<IElement> FindAllByRole(this IRenderedComponent<IComponent> renderedComponent, AriaRole role, Action<ByRoleOptions>? configureOptions = null)
	{
		var options = ByRoleOptions.Default;
		if (configureOptions is not null)
		{
			options = options with { };
			configureOptions.Invoke(options);
		}

		return FindAllByRoleInternal(renderedComponent, role, options);
	}

	internal static IElement? FindByRoleInternal(this IRenderedComponent<IComponent> renderedComponent, AriaRole role, ByRoleOptions options)
	{
		return FindAllByRoleInternal(renderedComponent, role, options).FirstOrDefault();
	}

	internal static IReadOnlyList<IElement> FindAllByRoleInternal(this IRenderedComponent<IComponent> renderedComponent, AriaRole role, ByRoleOptions options)
	{
		var allElements = renderedComponent.Nodes.TryQuerySelectorAll("*");
		var results = new List<IElement>();

		foreach (var element in allElements)
		{
			var effectiveRole = GetEffectiveRole(element);
			if (effectiveRole != role)
				continue;

			if (!MatchesOptions(element, options, renderedComponent.Nodes))
				continue;

			results.Add(element.WrapUsing(new ByRoleElementFactory(renderedComponent, role, options)));
		}

		return results;
	}

	private static bool MatchesOptions(IElement element, ByRoleOptions options, INodeList rootNodes)
	{
		// Name filter
		if (options.Name is not null)
		{
			var accessibleName = AccessibleNameCalculator.GetAccessibleName(element, rootNodes);
			if (accessibleName is null)
				return false;

			if (options.Exact)
			{
				if (!accessibleName.Equals(options.Name, options.NameComparisonType))
					return false;
			}
			else
			{
				if (!accessibleName.Contains(options.Name, options.NameComparisonType))
					return false;
			}
		}

		// Level filter
		if (options.Level.HasValue)
		{
			var headingLevel = ImplicitRoleMapping.GetHeadingLevel(element);
			if (headingLevel != options.Level.Value)
				return false;
		}

		// Selected filter — absent treated as false
		if (options.Selected.HasValue)
		{
			var isSelected = GetElementSelected(element) ?? false;
			if (isSelected != options.Selected.Value)
				return false;
		}

		// Checked filter — absent treated as false
		if (options.Checked.HasValue)
		{
			var isChecked = GetElementChecked(element) ?? false;
			if (isChecked != options.Checked.Value)
				return false;
		}

		// Pressed filter — absent treated as false
		if (options.Pressed.HasValue)
		{
			var isPressed = GetAriaTriStateBoolean(element, "aria-pressed") ?? false;
			if (isPressed != options.Pressed.Value)
				return false;
		}

		// Expanded filter — absent means no match (strict)
		if (options.Expanded.HasValue)
		{
			var isExpanded = GetElementExpanded(element);
			if (isExpanded is null || isExpanded != options.Expanded.Value)
				return false;
		}

		// Disabled filter
		if (options.Disabled.HasValue)
		{
			var isDisabled = IsElementDisabled(element);
			if (isDisabled != options.Disabled.Value)
				return false;
		}

		return true;
	}

	private static AriaRole? GetEffectiveRole(IElement element)
	{
		var explicitRole = element.GetAttribute("role");
		if (!string.IsNullOrEmpty(explicitRole))
		{
			// role attribute can have space-separated values; take the first token
			var firstToken = explicitRole.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0].ToLowerInvariant();
			return ParseRoleString(firstToken);
		}

		return ImplicitRoleMapping.GetImplicitRole(element);
	}

	private static AriaRole? ParseRoleString(string roleString) => roleString switch
	{
		"alert" => AriaRole.Alert,
		"alertdialog" => AriaRole.AlertDialog,
		"application" => AriaRole.Application,
		"article" => AriaRole.Article,
		"banner" => AriaRole.Banner,
		"blockquote" => AriaRole.Blockquote,
		"button" => AriaRole.Button,
		"caption" => AriaRole.Caption,
		"cell" => AriaRole.Cell,
		"checkbox" => AriaRole.Checkbox,
		"code" => AriaRole.Code,
		"columnheader" => AriaRole.ColumnHeader,
		"combobox" => AriaRole.Combobox,
		"complementary" => AriaRole.Complementary,
		"contentinfo" => AriaRole.ContentInfo,
		"definition" => AriaRole.Definition,
		"deletion" => AriaRole.Deletion,
		"dialog" => AriaRole.Dialog,
		"directory" => AriaRole.Directory,
		"document" => AriaRole.Document,
		"emphasis" => AriaRole.Emphasis,
		"feed" => AriaRole.Feed,
		"figure" => AriaRole.Figure,
		"form" => AriaRole.Form,
		"generic" => AriaRole.Generic,
		"grid" => AriaRole.Grid,
		"gridcell" => AriaRole.GridCell,
		"group" => AriaRole.Group,
		"heading" => AriaRole.Heading,
		"img" => AriaRole.Img,
		"insertion" => AriaRole.Insertion,
		"link" => AriaRole.Link,
		"list" => AriaRole.List,
		"listbox" => AriaRole.Listbox,
		"listitem" => AriaRole.Listitem,
		"log" => AriaRole.Log,
		"main" => AriaRole.Main,
		"mark" => AriaRole.Mark,
		"marquee" => AriaRole.Marquee,
		"math" => AriaRole.Math,
		"menu" => AriaRole.Menu,
		"menubar" => AriaRole.Menubar,
		"menuitem" => AriaRole.Menuitem,
		"menuitemcheckbox" => AriaRole.Menuitemcheckbox,
		"menuitemradio" => AriaRole.Menuitemradio,
		"meter" => AriaRole.Meter,
		"navigation" => AriaRole.Navigation,
		"none" => AriaRole.None,
		"note" => AriaRole.Note,
		"option" => AriaRole.Option,
		"paragraph" => AriaRole.Paragraph,
		"presentation" => AriaRole.Presentation,
		"progressbar" => AriaRole.Progressbar,
		"radio" => AriaRole.Radio,
		"radiogroup" => AriaRole.Radiogroup,
		"region" => AriaRole.Region,
		"row" => AriaRole.Row,
		"rowgroup" => AriaRole.Rowgroup,
		"rowheader" => AriaRole.Rowheader,
		"scrollbar" => AriaRole.Scrollbar,
		"search" => AriaRole.Search,
		"searchbox" => AriaRole.Searchbox,
		"separator" => AriaRole.Separator,
		"slider" => AriaRole.Slider,
		"spinbutton" => AriaRole.Spinbutton,
		"status" => AriaRole.Status,
		"strong" => AriaRole.Strong,
		"subscript" => AriaRole.Subscript,
		"superscript" => AriaRole.Superscript,
		"switch" => AriaRole.Switch,
		"tab" => AriaRole.Tab,
		"table" => AriaRole.Table,
		"tablist" => AriaRole.Tablist,
		"tabpanel" => AriaRole.Tabpanel,
		"term" => AriaRole.Term,
		"textbox" => AriaRole.Textbox,
		"time" => AriaRole.Time,
		"timer" => AriaRole.Timer,
		"toolbar" => AriaRole.Toolbar,
		"tooltip" => AriaRole.Tooltip,
		"tree" => AriaRole.Tree,
		"treegrid" => AriaRole.Treegrid,
		"treeitem" => AriaRole.Treeitem,
		_ => null,
	};

	private static bool? GetElementSelected(IElement element)
	{
		var ariaSelected = element.GetAttribute("aria-selected");
		if (ariaSelected is not null)
			return string.Equals(ariaSelected, "true", StringComparison.OrdinalIgnoreCase);

		// Native <option> selected attribute
		if (element.NodeName == "OPTION")
			return element.HasAttribute("selected");

		return null;
	}

	private static bool? GetElementChecked(IElement element)
	{
		var ariaChecked = element.GetAttribute("aria-checked");
		if (ariaChecked is not null)
			return string.Equals(ariaChecked, "true", StringComparison.OrdinalIgnoreCase);

		// Native checkbox/radio checked attribute
		if (element.NodeName == "INPUT")
			return element.HasAttribute("checked");

		return null;
	}

	private static bool? GetAriaTriStateBoolean(IElement element, string attribute)
	{
		var value = element.GetAttribute(attribute);
		if (value is null)
			return null;
		return string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);
	}

	private static bool? GetElementExpanded(IElement element)
	{
		var ariaExpanded = element.GetAttribute("aria-expanded");
		if (ariaExpanded is not null)
			return string.Equals(ariaExpanded, "true", StringComparison.OrdinalIgnoreCase);

		// Native <details> open attribute
		if (element.NodeName == "DETAILS")
			return element.HasAttribute("open");

		return null;
	}

	private static bool IsElementDisabled(IElement element)
	{
		var ariaDisabled = element.GetAttribute("aria-disabled");
		if (ariaDisabled is not null)
			return string.Equals(ariaDisabled, "true", StringComparison.OrdinalIgnoreCase);

		// Native disabled attribute
		if (element.HasAttribute("disabled"))
			return true;

		// Fieldset disabled inheritance
		if (IsDisabledByFieldset(element))
			return true;

		// Disabled optgroup inheritance
		if (element.NodeName == "OPTION" && element.ParentElement is { NodeName: "OPTGROUP" } optgroup && optgroup.HasAttribute("disabled"))
			return true;

		return false;
	}

	private static bool IsDisabledByFieldset(IElement element)
	{
		var parent = element.ParentElement;
		while (parent is not null)
		{
			if (parent.NodeName == "FIELDSET" && parent.HasAttribute("disabled"))
			{
				// Exception: elements in the first <legend> of a disabled fieldset are NOT disabled
				if (!IsInFirstLegend(element, parent))
					return true;
			}

			parent = parent.ParentElement;
		}

		return false;
	}

	private static bool IsInFirstLegend(IElement element, IElement fieldset)
	{
		var firstLegend = fieldset.Children.FirstOrDefault(c => c.NodeName == "LEGEND");
		if (firstLegend is null)
			return false;

		// Check if element is a descendant of the first legend
		var current = element.ParentElement;
		while (current is not null && current != fieldset)
		{
			if (current == firstLegend)
				return true;
			current = current.ParentElement;
		}

		return false;
	}
}
