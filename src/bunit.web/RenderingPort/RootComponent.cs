namespace Bunit.RenderingPort;

public sealed class RootComponent : IComponent
{
	private RenderHandle? renderHandle;

	public RenderFragment ChildContent { get; }

	public RootComponent(RenderFragment childContent)
		=> ChildContent = childContent;

	public void Attach(RenderHandle renderHandle)
		=> this.renderHandle = renderHandle;

	public Task SetParametersAsync(ParameterView parameters)
	{
		if (renderHandle.HasValue)
		{
			renderHandle.Value.Render(ChildContent);
		}

		return Task.CompletedTask;
	}

}
