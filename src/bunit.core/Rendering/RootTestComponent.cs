using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Bunit.Rendering
{
	/// <summary>
	/// Wrapper class that provides access to a <see cref="RenderHandle"/>.
	/// </summary>
	internal class RootTestComponent : IComponent
	{
		private readonly RenderFragment _renderFragment;
		private RenderHandle _renderHandle;

		public RootTestComponent(RenderFragment renderFragment)
			=> _renderFragment = renderFragment;

		public void Attach(RenderHandle renderHandle)
			=> _renderHandle = renderHandle;

		public Task SetParametersAsync(ParameterView parameters)
		{
			_renderHandle.Render(_renderFragment);
			return Task.CompletedTask;
		}
	}
}
