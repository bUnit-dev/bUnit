#if NET5_0_OR_GREATER
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Bunit.TestDoubles
{
	/// <summary>
	/// Represents a stub of a component of type <typeparamref name="TComponent"/>.
	/// </summary>
	/// <typeparam name="TComponent">The stub type.</typeparam>
	public sealed class Stub<TComponent> : IComponent
		where TComponent : IComponent
	{
		private RenderHandle renderHandle;

		/// <summary>
		/// Gets the parameters that was passed to the <typeparamref name="TComponent"/>
		/// that this stub replaced in the component tree.
		/// </summary>
		[Parameter(CaptureUnmatchedValues = true)]
		public IReadOnlyDictionary<string, object> Parameters { get; private set; } = ImmutableDictionary<string, object>.Empty;

		/// <inheritdoc/>
		void IComponent.Attach(RenderHandle renderHandle) => this.renderHandle = renderHandle;

		/// <inheritdoc/>
		Task IComponent.SetParametersAsync(ParameterView parameters)
		{
			Parameters = parameters.ToDictionary();
			renderHandle.Render(BuildRenderTree);
			return Task.CompletedTask;
		}

		private void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder?.AddMarkupContent(0, $"<{typeof(TComponent).Name}></{typeof(TComponent).Name}>");
		}
	}
}
#endif
