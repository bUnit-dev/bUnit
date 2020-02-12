using Microsoft.AspNetCore.Components;

namespace Bunit
{
    /// <summary>
    /// Represents a component that can be added inside a <see cref="Fixture"/>, whose content
    /// can be accessed in Razor-based test.
    /// </summary>
    public class Fragment : FragmentBase
    {
        /// <summary>
        /// Gets or sets the id of the fragment. The <see cref="Id"/> can be used to retrieve 
        /// the fragment from a <see cref="IRazorTestContext.GetFragment(string)"/>.
        /// </summary>
        [Parameter] public string Id { get; set; } = string.Empty;
    }
}
