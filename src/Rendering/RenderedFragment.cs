using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Diffing.Core;
using AngleSharp.Dom;
using Egil.RazorComponents.Testing.Asserting;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Egil.RazorComponents.Testing
{
    /// <summary>
    /// Represents a rendered fragment.
    /// </summary>
    public class RenderedFragment : RenderedFragmentBase
    {
        /// <inheritdoc/>
        protected override int ComponentId => Container.ComponentId;

        /// <inheritdoc/>
        protected override string FirstRenderMarkup { get; }

        /// <summary>
        /// Instantiates a <see cref="RenderedFragment"/> which will render the <paramref name="renderFragment"/> passed to it.
        /// </summary>
        public RenderedFragment(ITestContext testContext, RenderFragment renderFragment)
            : base(testContext, renderFragment)
        {
            FirstRenderMarkup = Markup;
        }
    }
}
