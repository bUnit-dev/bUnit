using AngleSharp.Dom;
using Bunit.Web.AngleSharp;
using Bunit.Roles.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Css.Dom;

namespace Bunit.Roles;

public static class RoleQueryExtensions
{
	private static readonly IReadOnlyList<IRoleQueryStrategy> RoleQueryStrategies =
	[
		new ExplicitRoleStrategy(),
		new ImplicitRoleStrategy(),
	];

	public static IElement FindByRole(this IRenderedComponent<IComponent> renderedComponent, AriaRole role, FindByRoleOptions? options = null)
	{
		options ??= new FindByRoleOptions();

		foreach (var strategy in RoleQueryStrategies)
		{
			var element = strategy.FindElement(renderedComponent, role);
			if (element is not null && (options.Hidden || !IsHidden(element)))
				return element;
		}

		var availableRoles = GetAvailableRoles(renderedComponent, options);
		throw new RoleNotFoundException(role, availableRoles, renderedComponent.Nodes);
	}

	private static bool IsHidden(IElement element)
	{
		return element.HasAttribute("hidden") || 
			   element.GetAttribute("aria-hidden") == "true" ||
			   element.ComputeStyle().GetDisplay() == "none";
	}

	private static IReadOnlyList<string> GetAvailableRoles(IRenderedComponent<IComponent> renderedComponent, FindByRoleOptions options)
	{
		var roles = new HashSet<string>();

		// Get explicit roles
		var elementsWithRole = renderedComponent.Nodes.TryQuerySelectorAll("[role]");
		foreach (var element in elementsWithRole)
		{
			if (options.Hidden || !IsHidden(element))
			{
				var role = element.GetAttribute("role");
				if (role is not null)
					roles.Add(role);
			}
		}

		// Get implicit roles
		foreach (var strategy in RoleQueryStrategies)
		{
			if (strategy is ImplicitRoleStrategy implicitStrategy)
			{
				foreach (var role in implicitStrategy.GetImplicitRoles())
				{
					var elements = renderedComponent.Nodes.TryQuerySelectorAll(role);
					if (elements.Any(e => options.Hidden || !IsHidden(e)))
					{
						// Find the ARIA role that corresponds to this element
						foreach (var (ariaRole, elementNames) in ImplicitRoleStrategy.ImplicitRoles)
						{
							if (elementNames.Contains(role))
							{
								roles.Add(ariaRole.ToString().ToLowerInvariant());
								break;
							}
						}
					}
				}
			}
		}

		return roles.OrderBy(x => x).ToList();
	}
}
