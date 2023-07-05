using System.Text;

namespace Bunit.Rendering.Internal;

internal abstract class BunitComponentState : ComponentState
{
	private IRenderedFragment? renderedComponent;
	private List<BunitChildComponentState>? children;

	protected internal int MarkupStart { get; set; }

	protected internal int MarkupLength { get; set; }

	public IReadOnlyList<BunitChildComponentState> Children
		=> children is null
		? Array.Empty<BunitChildComponentState>()
		: children.AsReadOnly();

	public abstract ReadOnlySpan<char> Markup { get; }

	protected BunitComponentState(BunitRendererV2 renderer, int componentId, IComponent component, BunitComponentState? parentComponentState)
		: base(renderer, componentId, component, parentComponentState)
	{
	}

	public override ValueTask DisposeAsync()
	{
		renderedComponent?.Dispose();
		renderedComponent = null;
		return base.DisposeAsync();
	}

	internal abstract void UpdateMarkup(int version);

	internal RenderedComponentV2<TComponent> GetRenderedComponent<TComponent>()
		where TComponent : IComponent
	{
		renderedComponent ??= new RenderedComponentV2<TComponent>(this);
		return (RenderedComponentV2<TComponent>)renderedComponent;
	}

	internal void AddChild(BunitChildComponentState child)
	{
		if (children is null)
		{
			children = new();
		}

		children.Add(child);
	}

	internal void RemoveChild(BunitChildComponentState child) => children?.Remove(child);
}

internal class BunitRootComponentState : BunitComponentState
{
	private readonly BunitRendererV2 renderer;
	private readonly StringBuilder markupBuilder = new();
	private int version;
	private string markup;

	internal string? ClosestSelectValueAsString { get; set; }

	public override ReadOnlySpan<char> Markup => markup.AsSpan();

	public BunitRootComponentState(BunitRendererV2 renderer, int componentId, BunitRootComponent component)
		: base(renderer, componentId, component, null)
	{
		this.renderer = renderer;
		markup = string.Empty;
	}

	internal override void UpdateMarkup(int version)
	{
		if (this.version >= version)
		{
			return;
		}

		this.version = version;

		markupBuilder.Clear();
		HtmlizerV2.GenerateMarkup(this);
		markup = markupBuilder.ToString();
		MarkupLength = markup.Length;
	}

	internal void Append(char value)
		=> markupBuilder.Append(value);

	internal void Append(string value)
		=> markupBuilder.Append(value);

	internal void Append(ReadOnlySpan<char> value)
		=> markupBuilder.Append(value);

	internal ArrayRange<RenderTreeFrame> GetRenderTreeFrames(int componentId)
		=> renderer.GetCurrentRenderTreeFrames(componentId);

	internal void MarkComponentStart(int componentId)
		=> renderer
		.GetComponentState(componentId)
		.MarkupStart = markupBuilder.Length;

	internal void MarkComponentStop(int componentId)
	{
		var childComponentState = renderer.GetComponentState(componentId);
		childComponentState.MarkupLength = markupBuilder.Length - childComponentState.MarkupStart;
	}
}

internal class BunitChildComponentState : BunitComponentState
{
	private readonly BunitRootComponentState rootComponent;
	private readonly BunitComponentState parentComponentState;

	public override ReadOnlySpan<char> Markup
		=> rootComponent.Markup.Slice(MarkupStart, MarkupLength);

	public BunitChildComponentState(BunitRendererV2 renderer, int componentId, IComponent component, BunitComponentState parentComponentState)
		: base(renderer, componentId, component, parentComponentState)
	{
		this.rootComponent = parentComponentState is BunitChildComponentState childComponentState
			? childComponentState.rootComponent
			: (BunitRootComponentState)parentComponentState;
		this.parentComponentState = parentComponentState;
		parentComponentState.AddChild(this);
	}

	public override ValueTask DisposeAsync()
	{
		parentComponentState.RemoveChild(this);
		return base.DisposeAsync();
	}

	internal override void UpdateMarkup(int version)
	{
		rootComponent.UpdateMarkup(version);
	}
}
