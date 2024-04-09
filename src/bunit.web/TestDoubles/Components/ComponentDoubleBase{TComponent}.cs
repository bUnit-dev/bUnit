#if NET5_0_OR_GREATER
namespace Bunit.TestDoubles;

/// <summary>
/// Represents a component that doubles as another component in the render tree.
/// </summary>
/// <typeparam name="TComponent"></typeparam>
public abstract class ComponentDoubleBase<TComponent> : IComponent
	where TComponent : IComponent
{
	private RenderHandle renderHandle;

	/// <summary>
	/// The type of the doubled component.
	/// </summary>
	protected static readonly Type DoubledType = typeof(TComponent);

	/// <summary>
	/// Gets the parameters that was passed to the <typeparamref name="TComponent"/>
	/// that this stub replaced in the component tree.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)]
	public CapturedParameterView<TComponent> Parameters { get; set; } = CapturedParameterView<TComponent>.Empty;

	/// <inheritdoc/>
	public virtual Task SetParametersAsync(ParameterView parameters)
	{
		Parameters = CapturedParameterView<TComponent>.From(parameters);
		renderHandle.Render(BuildRenderTree);
		return Task.CompletedTask;
	}

	/// <summary>
	/// Override to generate a DOM tree from the doubled component.
	/// </summary>
	/// <param name="builder">A <see cref="RenderTreeBuilder"/> to build DOM tree.</param>
	protected virtual void BuildRenderTree(RenderTreeBuilder builder) { }


	/// <inheritdoc/>
	[SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Justification = "This method is called by the Blazor runtime. Should not be called by inheriting types.")]
	void IComponent.Attach(RenderHandle renderHandle) => this.renderHandle = renderHandle;
}
#endif
