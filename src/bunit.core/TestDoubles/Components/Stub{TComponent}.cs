#if NET5_0_OR_GREATER
using System;
using Bunit.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Bunit.TestDoubles
{
	/// <summary>
	/// Represents a stub of a component of type <typeparamref name="TComponent"/>.
	/// </summary>
	/// <typeparam name="TComponent">The stub type.</typeparam>
	public class Stub<TComponent> : ComponentBase
		where TComponent : IComponent
	{
		/// <inheritdoc/>
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder?.AddMarkupContent(0, $"<{typeof(TComponent).Name}></{typeof(TComponent).Name}>");
		}
	}
}
#endif
