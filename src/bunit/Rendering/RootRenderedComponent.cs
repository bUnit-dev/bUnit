using Bunit.Rendering;

namespace Bunit;

/// <summary>
/// The rendered component of a render tree's root. It owns the markup snapshot that
/// every <see cref="RenderedComponent{TComponent}"/> in the same tree reads its slice from.
/// </summary>
internal sealed class RootRenderedComponent : RenderedComponent<BunitRootComponent>, IRenderedComponentRoot
{
	private RootMarkupSnapshot? snapshot;

	public RootRenderedComponent(
		BunitRenderer renderer,
		int componentId,
		IComponent instance,
		IServiceProvider services)
		: base(renderer, componentId, instance, services, parentComponentState: null)
	{
	}

	/// <inheritdoc/>
	public override IRenderedComponentRoot Root => this;

	/// <inheritdoc/>
	public RootMarkupSnapshot? MarkupSnapshot => Volatile.Read(ref snapshot);

	/// <inheritdoc/>
	public void RegenerateMarkup()
	{
		if (IsDisposed)
			return;

		var html = Htmlizer.GetHtml(ComponentId, OwningRenderer);
		Volatile.Write(ref snapshot, new RootMarkupSnapshot(ComponentId, html, HtmlParser));
	}

	/// <inheritdoc/>
	protected override void Dispose(bool disposing)
	{
		snapshot = null;
		base.Dispose(disposing);
	}
}
