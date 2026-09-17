using AngleSharp.Dom;

namespace Bunit;

/// <summary>
/// Provides helper methods for checking element state related to ARIA roles.
/// </summary>
internal static class RoleElementExtensions
{
	private const string AriaHiddenTrue = "true";

	/// <summary>
	/// Determines if an element is hidden from the accessibility tree.
	/// </summary>
	public static bool IsHiddenFromAccessibilityTree(this IElement element)
	{
		return HasAriaHiddenTrue(element)
		       || element.HasAttribute("hidden")
		       || HasHiddenAncestor(element);
	}

	/// <summary>
	/// Gets the checked state of an element.
	/// </summary>
	public static bool? GetCheckedState(this IElement element)
	{
		var ariaChecked = element.GetAttribute("aria-checked");
		return !string.IsNullOrWhiteSpace(ariaChecked)
			? ParseTriStateBoolean(ariaChecked)
			: GetNativeCheckedStateForCheckboxOrRadio(element);
	}

	/// <summary>
	/// Gets the selected state of an element.
	/// </summary>
	public static bool? GetSelectedState(this IElement element)
	{
		var ariaSelected = element.GetAttribute("aria-selected");
		return !string.IsNullOrWhiteSpace(ariaSelected)
			? ParseBooleanAttribute(ariaSelected)
			: GetNativeSelectedStateForOption(element);
	}

	/// <summary>
	/// Gets the pressed state of an element.
	/// </summary>
	public static bool? GetPressedState(this IElement element)
	{
		var ariaPressed = element.GetAttribute("aria-pressed");
		return string.IsNullOrWhiteSpace(ariaPressed)
			? null
			: ParseTriStateBoolean(ariaPressed);
	}

	/// <summary>
	/// Gets the expanded state of an element.
	/// </summary>
	public static bool? GetExpandedState(this IElement element)
	{
		var ariaExpanded = element.GetAttribute("aria-expanded");
		return !string.IsNullOrWhiteSpace(ariaExpanded)
			? ParseBooleanAttribute(ariaExpanded)
			: GetNativeExpandedStateForDetails(element);
	}

	/// <summary>
	/// Gets the busy state of an element.
	/// </summary>
	public static bool? GetBusyState(this IElement element)
	{
		var ariaBusy = element.GetAttribute("aria-busy");
		return string.IsNullOrWhiteSpace(ariaBusy) ? null : ParseBooleanAttribute(ariaBusy);
	}

	/// <summary>
	/// Gets the current state of an element.
	/// </summary>
	public static object? GetCurrentState(this IElement element)
	{
		var ariaCurrent = element.GetAttribute("aria-current");
		if (string.IsNullOrWhiteSpace(ariaCurrent))
		{
			return null;
		}

		return ariaCurrent.ToUpperInvariant() switch
		{
			"TRUE" => true,
			"FALSE" => false,
			_ => ariaCurrent
		};
	}

	/// <summary>
	/// Gets the explicit role of an element from the role attribute.
	/// </summary>
	public static AriaRole? GetExplicitRole(this IElement element)
	{
		var role = element.GetAttribute("role");
		if (string.IsNullOrWhiteSpace(role))
		{
			return null;
		}

		var firstRole = role.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
		if (string.IsNullOrWhiteSpace(firstRole))
		{
			return null;
		}

		return Enum.TryParse<AriaRole>(firstRole, ignoreCase: true, out var ariaRole)
			? ariaRole
			: null;
	}

	/// <summary>
	/// Gets the role of an element, checking explicit role first, then implicit role.
	/// </summary>
	public static AriaRole? GetRole(this IElement element)
	{
		return element.GetExplicitRole() ?? ImplicitRoleMapper.GetImplicitRole(element);
	}

	private static bool HasAriaHiddenTrue(IElement element) =>
		element.GetAttribute("aria-hidden") == AriaHiddenTrue;

	private static bool HasHiddenAncestor(IElement element)
	{
		var parent = element.ParentElement;
		while (parent != null)
		{
			if (parent.GetAttribute("aria-hidden") == AriaHiddenTrue || parent.HasAttribute("hidden"))
			{
				return true;
			}

			parent = parent.ParentElement;
		}

		return false;
	}

	private static bool? ParseTriStateBoolean(string value)
	{
		return value.ToUpperInvariant() switch
		{
			"TRUE" => true,
			"FALSE" => false,
			"MIXED" => null,
			_ => null
		};
	}

	private static bool? ParseBooleanAttribute(string value)
	{
		return value.ToUpperInvariant() switch
		{
			"TRUE" => true,
			"FALSE" => false,
			_ => null
		};
	}

	private static bool? GetNativeCheckedStateForCheckboxOrRadio(IElement element)
	{
		if (!element.TagName.Equals("INPUT", StringComparison.OrdinalIgnoreCase))
		{
			return null;
		}

		var type = element.GetAttribute("type")?.ToUpperInvariant();
		if (type is not ("CHECKBOX" or "RADIO"))
		{
			return null;
		}

		return element.HasAttribute("checked");
	}

	private static bool? GetNativeSelectedStateForOption(IElement element)
	{
		return !element.TagName.Equals("OPTION", StringComparison.OrdinalIgnoreCase)
			? null
			: element.HasAttribute("selected");
	}

	private static bool? GetNativeExpandedStateForDetails(IElement element)
	{
		return !element.TagName.Equals("DETAILS", StringComparison.OrdinalIgnoreCase)
			? null
			: element.HasAttribute("open");
	}
}
