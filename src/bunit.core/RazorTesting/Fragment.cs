using Bunit.RazorTesting;

using Microsoft.AspNetCore.Components;

namespace Bunit
{
	/// <summary>
	/// Represents a component that can be added inside a fixture, whose content
	/// can be accessed in Razor-based test.
	/// </summary>
	public class Fragment : FragmentBase
	{
		/// <summary>
		/// Gets or sets the id of the fragment.
		/// </summary>
		[Parameter] public string Id { get; set; } = string.Empty;
	}
}
