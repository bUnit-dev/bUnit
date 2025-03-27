using AngleSharp.Dom;
using Bunit.Web.AngleSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunit.Roles;

public static class RoleQueryExtensions
{
	private static readonly IReadOnlyDictionary<AriaRole, string[]> ImplicitRoles = new Dictionary<AriaRole, string[]>
	{
		[AriaRole.Button] = ["button"],
		[AriaRole.Listbox] = ["select"],
		[AriaRole.Combobox] = ["select"],
		[AriaRole.Heading] = ["h1", "h2", "h3", "h4", "h5", "h6"],
		[AriaRole.Group] = ["details"],
	};

	public static IElement FindByRole(this IRenderedComponent<IComponent> renderedComponent, AriaRole role)
	{
		var roleString = role.ToString().ToLowerInvariant();

		// First try to find by explicit role
		var element = renderedComponent.Nodes.TryQuerySelector($"[role='{roleString}']");
		if (element is not null)
			return element;

		// Then try to find by implicit role
		if (ImplicitRoles.TryGetValue(role, out var possibleElements))
		{
			foreach (var elementName in possibleElements)
			{
				var elements = renderedComponent.Nodes.TryQuerySelectorAll(elementName);
				foreach (var e in elements)
				{
					// For select elements, check if it's a listbox or combobox
					if (elementName == "select")
					{
						if (role == AriaRole.Listbox && e.HasAttribute("multiple"))
							return e;
						if (role == AriaRole.Combobox && !e.HasAttribute("multiple"))
							return e;
					}
					else
					{
						return e;
					}
				}
			}
		}

		throw new InvalidOperationException($"Unable to find element with role '{roleString}'");
	}
}
