namespace Bunit;

public partial class BunitContext
{
	/// <summary>
	/// Use <see cref="Render{TComponent}(System.Action{Bunit.ComponentParameterCollectionBuilder{TComponent}}?)"/> instead.
	/// </summary>
	[Obsolete($"Use {nameof(Render)} instead.", true, UrlFormat = "https://bunit.dev/docs/migration")]
	public IRenderedComponent<TComponent> RenderComponent<TComponent>()
		where TComponent : IComponent
	{
		throw new NotSupportedException($"Use {nameof(Render)}<{typeof(TComponent).Name}> instead.");
	}

	/// <summary>
	/// Use <see cref="Render{TComponent}(System.Action{Bunit.ComponentParameterCollectionBuilder{TComponent}}?)"/> instead.
	/// </summary>
	[Obsolete($"Use {nameof(Render)} instead.", true, UrlFormat = "https://bunit.dev/docs/migration")]
	public IRenderedComponent<TComponent> RenderComponent<TComponent>(Action<ComponentParameterCollectionBuilder<TComponent>>? parameterBuilder)
		where TComponent : IComponent
	{
		throw new NotSupportedException($"Use {nameof(Render)}<{typeof(TComponent).Name}> instead.");
	}
}
