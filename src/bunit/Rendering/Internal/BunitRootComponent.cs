namespace Bunit.Rendering.Internal;

internal class BunitRootComponent : IComponent
{
	private readonly RenderFragment renderFragment;
	private RenderHandle renderHandle;

	public BunitRootComponent(RenderFragment renderFragment)
		=> this.renderFragment = renderFragment;

	public void Attach(RenderHandle renderHandle) 
	{
		this.renderHandle = renderHandle;
	}

	public Task SetParametersAsync(ParameterView parameters) 
	{
		renderHandle.Render(renderFragment);
		return Task.CompletedTask;
	}
}
