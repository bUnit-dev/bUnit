using System.Diagnostics;
using System.Runtime.CompilerServices;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Bunit.RenderingV2.ComponentTree;
using Microsoft.Extensions.Logging;

namespace Bunit.RenderingV2;

public partial class BunitRenderer : Renderer
{
	internal static readonly ConditionalWeakTable<INode, NodeMetadata> NodeMetadata
		= new ConditionalWeakTable<INode, NodeMetadata>();

	private readonly ILogger logger;
	private readonly Dictionary<int, ComponentAdapter> componentAdapters = new();
	private readonly HtmlParser htmlParser;
	private readonly EventHandlerManager eventHandlerManager;
	private TaskCompletionSource<Exception> unhandledExceptionTsc = new();
	private Exception? capturedUnhandledException;

	public int RenderCount { get; private set; }

	public override Dispatcher Dispatcher { get; } = Dispatcher.CreateDefault();

	public Task<Exception> UnhandledException => unhandledExceptionTsc.Task;

	public BunitRenderer(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
		: base(serviceProvider, loggerFactory)
	{
		logger = loggerFactory.CreateLogger<BunitRenderer>();
		htmlParser = new HtmlParser(new HtmlParserOptions
		{
			IsAcceptingCustomElementsEverywhere = false,
			IsKeepingSourceReferences = false,
			IsNotConsumingCharacterReferences = false,
			IsNotSupportingFrames = false,
			IsPreservingAttributeNames = false,
			IsScripting = false,
			IsSupportingProcessingInstructions = false,
			IsEmbedded = true,
			IsStrictMode = false,
		});
		eventHandlerManager = new();
	}

	/// <summary>
	/// Renders the <paramref name="renderFragment"/> inside a <see cref="RootComponent"/>.
	/// This returns as soon all components in the <paramref name="renderFragment"/>
	/// have finished their first render cycle. It does not await any async life cycle methods
	/// in the components. To await life cycle methods, use <see cref="RenderAsync(RenderFragment)"/>.
	/// </summary>
	public IRenderedComponent<RootComponent> Render(RenderFragment renderFragment)
	{
		try
		{
			var renderTask = Dispatcher.InvokeAsync(() =>
			{
				var rc = InitializeRenderedRootComponent(renderFragment);
				_ = RenderRootComponentAsync(rc.ComponentId);
				return rc;
			});
			Debug.Assert(renderTask.IsCompletedSuccessfully, "There should not be any asynchronous code in the inside the lambda passed to Dispatcher.InvokeAsync.");
			return renderTask.Result;
		}
		catch (Exception ex)
		{
			HandleException(ex);
			throw;
		}
	}

	/// <summary>
	/// Renders the <paramref name="renderFragment"/> inside a <see cref="RootComponent"/>.
	/// The returned task completes when all components in the <paramref name="renderFragment"/>
	/// have finished rendering completely. That includes waiting for any async operations
	/// in life-cycle methods of the components in the render tree.
	/// </summary>
	public async Task<IRenderedComponent<RootComponent>> RenderAsync(RenderFragment renderFragment)
	{
		try
		{
			return await Dispatcher.InvokeAsync(async () =>
			{
				var rc = InitializeRenderedRootComponent(renderFragment);
				await RenderRootComponentAsync(rc.ComponentId).ConfigureAwait(false);
				return rc;
			}).ConfigureAwait(false);
		}
		catch (Exception ex)
		{
			HandleException(ex);
			throw;
		}
	}

	private IRenderedComponent<RootComponent> InitializeRenderedRootComponent(RenderFragment renderFragment)
	{
		var component = new RootComponent(renderFragment);
		var componentId = AssignRootComponentId(component);
		var dom = htmlParser.ParseDocument(string.Empty);
		var adapter = new ComponentAdapter(componentId, component, dom.Body!, new NodeSpan(dom.Body!), htmlParser, this, eventHandlerManager);
		componentAdapters[componentId] = adapter;
		return new RenderedComponentV2<RootComponent>(adapter);
	}

	protected override Task UpdateDisplayAsync(in RenderBatch renderBatch)
	{
		RenderCount++;

		var numUpdatedComponents = renderBatch.UpdatedComponents.Count;
		for (var componentIndex = 0; componentIndex < numUpdatedComponents; componentIndex++)
		{
			var updatedComponent = renderBatch.UpdatedComponents.Array[componentIndex];

			if (updatedComponent.Edits.Count > 0
				&& componentAdapters.TryGetValue(updatedComponent.ComponentId, out var adapter))
			{
				adapter.ApplyEdits(updatedComponent, renderBatch, RenderCount);
			}
		}

		var numDisposeEventHandlers = renderBatch.DisposedEventHandlerIDs.Count;
		for (var i = 0; i < numDisposeEventHandlers; i++)
		{
			eventHandlerManager.DisposeHandler(renderBatch.DisposedEventHandlerIDs.Array[i]);
		}

		return Task.CompletedTask;
	}

	public override Task DispatchEventAsync(ulong eventHandlerId, EventFieldInfo? fieldInfo, EventArgs eventArgs)
	{
		return Dispatcher.InvokeAsync(() => base.DispatchEventAsync(eventHandlerId, fieldInfo, eventArgs));
	}

	/// <inheritdoc/>
	protected override void HandleException(Exception exception)
	{
		if (exception is null)
			return;

		LogUnhandledException(logger, exception);

		capturedUnhandledException = exception;

		if (!unhandledExceptionTsc.TrySetResult(capturedUnhandledException))
		{
			unhandledExceptionTsc = new TaskCompletionSource<Exception>();
			unhandledExceptionTsc.SetResult(capturedUnhandledException);
		}
	}

	internal ComponentAdapter CreateComponentAdapter(
		int componentId,
		IComponent component,
		INode parentElement,
		NodeSpan nodeSpan)
	{
		var adapter = new ComponentAdapter(componentId, component, parentElement, nodeSpan, htmlParser, this, eventHandlerManager);
		componentAdapters[componentId] = adapter;
		return adapter;
	}
}
