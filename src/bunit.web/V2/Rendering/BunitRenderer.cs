using System.Runtime.ExceptionServices;
using Bunit.Rendering;
using Microsoft.Extensions.Logging;

namespace Bunit.V2.Rendering;

public partial class BunitRenderer : Renderer
{
	private readonly ILogger<BunitRenderer> logger;
	private readonly RootComponent root;
	private readonly int rootComponentId;
	private readonly RenderedFragment renderedFragment;
	private readonly BunitHtmlParser htmlParser;
	private Exception? capturedUnhandledException;

	/// <summary>
	/// Gets the number of times the renderer performed a render cycle.
	/// </summary>
	public int RenderCount { get; private set; }

	/// <inheritdoc/>
	public override Dispatcher Dispatcher { get; } = Dispatcher.CreateDefault();

	public BunitRenderer(
		IServiceProvider serviceProvider,
		ILoggerFactory loggerFactory)
		: base(serviceProvider, loggerFactory)
	{
		logger = loggerFactory.CreateLogger<BunitRenderer>();
		renderedFragment = new RenderedFragment(loggerFactory.CreateLogger<RenderedFragment>());
		root = new RootComponent();
		rootComponentId = AssignRootComponentId(root);
		htmlParser = new BunitHtmlParser();
	}

	public async Task<RenderedFragment> RenderAsync(RenderFragment renderFragment)
	{
		root.ChildContent = renderFragment;
		try
		{
			await Dispatcher.InvokeAsync(async () =>
			{
				await RenderRootComponentAsync(rootComponentId)
					.ConfigureAwait(false);
				AssertNoUnhandledExceptions();
			}).ConfigureAwait(false);
		}
		catch (Exception ex)
		{
			HandleException(ex);
			throw;
		}

		return renderedFragment;
	}

	protected override Task UpdateDisplayAsync(in RenderBatch renderBatch)
	{
		LogNewRenderBatchReceived(logger);
		RenderCount++;
		Dispatcher.AssertAccess();
		var frames = new RenderTreeFrameDictionary();
		LoadRenderTreeFrames(rootComponentId, frames);
		UpdateRenderedFragmentState(frames);
		return Task.CompletedTask;
	}

	/// <summary>
	/// Populates the <paramref name="framesCollection"/> with <see cref="ArrayRange{RenderTreeFrame}"/>
	/// starting with the one that belongs to the component with ID <paramref name="componentId"/>.
	/// </summary>
	private void LoadRenderTreeFrames(int componentId, RenderTreeFrameDictionary framesCollection)
	{
		var frames = GetOrLoadRenderTreeFrame(framesCollection, componentId);

		for (var i = 0; i < frames.Count; i++)
		{
			ref var frame = ref frames.Array[i];
			if (frame.FrameType == RenderTreeFrameType.Component)
			{
				LoadRenderTreeFrames(frame.ComponentId, framesCollection);
			}
		}
	}

	/// <summary>
	/// Gets the <see cref="ArrayRange{RenderTreeFrame}"/> from the <paramref name="framesCollection"/>.
	/// If the <paramref name="framesCollection"/> does not contain the frames, they are loaded into it first.
	/// </summary>
	private ArrayRange<RenderTreeFrame> GetOrLoadRenderTreeFrame(RenderTreeFrameDictionary framesCollection, int componentId)
	{
		if (!framesCollection.Contains(componentId))
		{
			var frames = GetCurrentRenderTreeFrames(componentId);
			framesCollection.Add(componentId, frames);
		}

		return framesCollection[componentId];
	}

	private void UpdateRenderedFragmentState(RenderTreeFrameDictionary frames)
	{
		var markup = Htmlizer.GetHtml(rootComponentId, frames);
		renderedFragment.Markup = markup;
		renderedFragment.Nodes = htmlParser.Parse(markup);
		LogChangedComponentsMarkupUpdated(logger);
		renderedFragment.NotifyRenderComplete();
	}

	/// <inheritdoc/>
	public override async Task DispatchEventAsync(ulong eventHandlerId, EventFieldInfo? fieldInfo, EventArgs eventArgs)
	{
		await Dispatcher.InvokeAsync(async () =>
		{
			try
			{
				await base
					.DispatchEventAsync(eventHandlerId, fieldInfo, eventArgs)
					.ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				HandleException(ex);
				throw;
			}
		}).ConfigureAwait(false);

		AssertNoUnhandledExceptions();
	}

	/// <inheritdoc/>
	protected override void HandleException(Exception exception)
	{
		if (exception is null)
		{
			return;
		}

		capturedUnhandledException = exception;
		LogUnhandledException(logger, exception);
	}

	private void AssertNoUnhandledExceptions()
	{
		if (capturedUnhandledException is Exception unhandled)
		{
			capturedUnhandledException = null;

			if (unhandled is AggregateException aggregateException && aggregateException.InnerExceptions.Count == 1)
			{
				ExceptionDispatchInfo.Capture(aggregateException.InnerExceptions[0]).Throw();
			}
			else
			{
				ExceptionDispatchInfo.Capture(unhandled).Throw();
			}
		}
	}
}
