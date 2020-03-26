using Microsoft.AspNetCore.Components;

namespace Bunit
{
    /// <summary>
    /// Represents a rendered fragment.
    /// </summary>
    public class RenderedFragment : RenderedFragmentBase
    {
        /// <inheritdoc/>
        protected override string FirstRenderMarkup { get; }

        /// <inheritdoc/>
        public override int ComponentId => Container.ComponentId;

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
