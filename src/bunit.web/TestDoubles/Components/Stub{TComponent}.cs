using System.Diagnostics;

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
		if (replacement is RenderFragment<CapturedParameterView<TComponent>> replacementTemplate)
			this.replacementTemplate = replacementTemplate;
		else if (replacement is RenderFragment replacementFragment)
			this.replacementFragment = replacementFragment;
		else if (replacement is not null)
			throw new ArgumentException($"The type of replacement is not supported. Replacement type = {replacement.GetType()}", nameof(replacement));
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
