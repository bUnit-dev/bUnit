using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Bunit.Rendering
{
	/// <summary>
	/// Creates an instance of the <see cref="ShallowRenderContainer"/>.
	/// </summary>
	internal sealed class ShallowRenderContainer : IComponent
	{
		private RenderHandle renderHandle;

		/// <inheritdoc/>
		public void Attach(RenderHandle renderHandle) => this.renderHandle = renderHandle;

		/// <inheritdoc/>
		public Task SetParametersAsync(ParameterView parameters)
		{
			if(parameters.TryGetValue<RenderFragment>("ChildContent", out var childContent))
			{
				renderHandle.Render(childContent);
			}

			return Task.CompletedTask;
		}

		/// <summary>
		/// Wraps the <paramref name="wrappingTarget"/> in a <see cref="ShallowRenderContainer"/>.
		/// </summary>
		public static RenderFragment Wrap(RenderFragment wrappingTarget)
		{
			return builder =>
			{
				builder.OpenComponent<ShallowRenderContainer>(0);
				builder.AddAttribute(1, "ChildContent", wrappingTarget);
				builder.CloseComponent();
			};
		}
	}
}
