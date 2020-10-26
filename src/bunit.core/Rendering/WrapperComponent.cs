using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Bunit.Rendering
{
	/// <summary>
	/// Wrapper class that provides access to a <see cref="RenderHandle"/>.
	/// </summary>
	internal class WrapperComponent : IComponent
	{
		private readonly RenderFragment _renderFragment;
		private RenderHandle _renderHandle;

		public WrapperComponent(RenderFragment renderFragment) => _renderFragment = renderFragment;

		public void Attach(RenderHandle renderHandle) => _renderHandle = renderHandle;

		public Task SetParametersAsync(ParameterView parameters) => throw new InvalidOperationException($"WrapperComponent shouldn't receive any parameters");

		public void Render()
		{
			_renderHandle.Render(_renderFragment);
		}
	}
}
