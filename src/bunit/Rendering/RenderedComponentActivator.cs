namespace Bunit.Rendering;

/// <summary>
/// Represents a rendered component activator for bUnit.web.
/// </summary>
public sealed class RenderedComponentActivator : IRenderedComponentActivator
{
	private readonly IServiceProvider services;

	/// <summary>
	/// Initializes a new instance of the <see cref="RenderedComponentActivator"/> class.
	/// </summary>
	public RenderedComponentActivator(IServiceProvider services)
	{
		this.services = services;
	}

	/// <inheritdoc/>
	public IRenderedFragment CreateRenderedFragment(int componentId)
		=> new RenderedFragment(componentId, services);

	/// <inheritdoc/>
	public IRenderedComponent<TComponent> CreateRenderedComponent<TComponent>(int componentId)
		where TComponent : IComponent
		=> new RenderedComponent<TComponent>(componentId, services);

	/// <inheritdoc/>
	public IRenderedComponent<TComponent> CreateRenderedComponent<TComponent>(int componentId, TComponent component, RenderTreeFrameDictionary componentFrames)
		where TComponent : IComponent
		=> new RenderedComponent<TComponent>(componentId, component, componentFrames, services);
}
