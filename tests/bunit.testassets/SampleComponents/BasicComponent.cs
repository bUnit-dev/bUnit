using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Bunit.TestAssets.SampleComponents
{
	public class BasicComponent : IComponent
	{
		private RenderHandle renderHandle;

		public void Attach(RenderHandle renderHandle)
		{
			this.renderHandle = renderHandle;
		}

		public Task SetParametersAsync(ParameterView parameters)
		{
			renderHandle.Render(builder => builder.AddMarkupContent(0, nameof(BasicComponent)));
			return Task.CompletedTask;
		}
	}
}
