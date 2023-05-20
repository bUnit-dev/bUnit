using System.Runtime.ExceptionServices;
using Microsoft.Extensions.Logging;

namespace Bunit.Rendering;

/// <summary>
/// Represents a bUnit <see cref="Renderer"/> used to render Blazor components and fragments during bUnit tests.
/// </summary>
public sealed class BunitRenderer : Renderer
{
	private readonly object renderTreeUpdateLock = new();
	private readonly SynchronizationContext? usersSyncContext = SynchronizationContext.Current;
	private readonly Dictionary<int, IRenderedFragment> renderedComponents = new();
	private readonly List<int> rootComponentIds = new();
	private readonly ILogger<BunitRenderer> logger;
	private readonly IRenderedComponentActivator activator;
	private TaskCompletionSource<Exception> unhandledExceptionTsc = new(TaskCreationOptions.RunContinuationsAsynchronously);
	private Exception? capturedUnhandledException;

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
	public BunitRenderer(IRenderedComponentActivator renderedComponentActivator, TestServiceProvider services, ILoggerFactory loggerFactory)
		: base(services, loggerFactory)
	{
		logger = loggerFactory.CreateLogger<BunitRenderer>();
		activator = renderedComponentActivator;
		ElementReferenceContext = new WebElementReferenceContext(services.GetRequiredService<IJSRuntime>());
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BunitRenderer"/> class.
	/// </summary>
	public BunitRenderer(IRenderedComponentActivator renderedComponentActivator, TestServiceProvider services, ILoggerFactory loggerFactory, IComponentActivator componentActivator)
		: base(services, loggerFactory, componentActivator)
	{
		logger = loggerFactory.CreateLogger<BunitRenderer>();
		activator = renderedComponentActivator;
		ElementReferenceContext = new WebElementReferenceContext(services.GetRequiredService<IJSRuntime>());
	}

	/// <summary>
	/// Renders the <paramref name="renderFragment"/>.
	/// </summary>
	/// <param name="renderFragment">The <see cref="Microsoft.AspNetCore.Components.RenderFragment"/> to render.</param>
	/// <returns>A <see cref="IRenderedFragment"/> that provides access to the rendered <paramref name="renderFragment"/>.</returns>
	public IRenderedFragment RenderFragment(RenderFragment renderFragment)
		=> Render(renderFragment, id => activator.CreateRenderedFragment(id));

	/// <summary>
	/// Renders a <typeparamref name="TComponent"/> with the <paramref name="parameters"/> passed to it.
	/// </summary>
	/// <typeparam name = "TComponent" > The type of component to render.</typeparam>
	/// <param name="parameters">The parameters to pass to the component.</param>
	/// <returns>A <see cref="IRenderedComponent{TComponent}"/> that provides access to the rendered component.</returns>
	public IRenderedComponent<TComponent> RenderComponent<TComponent>(ComponentParameterCollection parameters)
		where TComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(parameters);

		var renderFragment = parameters.ToRenderFragment<TComponent>();
		return Render(renderFragment, id => activator.CreateRenderedComponent<TComponent>(id));
	}

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

		// Calling base.DispatchEventAsync updates the render tree
		// if the event contains associated data.
		lock (renderTreeUpdateLock)
		{
			var result = Dispatcher.InvokeAsync(() =>
			{
				ResetUnhandledException();

				try
				{
					return base.DispatchEventAsync(eventHandlerId, fieldInfo, eventArgs);
				}
				catch (ArgumentException ex) when (string.Equals(ex.Message, $"There is no event handler associated with this event. EventId: '{eventHandlerId}'. (Parameter 'eventHandlerId')", StringComparison.Ordinal))
				{
					if (ignoreUnknownEventHandlers)
					{
						return Task.CompletedTask;
					}

					var betterExceptionMsg = new UnknownEventHandlerIdException(eventHandlerId, fieldInfo, ex);
					return Task.FromException(betterExceptionMsg);
				}
			});

			if (result.IsFaulted && result.Exception is not null)
			{
				HandleException(result.Exception);
			}

			AssertNoUnhandledExceptions();

			return result;
		}
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
		// The dispatcher will always return a completed task,
		// when dealing with an IAsyncDisposable.
		// Therefore checking for a completed task and awaiting it
		// will only work on IDisposable
		var disposeTask = Dispatcher.InvokeAsync(() =>
		{
			ResetUnhandledException();

			foreach (var root in rootComponentIds)
			{
				RemoveRootComponent(root);
			}
		});

		if (!disposeTask.IsCompleted)
		{
			disposeTask.GetAwaiter().GetResult();
		}

		rootComponentIds.Clear();
		AssertNoUnhandledExceptions();
	}

	/// <inheritdoc/>
	protected override void ProcessPendingRender()
	{
		// Blocks updates to the renderers internal render tree
		// while the render tree is being read elsewhere.
		// base.ProcessPendingRender calls UpdateDisplayAsync,
		// so there is no need to lock in that method.
		lock (renderTreeUpdateLock)
		{
			base.ProcessPendingRender();
		}
	}

	/// <inheritdoc/>
	protected override Task UpdateDisplayAsync(in RenderBatch renderBatch)
	{
		if (usersSyncContext is not null && usersSyncContext != SynchronizationContext.Current)
		{
			// The users' sync context, typically one established by
			// xUnit or another testing framework is used to update any
			// rendered fragments/dom trees and trigger WaitForX handlers.
			// This ensures that changes to DOM observed inside a WaitForX handler
			// will also be visible outside a WaitForX handler, since
			// they will be running in the same sync context. The theory is that
			// this should mitigate the issues where Blazor's dispatcher/thread is used
			// to verify an assertion inside a WaitForX handler, and another thread is
			// used again to access the DOM/repeat the assertion, where the change
			// may not be visible yet (another theory about why that may happen is different
			// CPU cache updates not happening immediately).
			//
			// There is no guarantee a caller/test framework has set a sync context.
			usersSyncContext.Send(static (state) =>
			{
				var (renderBatch, renderer) = ((RenderBatch, BunitRenderer))state!;
				renderer.UpdateDisplay(renderBatch);
			}, (renderBatch, this));
		}
		else
		{
			UpdateDisplay(renderBatch);
		}

		return Task.CompletedTask;
	}

	private void UpdateDisplay(in RenderBatch renderBatch)
	{
		RenderCount++;
		var renderEvent = new RenderEvent(renderBatch, new RenderTreeFrameDictionary());

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
		if (disposing)
		{
			foreach (var rc in renderedComponents.Values)
			{
				rc.Dispose();
			}

			renderedComponents.Clear();
			unhandledExceptionTsc.TrySetCanceled();
		}

		base.Dispose(disposing);
	}

	private TResult Render<TResult>(RenderFragment renderFragment, Func<int, TResult> activator)
		where TResult : IRenderedFragment
	{
		var renderTask = Dispatcher.InvokeAsync(() =>
		{
			ResetUnhandledException();

			var root = new RootComponent(renderFragment);
			var rootComponentId = AssignRootComponentId(root);
			var result = activator(rootComponentId);
			renderedComponents.Add(rootComponentId, result);
			rootComponentIds.Add(rootComponentId);
			root.Render();
			return result;
		});

		TResult result;

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

		return result;
	}

	private IEnumerable<IRenderedComponent<TComponent>> FindComponentsInternal<TComponent>(IRenderedFragment parentComponent)
		where TComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(parentComponent);

		var framesCollection = new RenderTreeFrameDictionary();

		// Blocks the renderer from changing the render tree
		// while this method searches through it.
		lock (renderTreeUpdateLock)
		{
			return FindComponentsInRenderTree(parentComponent.ComponentId);
		}

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
		var result = activator.CreateRenderedComponent(componentId, component, framesCollection);
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
		if (exception is null)
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
