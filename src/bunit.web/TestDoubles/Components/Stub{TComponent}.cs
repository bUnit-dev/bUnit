#if NET5_0_OR_GREATER
using System.Diagnostics;
using System.Linq.Expressions;

namespace Bunit.TestDoubles;

/// <summary>
/// Represents a test double stub of a component of type <typeparamref name="TComponent"/>.
/// </summary>
/// <typeparam name="TComponent">The stub type.</typeparam>
public sealed class Stub<TComponent> : ComponentDoubleBase<TComponent>
	where TComponent : IComponent
{
	private readonly RenderFragment<CapturedParameterView<TComponent>>? replacementTemplate;
	private readonly RenderFragment? replacementFragment;

	/// <summary>
	/// Initializes a new instance of the <see cref="Stub{TComponent}"/> class.
	/// </summary>
	public Stub() { }

	/// <summary>
	/// Initializes a new instance of the <see cref="Stub{TComponent}"/> class
	/// that uses the provided <paramref name="replacement"/> when rendering.
	/// </summary>
	/// <param name="replacement">The replacement template to use when rendering.</param>
	public Stub(object? replacement)
	{
		if (replacement is RenderFragment<CapturedParameterView<TComponent>> replacementRenderTemplate)
			replacementTemplate = replacementRenderTemplate;
		else if (replacement is RenderFragment replacementRenderFragment)
			replacementFragment = replacementRenderFragment;
		else if (replacement is not null)
			throw new ArgumentException($"The type of replacement is not supported. Replacement type = {replacement.GetType()}", nameof(replacement));
	}

	/// <summary>
	/// Triggers an event callback from the stub.
	/// </summary>
	/// <param name="selector">Event to trigger.</param>
	public Task InvokeEventCallback(Expression<Func<TComponent, EventCallback>> selector)
	{
		var callback = Parameters.Get(selector);
		return InvokeAsync(callback.InvokeAsync);
	}

	/// <summary>
	/// Triggers an event callback from the stub.
	/// </summary>
	/// <param name="selector">Event to trigger.</param>
	/// <param name="value">Value to pass to the event callback.</param>
	public Task InvokeEventCallback<T>(Expression<Func<TComponent, EventCallback<T>>> selector, T? value)
	{
		var callback = Parameters.Get(selector);
		return InvokeAsync(() => callback.InvokeAsync(value));
	}

	/// <inheritdoc/>
	public override string ToString() => $"Stub<{DoubledType.Name}>";

	/// <inheritdoc/>
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		Debug.Assert(builder is not null);

		if (replacementTemplate is not null)
		{
			builder.AddContent(0, replacementTemplate(Parameters));
		}
		else if (replacementFragment is not null)
		{
			builder.AddContent(0, replacementFragment);
		}
	}
}
#endif
