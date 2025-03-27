using AngleSharp.Dom;
using Bunit.Web.AngleSharp;
using Bunit.Roles.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Css.Dom;
using System.Text.RegularExpressions;

namespace Bunit.Roles;

/// <summary>
/// Extension methods for finding elements by ARIA role.
/// </summary>
public static class RoleQueryExtensions
{
	private static readonly IRoleStrategy[] RoleStrategies = new IRoleStrategy[]
	{
		new ExplicitRoleStrategy(),
		new ImplicitRoleStrategy()
	};

	/// <summary>
	/// Finds an element by its ARIA role.
	/// </summary>
	/// <param name="context">The query context.</param>
	/// <param name="role">The ARIA role to find.</param>
	/// <param name="options">Additional options for finding the element.</param>
	/// <returns>The first element that matches the role.</returns>
	/// <exception cref="RoleNotFoundException">Thrown when no element with the specified role is found.</exception>
	public static IElement FindByRole(this IElement context, AriaRole role, FindByRoleOptions? options = null)
	{
		options ??= new FindByRoleOptions();
		var roleString = role.ToString().ToLowerInvariant();

		var explicitRoleElements = context.TryQuerySelectorAll($"[role='{roleString}']");
		var elements = explicitRoleElements.Count > 0
			? explicitRoleElements
			: RoleStrategies.SelectMany(strategy => strategy.FindAll(context, role)).Distinct();

		// Filter by level for headings
		if (options.Level.HasValue && role == AriaRole.Heading)
		{
			elements = elements.Where(el =>
			{
				// Check for explicit aria-level attribute
				if (el.HasAttribute("aria-level") && int.TryParse(el.GetAttribute("aria-level"), out var ariaLevel))
				{
					return ariaLevel == options.Level.Value;
				}

				// Check for implicit heading level (h1-h6)
				if (el.NodeName.StartsWith("H", StringComparison.OrdinalIgnoreCase) &&
					int.TryParse(el.NodeName.Substring(1), out var headingLevel))
				{
					return headingLevel == options.Level.Value;
				}

				return false;
			});
		}

		// Filter by checked state
		if (options.Checked.HasValue)
		{
			elements = elements.Where(el =>
			{
				if (el.HasAttribute("aria-checked"))
				{
					return el.GetAttribute("aria-checked") == options.Checked.Value.ToString().ToLowerInvariant();
				}

				if (el.NodeName.Equals("INPUT", StringComparison.OrdinalIgnoreCase) &&
					el.GetAttribute("type")?.Equals("checkbox", StringComparison.OrdinalIgnoreCase) == true)
				{
					return el.HasAttribute("checked") == options.Checked.Value;
				}

				return false;
			});
		}

		// Filter by selected state
		if (options.Selected.HasValue)
		{
			elements = elements.Where(el =>
			{
				if (el.HasAttribute("aria-selected"))
				{
					return el.GetAttribute("aria-selected") == options.Selected.Value.ToString().ToLowerInvariant();
				}

				if (el.NodeName.Equals("OPTION", StringComparison.OrdinalIgnoreCase))
				{
					return el.HasAttribute("selected") == options.Selected.Value;
				}

				return false;
			});
		}

		// Filter by pressed state
		if (options.Pressed.HasValue)
		{
			elements = elements.Where(el =>
				el.HasAttribute("aria-pressed") &&
				el.GetAttribute("aria-pressed") == options.Pressed.Value.ToString().ToLowerInvariant());
		}

		// Filter by expanded state
		if (options.Expanded.HasValue)
		{
			elements = elements.Where(el =>
				el.HasAttribute("aria-expanded") &&
				el.GetAttribute("aria-expanded") == options.Expanded.Value.ToString().ToLowerInvariant());
		}

		// Filter by name
		if (!string.IsNullOrEmpty(options.Name))
		{
			elements = elements.Where(el =>
			{
				var accessibleName = GetAccessibleName(el);
				if (string.IsNullOrEmpty(accessibleName))
					return false;

				// Check if the name is a regex pattern
				if (options.Name.StartsWith("/") && options.Name.EndsWith("/"))
				{
					var regexPattern = options.Name.Substring(1, options.Name.Length - 2);
					return Regex.IsMatch(accessibleName, regexPattern);
				}

				// Exact match (default)
				if (options.Exact)
					return accessibleName.Equals(options.Name, StringComparison.Ordinal);

				// Partial match
				return accessibleName.Contains(options.Name);
			});
		}

		// Filter by description
		if (!string.IsNullOrEmpty(options.Description))
		{
			elements = elements.Where(el =>
			{
				var description = GetAccessibleDescription(el);
				if (string.IsNullOrEmpty(description))
					return false;
				
				return description.Contains(options.Description);
			});
		}

		// Filter by attributes
		if (options.Attributes != null && options.Attributes.Count > 0)
		{
			elements = elements.Where(el =>
			{
				foreach (var attr in options.Attributes)
				{
					if (!el.HasAttribute(attr.Key) || el.GetAttribute(attr.Key) != attr.Value)
						return false;
				}
				return true;
			});
		}

		// Filter by hidden
		if (!options.Hidden)
		{
			elements = elements.Where(IsAccessible);
		}

		var element = elements.FirstOrDefault();
		if (element == null)
		{
			var availableRoles = GetAvailableRoles(context, options.Hidden);
			throw new RoleNotFoundException(role, options.Name, availableRoles);
		}

		return element;
	}

	private static string GetAccessibleName(IElement element)
	{
		// Check for aria-label
		if (element.HasAttribute("aria-label"))
			return element.GetAttribute("aria-label") ?? string.Empty;

		// Check for aria-labelledby (simplified implementation)
		if (element.HasAttribute("aria-labelledby"))
		{
			var labelledById = element.GetAttribute("aria-labelledby");
			if (!string.IsNullOrEmpty(labelledById))
			{
				var doc = element.Owner;
				var labelElement = doc.GetElementById(labelledById);
				if (labelElement != null)
					return labelElement.TextContent.Trim();
			}
		}

		// Default to text content for roles like button, heading
		var role = element.GetAttribute("role");
		if (role == "button" || role == "heading" || element.NodeName.StartsWith("H", StringComparison.OrdinalIgnoreCase))
			return element.TextContent.Trim();

		// For inputs, check the associated label
		if (element.NodeName.Equals("INPUT", StringComparison.OrdinalIgnoreCase) && element.HasAttribute("id"))
		{
			var id = element.GetAttribute("id");
			var doc = element.Owner;
			var label = doc.QuerySelector($"label[for='{id}']");
			if (label != null)
				return label.TextContent.Trim();
		}

		return element.TextContent.Trim();
	}

	private static string GetAccessibleDescription(IElement element)
	{
		// Check for aria-describedby
		if (element.HasAttribute("aria-describedby"))
		{
			var describedById = element.GetAttribute("aria-describedby");
			if (!string.IsNullOrEmpty(describedById))
			{
				var doc = element.Owner;
				var ids = describedById.Split(' ');
				var descriptions = new List<string>();
				
				foreach (var id in ids)
				{
					var descElement = doc.GetElementById(id);
					if (descElement != null)
					{
						descriptions.Add(descElement.TextContent.Trim());
					}
				}
				
				return string.Join(" ", descriptions);
			}
		}

		return string.Empty;
	}

	private static bool IsAccessible(IElement element)
	{
		// Check if element or any parent has hidden attribute
		var current = element;
		while (current != null)
		{
			// Check for HTML hidden attribute
			if (current.HasAttribute("hidden"))
				return false;

			// Check for aria-hidden="true"
			if (current.HasAttribute("aria-hidden") && current.GetAttribute("aria-hidden") == "true")
				return false;

			// Check for display: none (simplified check)
			var style = current.GetAttribute("style");
			if (!string.IsNullOrEmpty(style) && style.Contains("display: none"))
				return false;

			// Check for visibility: hidden (only on the element itself, not parents)
			if (current == element && !string.IsNullOrEmpty(style) && style.Contains("visibility: hidden"))
				return false;

			current = current.ParentElement;
		}

		return true;
	}

	/// <summary>
	/// Gets a list of available roles in the DOM.
	/// </summary>
	/// <param name="context">The element to search within.</param>
	/// <param name="includeHidden">Whether to include hidden elements.</param>
	/// <returns>A sorted list of available roles.</returns>
	public static IReadOnlyList<string> GetAvailableRoles(IElement context, bool includeHidden = false)
	{
		var roles = new List<string>();

		// Find elements with explicit roles
		var explicitRoleElements = context.TryQuerySelectorAll("[role]");
		foreach (var element in explicitRoleElements)
		{
			if (includeHidden || IsAccessible(element))
			{
				roles.Add(element.GetAttribute("role")?.ToLowerInvariant() ?? string.Empty);
			}
		}

		// Find elements with implicit roles
		var implicitStrategy = new ImplicitRoleStrategy();
		var implicitRoles = implicitStrategy.GetImplicitRoles();
		
		foreach (var role in implicitRoles)
		{
			var roleString = role.ToString().ToLowerInvariant();
			var elements = implicitStrategy.FindAll(context, role);
			
			if (elements.Any(el => includeHidden || IsAccessible(el)))
			{
				roles.Add(roleString);
			}
		}

		return roles.Distinct().OrderBy(r => r).ToList();
	}

	private static IReadOnlyList<IElement> TryQuerySelectorAll(this IElement element, string selector)
	{
		try
		{
			return element.QuerySelectorAll(selector).ToList();
		}
		catch
		{
			return Array.Empty<IElement>();
		}
	}
}
