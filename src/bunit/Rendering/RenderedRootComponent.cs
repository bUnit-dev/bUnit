using System.Diagnostics;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Bunit.Diffing;
using Bunit.Rendering;

namespace Bunit;

/// <summary>
/// Represents a rendered component.
/// </summary>
[DebuggerDisplay("RootComponent: RenderCount={RenderCount}, IsDirty={IsDirty}")]
internal sealed class RenderedRootComponent : ComponentState, IRenderedComponent<BunitRootComponent>
{
	private readonly BunitRenderer renderer;
	private readonly BunitRootComponent instance;
	private readonly BunitHtmlParser htmlParser;
	private string? markup;
	private INodeList? latestRenderNodes;

	public bool IsDirty { get; set; }

	/// <summary>
	/// Gets the component under test.
	/// </summary>
	public BunitRootComponent Instance
	{
		get
		{
			EnsureComponentNotDisposed();
			return instance ?? throw new InvalidOperationException("Component has not rendered yet...");
		}
	}

	/// <summary>
	/// Gets a value indicating whether the rendered component or fragment has been disposed by the <see cref="BunitRenderer"/>.
	/// </summary>
	public bool IsDisposed { get; private set; }

	/// <summary>
	/// Gets the HTML markup from the rendered fragment/component.
	/// </summary>
	public string Markup
	{
		get
		{
			EnsureComponentNotDisposed();

			if (markup is null)
				throw new InvalidOperationException("Component has not rendered!");

			// Volatile read is necessary to ensure the updated markup
			// is available across CPU cores. Without it, the pointer to the
			// markup string can be stored in a CPUs register and not
			// get updated when another CPU changes the string.
			return Volatile.Read(ref markup);
		}
	}

	/// <summary>
	/// Adds or removes an event handler that will be triggered after
	/// each render of this <see cref="RenderedComponent{T}"/>.
	/// </summary>
	public event EventHandler? OnAfterRender;

	/// <summary>
	/// An event that is raised after the markup of the
	/// <see cref="RenderedComponent{T}"/> is updated.
	/// </summary>
	public event EventHandler? OnMarkupUpdated;

	/// <summary>
	/// Gets the total number times the fragment has been through its render life-cycle.
	/// </summary>
	public int RenderCount { get; private set; }

	/// <summary>
	/// Gets the AngleSharp <see cref="INodeList"/> based
	/// on the HTML markup from the rendered fragment/component.
	/// </summary>
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public INodeList Nodes
	{
		get
		{
			EnsureComponentNotDisposed();
			latestRenderNodes ??= htmlParser.Parse(Markup);
			return latestRenderNodes;
		}
	}

	/// <summary>
	/// Gets the <see cref="IServiceProvider"/> used when rendering the component.
	/// </summary>
	public IServiceProvider Services { get; }

	public RenderedRootComponent(
		BunitRenderer renderer,
		int componentId,
		IComponent instance,
		IServiceProvider services,
		ComponentState? parentComponentState)
		: base(renderer, componentId, instance, parentComponentState)
	{
		Services = services;
		this.renderer = renderer;
		this.instance = (BunitRootComponent)instance;
		Debug.Assert(parentComponentState is null, "A root component should not have a parent.");
		//var config = Configuration.Default.WithCss().With(new HtmlComparer());
		//var context = BrowsingContext.New(config);
		//var parseOptions = new HtmlParserOptions
		//{
		//	IsKeepingSourceReferences = true,
		//	IsAcceptingCustomElementsEverywhere = true,
		//	IsStrictMode = false,
		//	OnCreated = (elm, pos) =>
		//	{
		//	},
		//};
		htmlParser = services.GetRequiredService<BunitHtmlParser>(); // new HtmlParser(parseOptions, context);
	}

	/// <inheritdoc/>
	public void Dispose()
	{
		if (IsDisposed)
			return;

		IsDisposed = true;
		markup = string.Empty;
		OnAfterRender = null;
		OnMarkupUpdated = null;
	}

	/// <inheritdoc/>
	public override ValueTask DisposeAsync()
	{
		Dispose();
		return base.DisposeAsync();
	}

	/// <summary>
	/// Called by the owning <see cref="BunitRenderer"/> when it finishes a render.
	/// </summary>
	public void RegenerateMarkup()
	{
		if (IsDisposed)
			return;

		var newMarkup = Htmlizer.GetHtml(ComponentId, renderer);

		// Volatile write is necessary to ensure the updated markup
		// is available across CPU cores. Without it, the pointer to the
		// markup string can be stored in a CPUs register and not
		// get updated when another CPU changes the string.
		Volatile.Write(ref markup, newMarkup);
		latestRenderNodes = null;
		IsDirty = false;
		RenderCount++;
		OnMarkupUpdated?.Invoke(this, EventArgs.Empty);
		OnAfterRender?.Invoke(this, EventArgs.Empty);
	}

	/// <summary>
	/// Ensures that the underlying component behind the
	/// fragment has not been removed from the render tree.
	/// </summary>
	private void EnsureComponentNotDisposed()
	{
		if (IsDisposed)
			throw new ComponentDisposedException(ComponentId);
	}
}
