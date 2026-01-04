namespace Bunit;

[RemovedInFutureVersion("Obsolete in v2, removed in future version")]
public partial class BunitContext
{
	/// <summary>
	/// Use <see cref="Render{TComponent}(System.Action{Bunit.ComponentParameterCollectionBuilder{TComponent}}?)"/> instead.
	/// </summary>
	[Obsolete($"Use {nameof(Render)} instead.", true, UrlFormat = "https://bunit.dev/docs/migrations")]
	public IRenderedComponent<TComponent> RenderComponent<TComponent>()
		where TComponent : IComponent
	{
		throw new NotSupportedException($"Use {nameof(Render)}<{typeof(TComponent).Name}> instead.");
	}

	/// <summary>
	/// Use <see cref="Render{TComponent}(System.Action{Bunit.ComponentParameterCollectionBuilder{TComponent}}?)"/> instead.
	/// </summary>
	[Obsolete($"Use {nameof(Render)} instead.", true, UrlFormat = "https://bunit.dev/docs/migrations")]
	public IRenderedComponent<TComponent> RenderComponent<TComponent>(Action<ComponentParameterCollectionBuilder<TComponent>>? parameterBuilder)
		where TComponent : IComponent
	{
		throw new NotSupportedException($"Use {nameof(Render)}<{typeof(TComponent).Name}> instead.");
	}
}
