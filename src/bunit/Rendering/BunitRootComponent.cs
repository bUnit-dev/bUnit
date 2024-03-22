namespace Bunit.Rendering;

/// <summary>
/// Creates an instance of the <see cref="BunitRootComponent"/>, which is used
/// when a fragment is rendered inside a test contexts render tree.
/// It is primarily used to be able to find the starting point to return.
/// </summary>
public sealed class BunitRootComponent : IComponent
{
	private RenderHandle renderHandle;

	/// <inheritdoc/>
	public void Attach(RenderHandle renderHandle) => this.renderHandle = renderHandle;

	/// <inheritdoc/>
	public Task SetParametersAsync(ParameterView parameters)
	{
		if (parameters.TryGetValue<RenderFragment>("ChildContent", out var childContent))
		{
			renderHandle.Render(childContent);
		}

		return Task.CompletedTask;
	}

	/// <summary>
	/// Wraps the <paramref name="wrappingTarget"/> in a <see cref="BunitRootComponent"/>.
	/// </summary>
	public static RenderFragment Wrap(RenderFragment wrappingTarget)
	{
		return builder =>
		{
			builder.OpenComponent<BunitRootComponent>(0);
			builder.AddAttribute(1, "ChildContent", wrappingTarget);
			builder.CloseComponent();
		};
	}
}
