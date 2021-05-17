using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Bunit.Rendering
{
	/// <summary>
	/// Creates an instance of the <see cref="FragmentContainer"/>, which is used
	/// when a fragment is rendered inside a test contexts render tree.
	/// It is primarily used to be able to find the starting point to return.
	/// </summary>
	internal sealed class FragmentContainer : IComponent
	{
		private RenderHandle renderHandle;

		/// <inheritdoc/>
		public void Attach(RenderHandle renderHandle) => this.renderHandle = renderHandle;

		/// <inheritdoc/>
		public Task SetParametersAsync(ParameterView parameters)
		{
			if (parameters.TryGetValue<RenderFragment>("ChildContent", out var childContent))
			{
				renderHandle.Render(childContent);
			}

			return Task.CompletedTask;
		}

		/// <summary>
		/// Wraps the <paramref name="wrappingTarget"/> in a <see cref="FragmentContainer"/>.
		/// </summary>
		public static RenderFragment Wrap(RenderFragment wrappingTarget)
		{
			return builder =>
			{
				builder.OpenComponent<FragmentContainer>(0);
				builder.AddAttribute(1, "ChildContent", wrappingTarget);
				builder.CloseComponent();
			};
		}
	}
}
