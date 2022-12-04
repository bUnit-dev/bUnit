namespace Bunit.V2.Rendering;

internal sealed class RootComponent : IComponent
{
	private RenderHandle? renderHandle;

	internal RenderFragment? ChildContent { get; set; }

	public void Attach(RenderHandle renderHandle)
		=> this.renderHandle = renderHandle;

	public Task SetParametersAsync(ParameterView parameters)
	{
		if (renderHandle.HasValue && ChildContent is not null)
		{
			renderHandle.Value.Render(ChildContent);
		}

		return Task.CompletedTask;
	}
}
