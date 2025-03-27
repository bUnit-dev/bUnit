using AngleSharp.Dom;
using Bunit.Web.AngleSharp;
using Bunit.Roles.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunit.Roles;

public static class RoleQueryExtensions
{
	private static readonly IReadOnlyList<IRoleQueryStrategy> RoleQueryStrategies =
	[
		new ExplicitRoleStrategy(),
		new ImplicitRoleStrategy(),
	];

	public static IElement FindByRole(this IRenderedComponent<IComponent> renderedComponent, AriaRole role)
	{
		foreach (var strategy in RoleQueryStrategies)
		{
			var element = strategy.FindElement(renderedComponent, role);
			if (element is not null)
				return element;
		}

		var availableRoles = GetAvailableRoles(renderedComponent);
		throw new RoleNotFoundException(role, availableRoles);
	}

	private static IReadOnlyList<string> GetAvailableRoles(IRenderedComponent<IComponent> renderedComponent)
	{
		var roles = new HashSet<string>();

		// Get explicit roles
		var elementsWithRole = renderedComponent.Nodes.TryQuerySelectorAll("[role]");
		foreach (var element in elementsWithRole)
		{
			var role = element.GetAttribute("role");
			if (role is not null)
				roles.Add(role);
		}

		// Get implicit roles
		foreach (var strategy in RoleQueryStrategies)
		{
			if (strategy is ImplicitRoleStrategy implicitStrategy)
			{
				foreach (var role in implicitStrategy.GetImplicitRoles())
				{
					var elements = renderedComponent.Nodes.TryQuerySelectorAll(role);
					if (elements.Any())
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
