using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Bunit.Rendering
{
	/// <summary>
	/// Wrapper class that provides access to a <see cref="RenderHandle"/>.
	/// </summary>
	internal sealed class WrapperComponent : IComponent
	{
		private readonly RenderFragment renderFragment;
		private RenderHandle renderHandle;

		public WrapperComponent(RenderFragment renderFragment) => this.renderFragment = renderFragment;

		public void Attach(RenderHandle renderHandle) => this.renderHandle = renderHandle;

		public Task SetParametersAsync(ParameterView parameters) => throw new InvalidOperationException($"WrapperComponent shouldn't receive any parameters");

		public void Render()
		{
			renderHandle.Render(renderFragment);
		}
	}
}
