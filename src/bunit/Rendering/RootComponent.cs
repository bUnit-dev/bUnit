namespace Bunit.Rendering;

/// <summary>
/// Wrapper class that provides access to a <see cref="RenderHandle"/>.
/// </summary>
internal sealed class RootComponent : IComponent
{
	private readonly RenderFragment renderFragment;
	private RenderHandle renderHandle;

	public RootComponent(RenderFragment renderFragment) => this.renderFragment = renderFragment;

	public void Attach(RenderHandle renderHandle) => this.renderHandle = renderHandle;

	public Task SetParametersAsync(ParameterView parameters)
		=> throw new InvalidOperationException($"{nameof(RootComponent)} shouldn't receive any parameters");

	public void Render() => renderHandle.Render(renderFragment);
}
