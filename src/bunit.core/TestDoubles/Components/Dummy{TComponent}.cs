#if NET5_0_OR_GREATER
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Bunit.TestDoubles
{
	/// <summary>
	/// Represents a test double dummy of a component of type <typeparamref name="TComponent"/>.
	/// </summary>
	/// <typeparam name="TComponent">The stub type.</typeparam>
	public sealed class Dummy<TComponent> : IComponent
		where TComponent : IComponent
	{
		/// <summary>
		/// Gets the parameters that was passed to the <typeparamref name="TComponent"/>
		/// that this stub replaced in the component tree.
		/// </summary>
		[Parameter(CaptureUnmatchedValues = true)]
		public IReadOnlyDictionary<string, object> Parameters { get; private set; } = ImmutableDictionary<string, object>.Empty;

		/// <inheritdoc/>
		void IComponent.Attach(RenderHandle renderHandle) { }

		/// <inheritdoc/>
		Task IComponent.SetParametersAsync(ParameterView parameters)
		{
			Parameters = parameters.ToDictionary();
			return Task.CompletedTask;
		}
	}
}
#endif
