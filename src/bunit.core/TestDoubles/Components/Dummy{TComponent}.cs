#if NET5_0_OR_GREATER
using Microsoft.AspNetCore.Components;

namespace Bunit.TestDoubles
{
	/// <summary>
	/// Represents a test double dummy of a component of type <typeparamref name="TComponent"/>.
	/// </summary>
	/// <typeparam name="TComponent">The stub type.</typeparam>
	public sealed class Dummy<TComponent> : ComponentDoubleBase<TComponent>
		where TComponent : IComponent
	{
		/// <inheritdoc/>
		public override string ToString() => $"Dummy<{DoubledType.Name}>";
	}
}
#endif
