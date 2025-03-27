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

		throw new InvalidOperationException($"Unable to find element with role '{role.ToString().ToLowerInvariant()}'");
	}
}
