using System.Diagnostics;

namespace Bunit.TestDoubles;

/// <summary>
/// Represents a test double stub of a component of type <typeparamref name="TComponent"/>.
/// </summary>
/// <typeparam name="TComponent">The stub type.</typeparam>
public sealed class Stub<TComponent> : ComponentDoubleBase<TComponent>
	where TComponent : IComponent
{
	private readonly RenderFragment<IReadOnlyDictionary<string, object>>? replacementTemplate;

	/// <summary>
	/// Initializes a new instance of the <see cref="Stub{TComponent}"/> class.
	/// </summary>
	public Stub() { }

	/// <summary>
	/// Initializes a new instance of the <see cref="Stub{TComponent}"/> class
	/// that uses the provided <paramref name="replacementTemplate"/> when rendering.
	/// </summary>
	/// <param name="replacementTemplate">The replacement template to use when rendering.</param>
	public Stub(RenderFragment<IReadOnlyDictionary<string, object>> replacementTemplate)
	{
		this.replacementTemplate = replacementTemplate;
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
	}
}
