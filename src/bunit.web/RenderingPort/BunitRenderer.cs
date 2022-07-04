// This file is a port of the Renderer.ts in https://github.dev/dotnet/aspnetcore
// Version ported: https://github.com/dotnet/aspnetcore/blob/8a95ee9b8b1bbd41357cd756375d78cec6936116/src/Components/Web.JS/src/Rendering/Renderer.ts
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using Microsoft.Extensions.Logging;

namespace Bunit.RenderingPort;

/// <summary>
/// 
/// </summary>
public partial class BunitRenderer : Renderer
{
	private readonly ILogger logger;
	private readonly AngleSharpRenderer anglesharpRenderer;
	private Exception? capturedUnhandledException;

	/// <summary>
	/// Gets the number of times the renderer performed a render cycle.
	/// </summary>
	public int RenderCount { get; private set; }

	/// <inheritdoc/>
	public override Dispatcher Dispatcher { get; } = Dispatcher.CreateDefault();

	public BunitRenderer(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
		: base(serviceProvider, loggerFactory)
	{
		logger = loggerFactory.CreateLogger<BunitRenderer>();
		anglesharpRenderer = new AngleSharpRenderer(this);
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
				var result = InitializeRenderedRootComponent(renderFragment);
				_ = RenderRootComponentAsync(result.ComponentId);
				return result;
			});
			Debug.Assert(renderTask.IsCompletedSuccessfully, "There should not be any asynchronous code in the inside the lambda passed to Dispatcher.InvokeAsync.");

			AssertNoUnhandledExceptions();

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
				var result = InitializeRenderedRootComponent(renderFragment);
				await RenderRootComponentAsync(result.ComponentId).ConfigureAwait(false);

				AssertNoUnhandledExceptions();

				return result;
			}).ConfigureAwait(false);
		}
		catch (Exception ex)
		{
			HandleException(ex);
			throw;
		}
	}

	private RenderedComponent<RootComponent> InitializeRenderedRootComponent(RenderFragment renderFragment)
	{
		var component = new RootComponent(renderFragment);
		var componentId = AssignRootComponentId(component);
		return anglesharpRenderer.InitializeRenderedComponent(componentId, component);
	}

	protected override Task UpdateDisplayAsync(in RenderBatch renderBatch)
	{
		RenderCount++;

		var updatedComponentsLength = renderBatch.UpdatedComponents.Count;
		for (var i = 0; i < updatedComponentsLength; i++)
		{
			var diff = renderBatch.UpdatedComponents.Array[i];
			var componentId = diff.ComponentId;
			var edits = diff.Edits;

			anglesharpRenderer.UpdateComponent(
				in renderBatch,
				componentId,
				in edits,
				renderBatch.ReferenceFrames.Array);
		}

		var disposedComponentIdsLength = renderBatch.DisposedComponentIDs.Count;
		for (int i = 0; i < disposedComponentIdsLength; i++)
		{
			var componentId = renderBatch.DisposedComponentIDs.Array[i];
			anglesharpRenderer.DisposeComponent(componentId);
		}

		var disposedEventHandlerIdsLength = renderBatch.DisposedEventHandlerIDs.Count;
		for (int i = 0; i < disposedEventHandlerIdsLength; i++)
		{
			var eventHandlerId = renderBatch.DisposedEventHandlerIDs.Array[i];
			anglesharpRenderer.DisposeEventHandler(eventHandlerId);
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

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		anglesharpRenderer.Dispose();
	}
}
