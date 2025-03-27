using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bunit.Roles.Strategies
{
    /// <summary>
    /// Strategy for finding elements with implicit ARIA roles.
    /// </summary>
    public class ImplicitRoleStrategy : IRoleStrategy
    {
        /// <summary>
        /// Dictionary of implicit ARIA roles mapped to their corresponding element selectors.
        /// </summary>
        internal static readonly Dictionary<AriaRole, string[]> ImplicitRoles = new Dictionary<AriaRole, string[]>
        {
            // Basic Structures
            { AriaRole.Button, new[] { "button", "input[type=button]", "input[type=submit]", "input[type=reset]" } },
            { AriaRole.Link, new[] { "a[href]" } },
            { AriaRole.Heading, new[] { "h1", "h2", "h3", "h4", "h5", "h6" } },
            { AriaRole.Img, new[] { "img" } },
            
            // Form Elements
            { AriaRole.Checkbox, new[] { "input[type=checkbox]" } },
            { AriaRole.Radio, new[] { "input[type=radio]" } },
            { AriaRole.TextBox, new[] { "input[type=text]", "input:not([type])", "textarea" } },
            { AriaRole.Combobox, new[] { "select" } },
            { AriaRole.Option, new[] { "option" } },
            
            // Landmarks
            { AriaRole.Banner, new[] { "header:not([role])" } },
            { AriaRole.Navigation, new[] { "nav" } },
            { AriaRole.Main, new[] { "main" } },
            { AriaRole.Complementary, new[] { "aside" } },
            { AriaRole.Contentinfo, new[] { "footer:not([role])" } },
            
            // Lists & Tables
            { AriaRole.List, new[] { "ul", "ol" } },
            { AriaRole.Listitem, new[] { "li" } },
            { AriaRole.Table, new[] { "table" } },
            { AriaRole.Row, new[] { "tr" } },
            { AriaRole.Cell, new[] { "td" } },
            { AriaRole.Rowheader, new[] { "th[scope=row]" } },
            { AriaRole.Columnheader, new[] { "th[scope=col]" } },
            
            // Other Common Elements
            { AriaRole.Article, new[] { "article" } },
            { AriaRole.Code, new[] { "code" } },
            { AriaRole.Separator, new[] { "hr" } },
        };

        /// <summary>
        /// Finds all elements with the specified implicit role.
        /// </summary>
        /// <param name="context">The element to search within.</param>
        /// <param name="role">The ARIA role to find.</param>
        /// <returns>A collection of elements that match the role.</returns>
        public IEnumerable<IElement> FindAll(IElement context, AriaRole role)
        {
            if (!ImplicitRoles.TryGetValue(role, out var selectors))
                return Array.Empty<IElement>();

            var result = new List<IElement>();
            foreach (var selector in selectors)
            {
                try
                {
                    var elements = context.QuerySelectorAll(selector);
                    result.AddRange(elements);
                }
                catch
                {
                    // If a selector fails, continue with the next one
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a list of all the implicit roles defined in this strategy.
        /// </summary>
        /// <returns>A collection of all implicitly defined ARIA roles.</returns>
        public IEnumerable<AriaRole> GetImplicitRoles()
        {
            return ImplicitRoles.Keys;
        }
    }
} 