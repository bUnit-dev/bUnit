using AngleSharp.Dom;
using Bunit.Web.AngleSharp;

namespace Bunit;

/// <summary>
/// Extension methods for querying <see cref="IRenderedComponent{TComponent}" /> by ARIA role.
/// </summary>
public static class RoleQueryExtensions
{
	/// <summary>
	/// Returns the first element with the specified ARIA role.
	/// </summary>
	/// <param name="renderedComponent">The rendered fragment to search.</param>
	/// <param name="role">The ARIA role to search for (e.g., AriaRole.Button, AriaRole.Link).</param>
	/// <param name="configureOptions">Method used to override the default behavior of FindByRole.</param>
	/// <returns>The first element matching the specified role and options.</returns>
	/// <exception cref="RoleNotFoundException">Thrown when no element matching the role is found.</exception>
	public static IElement FindByRole(this IRenderedComponent<IComponent> renderedComponent, AriaRole role, Action<ByRoleOptions>? configureOptions = null)
	{
		var options = ByRoleOptions.Default;
		if (configureOptions is not null)
		{
			options = options with { };
			configureOptions.Invoke(options);
		}

		return FindByRoleInternal(renderedComponent, role, options) ?? throw new RoleNotFoundException(role, options.Name);
	}

	/// <summary>
	/// Returns all elements with the specified ARIA role.
	/// </summary>
	/// <param name="renderedComponent">The rendered fragment to search.</param>
	/// <param name="role">The ARIA role to search for (e.g., AriaRole.Button, AriaRole.Link).</param>
	/// <param name="configureOptions">Method used to override the default behavior of FindAllByRole.</param>
	/// <returns>A read-only collection of elements matching the role. Returns an empty collection if no matches are found.</returns>
	public static IReadOnlyList<IElement> FindAllByRole(this IRenderedComponent<IComponent> renderedComponent, AriaRole role, Action<ByRoleOptions>? configureOptions = null)
	{
		ArgumentNullException.ThrowIfNull(renderedComponent);

		var options = ByRoleOptions.Default;
		if (configureOptions is not null)
		{
			options = options with { };
			configureOptions.Invoke(options);
		}

		return FindAllByRoleInternal(renderedComponent, role, options);
	}

	private static IElement? FindByRoleInternal(IRenderedComponent<IComponent> renderedComponent, AriaRole role, ByRoleOptions options)
	{
		var results = FindAllByRoleInternal(renderedComponent, role, options);
		return results.Count > 0 ? results[0] : null;
	}

	private static List<IElement> FindAllByRoleInternal(IRenderedComponent<IComponent> renderedComponent, AriaRole role, ByRoleOptions options)
	{
		var allElements = renderedComponent.Nodes.TryQuerySelectorAll("*");
		var results = new List<IElement>();
		var seen = new HashSet<IElement>();

		foreach (var element in allElements)
		{
			if (!MatchesRole(element, role, options))
				continue;

			// Deduplicate using the underlying element
			var underlyingElement = element.Unwrap();
			if (seen.Add(underlyingElement))
			{
				results.Add(element);
			}
		}

		return results;
	}

	private static bool MatchesRole(IElement element, AriaRole role, ByRoleOptions options)
	{
		if (!options.Hidden && element.IsHiddenFromAccessibilityTree())
			return false;

		var elementRole = element.GetRole();
		if (elementRole != role)
			return false;

		if (options.Name is not null)
		{
			var accessibleName = AccessibleNameComputation.GetAccessibleName(element);
			if (accessibleName is null || !accessibleName.Equals(options.Name, options.ComparisonType))
				return false;
		}

		if (options.Level.HasValue)
		{
			if (role != AriaRole.Heading)
				return false;

			var level = ImplicitRoleMapper.GetHeadingLevel(element);
			if (level != options.Level.Value)
				return false;
		}

		if (options.Checked.HasValue)
		{
			var checkedState = element.GetCheckedState();
			if (checkedState != options.Checked.Value)
				return false;
		}

		if (options.Selected.HasValue)
		{
			var selectedState = element.GetSelectedState();
			if (selectedState != options.Selected.Value)
				return false;
		}

		if (options.Pressed.HasValue)
		{
			var pressedState = element.GetPressedState();
			if (pressedState != options.Pressed.Value)
				return false;
		}

		if (options.Expanded.HasValue)
		{
			var expandedState = element.GetExpandedState();
			if (expandedState != options.Expanded.Value)
				return false;
		}

		if (options.Busy.HasValue)
		{
			var busyState = element.GetBusyState();
			if (busyState != options.Busy.Value)
				return false;
		}

		if (options.Current is not null)
		{
			var currentState = element.GetCurrentState();
			switch (options.Current)
			{
				case bool currentBool when !Equals(currentState, currentBool):
				case string currentString when !currentString.Equals(currentState as string, options.ComparisonType):
					return false;
			}
		}

		return true;
	}
}
