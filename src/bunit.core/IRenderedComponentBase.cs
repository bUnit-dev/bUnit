using Microsoft.AspNetCore.Components;

namespace Bunit
{
	/// <summary>
	/// Represents a rendered component-under-test.
	/// </summary>
	/// <typeparam name="TComponent">The type of the component under test.</typeparam>
	public interface IRenderedComponentBase<out TComponent> : IRenderedFragmentBase
	    where TComponent : IComponent
	{
		/// <summary>
		/// Gets the component under test.
		/// </summary>
		TComponent Instance { get; }
	}
}
