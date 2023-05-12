using Microsoft.Extensions.Logging;
using System.Runtime.ExceptionServices;

namespace Bunit.Rendering;

/// <summary>
/// Represents a bUnit <see cref="Renderer"/> used to render Blazor components and fragments during bUnit tests.
/// </summary>
public sealed class BunitRenderer : Renderer
{
	private readonly Dictionary<int, IRenderedFragment> renderedComponents = new();
	private readonly List<int> rootComponentIds = new();
	private readonly ILogger<BunitRenderer> logger;
	private readonly TestServiceProvider services;
	private readonly ManualResetEventSlim renderBlocker = new(initialState: true);
	private readonly object renderCycleLock = new();
	private TaskCompletionSource<Exception> unhandledExceptionTsc = new(TaskCreationOptions.RunContinuationsAsynchronously);
	private Exception? capturedUnhandledException;
	private bool disposed;
	private bool blockRenderer;

	/// <summary>
	/// Gets a <see cref="Task{Exception}"/>, which completes when an unhandled exception
	/// is thrown during the rendering of a component, that is caught by the renderer.
	/// </summary>
	public Task<Exception> UnhandledException => unhandledExceptionTsc.Task;

	/// <inheritdoc/>
	public override Dispatcher Dispatcher { get; } = Dispatcher.CreateDefault();

	/// <summary>
	/// Gets the number of render cycles that has been performed.
	/// </summary>
	internal int RenderCount { get; private set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="BunitRenderer"/> class.
	/// </summary>
	public BunitRenderer(TestServiceProvider services, ILoggerFactory loggerFactory)
		: base(services, loggerFactory)
	{
		logger = loggerFactory.CreateLogger<BunitRenderer>();
		ElementReferenceContext = new WebElementReferenceContext(services.GetRequiredService<IJSRuntime>());
		this.services = services;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BunitRenderer"/> class.
	/// </summary>
	public BunitRenderer(TestServiceProvider services, ILoggerFactory loggerFactory, IComponentActivator componentActivator)
		: base(services, loggerFactory, componentActivator)
	{
		logger = loggerFactory.CreateLogger<BunitRenderer>();
		ElementReferenceContext = new WebElementReferenceContext(services.GetRequiredService<IJSRuntime>());
		this.services = services;
	}

	/// <summary>
	/// Renders the <paramref name="renderFragment"/>.
	/// </summary>
	/// <param name="renderFragment">The <see cref="Microsoft.AspNetCore.Components.RenderFragment"/> to render.</param>
	/// <returns>A <see cref="IRenderedFragment"/> that provides access to the rendered <paramref name="renderFragment"/>.</returns>
	public IRenderedFragment RenderFragment(RenderFragment renderFragment)
		=> Render(renderFragment);

	/// <summary>
	/// Notifies the renderer that an event has occurred.
	/// </summary>
	/// <param name="eventHandlerId">The <see cref="RenderTreeFrame.AttributeEventHandlerId"/> value from the original event attribute.</param>
	/// <param name="fieldInfo">Information that the renderer can use to update the state of the existing render tree to match the UI.</param>
	/// <param name="eventArgs">Arguments to be passed to the event handler.</param>
	/// <returns>A <see cref="Task"/> which will complete once all asynchronous processing related to the event has completed.</returns>
	public override Task DispatchEventAsync(
		ulong eventHandlerId,
		EventFieldInfo? fieldInfo,
		EventArgs eventArgs) => DispatchEventAsync(eventHandlerId, fieldInfo, eventArgs, ignoreUnknownEventHandlers: false);

	/// <summary>
	/// Notifies the renderer that an event has occurred.
	/// </summary>
	/// <param name="eventHandlerId">The <see cref="RenderTreeFrame.AttributeEventHandlerId"/> value from the original event attribute.</param>
	/// <param name="fieldInfo">Information that the renderer can use to update the state of the existing render tree to match the UI.</param>
	/// <param name="eventArgs">Arguments to be passed to the event handler.</param>
	/// <param name="ignoreUnknownEventHandlers">Set to true to ignore the <see cref="UnknownEventHandlerIdException"/>.</param>
	/// <returns>A <see cref="Task"/> which will complete once all asynchronous processing related to the event has completed.</returns>
	public new Task DispatchEventAsync(
		ulong eventHandlerId,
		EventFieldInfo? fieldInfo,
		EventArgs eventArgs,
		bool ignoreUnknownEventHandlers)
	{
		ArgumentNullException.ThrowIfNull(fieldInfo);

		ObjectDisposedException.ThrowIf(disposed, this);

		var result = Dispatcher.InvokeAsync(() =>
		{
			UnblockRendering();
			ResetUnhandledException();

			try
			{
				return base.DispatchEventAsync(eventHandlerId, fieldInfo, eventArgs);
			}
			catch (ArgumentException ex) when (string.Equals(ex.Message,
				                                   $"There is no event handler associated with this event. EventId: '{eventHandlerId}'. (Parameter 'eventHandlerId')",
				                                   StringComparison.Ordinal))
			{
				if (ignoreUnknownEventHandlers)
				{
					return Task.CompletedTask;
				}

				var betterExceptionMsg = new UnknownEventHandlerIdException(eventHandlerId, fieldInfo, ex);
				return Task.FromException(betterExceptionMsg);
			}
			finally
			{
				BlockRendering();
			}
		});

		if (result.IsFaulted && result.Exception is not null)
		{
			HandleException(result.Exception);
		}

		AssertNoUnhandledExceptions();

		return result;
	}

	/// <summary>
	/// Performs a depth-first search for the first <typeparamref name="TComponent"/> child component of the <paramref name="parentComponent"/>.
	/// </summary>
	/// <typeparam name="TComponent">Type of component to find.</typeparam>
	/// <param name="parentComponent">Parent component to search.</param>
	public IRenderedComponent<TComponent> FindComponent<TComponent>(IRenderedFragment parentComponent)
		where TComponent : IComponent
		=> FindComponentsInternal<TComponent>(parentComponent).FirstOrDefault() ?? throw new ComponentNotFoundException(typeof(TComponent));

	/// <summary>
	/// Performs a depth-first search for all <typeparamref name="TComponent"/> child components of the <paramref name="parentComponent"/>.
	/// </summary>
	/// <typeparam name="TComponent">Type of components to find.</typeparam>
	/// <param name="parentComponent">Parent component to search.</param>
	public IReadOnlyList<IRenderedComponent<TComponent>> FindComponents<TComponent>(IRenderedFragment parentComponent)
		where TComponent : IComponent
		=> FindComponentsInternal<TComponent>(parentComponent).ToList();

	/// <summary>
	/// Disposes all components rendered by the <see cref="BunitRenderer" />.
	/// </summary>
	public void DisposeComponents()
	{
		ObjectDisposedException.ThrowIf(disposed, this);

		// The dispatcher will always return a completed task,
		// when dealing with an IAsyncDisposable.
		// When the component is an IDisposable, the task will be completed
		// Therefore we don't have to wait for the task to complete.
		Dispatcher.InvokeAsync(() =>
		{
			ResetUnhandledException();

			UnblockRendering();
			foreach (var root in rootComponentIds)
			{
				RemoveRootComponent(root);
			}

			BlockRendering();
		});

		rootComponentIds.Clear();
		AssertNoUnhandledExceptions();
	}

	/// <summary>
	/// Prepares the renderer for a new asynchronous render operation.
	/// </summary>
	internal void UnblockRendering() => renderBlocker.Set();

	internal void AllowOneRenderCycle()
	{
		// Normally UnblockRendering will be called inside the Dispatcher
		// so we don't have to deal with concurrency issues. But here we can't as
		// the average case is that the Renderer is currently "locked" by the renderBlocker, so we go into a deadlock
		// To prevent concurrency issues, we introduce this "defensive" lock.
		//
		// In WaitForHelpers we allow one render cycle which triggers the OnRender event of the component
		// This will trigger another check inside the WaitForHelpers.
		// The WaitForHelper and the Renderer are in different threads, leading to concurrency issues.
		lock (renderCycleLock)
		{
			blockRenderer = true;
			UnblockRendering();
		}
	}

	internal void EnableUnblockedRendering(Action act)
	{
		Dispatcher.InvokeAsync(() =>
		{
			UnblockRendering();
			act();
			BlockRendering();
		});
	}

	internal Task<T> EnableUnblockedRendering<T>(Func<T> act)
	{
		return Dispatcher.InvokeAsync(() =>
		{
			UnblockRendering();
			var result = act();
			BlockRendering();
			return result;
		});
	}

	/// <inheritdoc/>
	protected override void ProcessPendingRender()
	{
		if (disposed)
			return;

		renderBlocker.Wait();

		if (disposed)
			return;

		base.ProcessPendingRender();
	}

	/// <inheritdoc/>
	protected override Task UpdateDisplayAsync(in RenderBatch renderBatch)
	{
		if (disposed)
			return Task.CompletedTask;

		renderBlocker.Wait();

		if (disposed)
			return Task.CompletedTask;

		BlockNextRenderCycle();

		UpdateDisplay(renderBatch);
		return Task.CompletedTask;

		void BlockNextRenderCycle()
		{
			if (blockRenderer)
			{
				BlockRendering();
				blockRenderer = false;
			}
		}
	}

	/// <inheritdoc/>
	protected override IComponent ResolveComponentForRenderMode(Type componentType, int? parentComponentId,
		IComponentActivator componentActivator, IComponentRenderMode renderMode)

	{
		ArgumentNullException.ThrowIfNull(componentActivator);
		return componentActivator.CreateInstance(componentType);
	}

	private void UpdateDisplay(in RenderBatch renderBatch)
	{
		RenderCount++;
		var renderEvent = new RenderEvent(renderBatch, new RenderTreeFrameDictionary());

		if (disposed)
			return;

		// removes disposed components
		for (var i = 0; i < renderBatch.DisposedComponentIDs.Count; i++)
		{
			var id = renderBatch.DisposedComponentIDs.Array[i];

			logger.LogComponentDisposed(id);

			if (renderedComponents.TryGetValue(id, out var rc))
			{
				renderedComponents.Remove(id);
				rc.OnRender(renderEvent);
			}
		}

		// notify each rendered component about the render
		foreach (var (key, rc) in renderedComponents.ToArray())
		{
			LoadRenderTreeFrames(rc.ComponentId, renderEvent.Frames);

			rc.OnRender(renderEvent);

			logger.LogComponentRendered(rc.ComponentId);

			// RC can replace the instance of the component it is bound
			// to while processing the update event.
			if (key != rc.ComponentId)
			{
				renderedComponents.Remove(key);
				renderedComponents.Add(rc.ComponentId, rc);
			}
		}
	}

	/// <inheritdoc/>
	protected override void Dispose(bool disposing)
	{
		if(disposed)
			return;

		BlockRendering();
		disposed = true;

		if (disposing)
		{
			foreach (var rc in renderedComponents.Values)
			{
				rc.Dispose();
			}

			renderedComponents.Clear();
			unhandledExceptionTsc.TrySetCanceled();
		}

		UnblockRendering();
		renderBlocker.Dispose();

		base.Dispose(disposing);
	}

	private void BlockRendering() => renderBlocker.Reset();

	private IRenderedFragment Render(RenderFragment renderFragment)
	{
		ObjectDisposedException.ThrowIf(disposed, this);

		var renderTask = Dispatcher.InvokeAsync(() =>
		{
			UnblockRendering();
			ResetUnhandledException();

			var root = new RootComponent(renderFragment);
			var rootComponentId = AssignRootComponentId(root);
			var result = new RenderedFragment(rootComponentId, services);
			renderedComponents.Add(rootComponentId, result);
			rootComponentIds.Add(rootComponentId);
			root.Render();
			return result;
		});

		IRenderedFragment result;

		if (!renderTask.IsCompleted)
		{
			logger.LogAsyncInitialRender();
			result = renderTask.GetAwaiter().GetResult();
		}
		else
		{
			result = renderTask.Result;
		}

		logger.LogInitialRenderCompleted(result.ComponentId);

		AssertNoUnhandledExceptions();
		BlockRendering();

		return result;
	}

	private IEnumerable<IRenderedComponent<TComponent>> FindComponentsInternal<TComponent>(IRenderedFragment parentComponent)
		where TComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(parentComponent);
		ObjectDisposedException.ThrowIf(disposed, this);

		var framesCollection = new RenderTreeFrameDictionary();

		return FindComponentsInRenderTree(parentComponent.ComponentId);

		IEnumerable<IRenderedComponent<TComponent>> FindComponentsInRenderTree(int componentId)
		{
			var frames = GetOrLoadRenderTreeFrame(framesCollection, componentId);

			for (var i = 0; i < frames.Count; i++)
			{
				var frame = frames.Array[i];
				if (frame.FrameType == RenderTreeFrameType.Component)
				{
					if (frame.Component is TComponent component)
					{
						yield return GetOrCreateRenderedComponent(framesCollection, frame.ComponentId, component);
					}

					foreach (var childComponent in FindComponentsInRenderTree(frame.ComponentId))
					{
						yield return childComponent;
					}
				}
			}
		}
	}

	IRenderedComponent<TComponent> GetOrCreateRenderedComponent<TComponent>(RenderTreeFrameDictionary framesCollection, int componentId, TComponent component)
		where TComponent : IComponent
	{
		if (renderedComponents.TryGetValue(componentId, out var renderedComponent))
		{
			return (IRenderedComponent<TComponent>)renderedComponent;
		}

		LoadRenderTreeFrames(componentId, framesCollection);
		var result = new RenderedComponent<TComponent>(componentId, component, framesCollection, services);
		renderedComponents.Add(result.ComponentId, result);

		return result;
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

	/// <inheritdoc/>
	protected override void HandleException(Exception exception)
	{
		if (exception is null || disposed)
			return;

		logger.LogUnhandledException(exception);

		capturedUnhandledException = exception;

		if (!unhandledExceptionTsc.TrySetResult(capturedUnhandledException))
		{
			unhandledExceptionTsc = new TaskCompletionSource<Exception>(TaskCreationOptions.RunContinuationsAsynchronously);
			unhandledExceptionTsc.SetResult(capturedUnhandledException);
		}
	}

	private void ResetUnhandledException()
	{
		capturedUnhandledException = null;

		if (unhandledExceptionTsc.Task.IsCompleted)
			unhandledExceptionTsc = new TaskCompletionSource<Exception>(TaskCreationOptions.RunContinuationsAsynchronously);
	}

	private void AssertNoUnhandledExceptions()
	{
		if (capturedUnhandledException is Exception unhandled && !disposed)
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
