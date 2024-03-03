using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace Bunit.Rendering;

/// <summary>
/// Represents a bUnit <see cref="BunitRenderer"/> used to render Blazor components and fragments during bUnit tests.
/// </summary>
public sealed class BunitRenderer : Renderer
{
	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_isBatchInProgress")]
	private static extern ref bool GetIsBatchInProgressField(Renderer renderer);

	[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "SetDirectParameters")]
	private static extern void CallSetDirectParameters(ComponentState componentState, ParameterView parameters);

	private readonly object renderTreeUpdateLock = new();
	private readonly Dictionary<int, RenderedFragment> renderedComponents = new();
	private readonly List<RootComponent> rootComponents = new();
	private readonly ILogger<BunitRenderer> logger;
	private readonly IRenderedComponentActivator activator;
	private bool disposed;
	private TaskCompletionSource<Exception> unhandledExceptionTsc = new(TaskCreationOptions.RunContinuationsAsynchronously);
	private Exception? capturedUnhandledException;

	private bool IsBatchInProgress
	{
#pragma warning disable S1144 // Unused private types or members should be removed
		get
		{
			return GetIsBatchInProgressField(this);
		}
#pragma warning restore S1144 // Unused private types or members should be removed
		set
		{
			GetIsBatchInProgressField(this) = value;
		}
	}

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
		: base(services, loggerFactory, new BunitComponentActivator(services.GetRequiredService<ComponentFactoryCollection>(), null))
	{
		logger = loggerFactory.CreateLogger<BunitRenderer>();
		activator = renderedComponentActivator;
		ElementReferenceContext = new WebElementReferenceContext(services.GetRequiredService<IJSRuntime>());
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BunitRenderer"/> class.
	/// </summary>
	public BunitRenderer(IRenderedComponentActivator renderedComponentActivator, TestServiceProvider services, ILoggerFactory loggerFactory, IComponentActivator componentActivator)
		: base(services, loggerFactory, new BunitComponentActivator(services.GetRequiredService<ComponentFactoryCollection>(), componentActivator))
	{
		logger = loggerFactory.CreateLogger<BunitRenderer>();
		activator = renderedComponentActivator;
		ElementReferenceContext = new WebElementReferenceContext(services.GetRequiredService<IJSRuntime>());
	}

	/// <summary>
	/// Renders the <paramref name="renderFragment"/>.
	/// </summary>
	/// <param name="renderFragment">The <see cref="Microsoft.AspNetCore.Components.RenderFragment"/> to render.</param>
	/// <returns>A <see cref="RenderedFragment"/> that provides access to the rendered <paramref name="renderFragment"/>.</returns>
	public RenderedFragment RenderFragment(RenderFragment renderFragment)
		=> Render(renderFragment, id => activator.CreateRenderedFragment(id));

	/// <summary>
	/// Renders a <typeparamref name="TComponent"/> with the <paramref name="parameters"/> passed to it.
	/// </summary>
	/// <typeparam name="TComponent">The type of component to render.</typeparam>
	/// <param name="parameters">The parameters to pass to the component.</param>
	/// <returns>A <see cref="RenderedComponent{TComponent}"/> that provides access to the rendered component.</returns>
	public RenderedComponent<TComponent> RenderComponent<TComponent>(ComponentParameterCollection parameters)
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
	public new Task DispatchEventAsync(
		ulong eventHandlerId,
		EventFieldInfo fieldInfo,
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
		EventFieldInfo fieldInfo,
		EventArgs eventArgs,
		bool ignoreUnknownEventHandlers)
	{
		ArgumentNullException.ThrowIfNull(fieldInfo);

		ObjectDisposedException.ThrowIf(disposed, this);

		// Calling base.DispatchEventAsync updates the render tree
		// if the event contains associated data.
		lock (renderTreeUpdateLock)
		{
			ObjectDisposedException.ThrowIf(disposed, this);

			var result = Dispatcher.InvokeAsync(() =>
			{
				ResetUnhandledException();

				try
				{
					logger.LogDispatchingEvent(eventHandlerId, fieldInfo, eventArgs);
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
	public RenderedComponent<TComponent> FindComponent<TComponent>(RenderedFragment parentComponent)
		where TComponent : IComponent
	{
		var foundComponents = FindComponents<TComponent>(parentComponent, 1);
		return foundComponents.Count == 1
			? foundComponents[0]
			: throw new ComponentNotFoundException(typeof(TComponent));
	}

	/// <summary>
	/// Performs a depth-first search for all <typeparamref name="TComponent"/> child components of the <paramref name="parentComponent"/>.
	/// </summary>
	/// <typeparam name="TComponent">Type of components to find.</typeparam>
	/// <param name="parentComponent">Parent component to search.</param>
	public IReadOnlyList<RenderedComponent<TComponent>> FindComponents<TComponent>(RenderedFragment parentComponent)
		where TComponent : IComponent
		=> FindComponents<TComponent>(parentComponent, int.MaxValue);

	/// <summary>
	/// Disposes all components rendered by the <see cref="BunitRenderer" />.
	/// </summary>
	public void DisposeComponents()
	{
		ObjectDisposedException.ThrowIf(disposed, this);

		lock (renderTreeUpdateLock)
		{
			// The dispatcher will always return a completed task,
			// when dealing with an IAsyncDisposable.
			// Therefore checking for a completed task and awaiting it
			// will only work on IDisposable
			Dispatcher.InvokeAsync(() =>
			{
				ResetUnhandledException();

				foreach (var root in rootComponents)
				{
					root.Detach();
				}
			});

			rootComponents.Clear();
			AssertNoUnhandledExceptions();
		}
	}

	/// <inheritdoc/>
	protected override IComponent ResolveComponentForRenderMode(Type componentType, int? parentComponentId,
		IComponentActivator componentActivator, IComponentRenderMode renderMode)

	{
		ArgumentNullException.ThrowIfNull(componentActivator);
		return componentActivator.CreateInstance(componentType);
	}

	/// <inheritdoc/>
	internal Task SetDirectParametersAsync(RenderedFragment renderedComponent, ParameterView parameters)
	{
		ObjectDisposedException.ThrowIf(disposed, this);

		var result = Dispatcher.InvokeAsync(() =>
		{
			try
			{
				IsBatchInProgress = true;
				SetDirectParametersViaComponentState(this, renderedComponent.ComponentId, parameters);
			}
			catch (TargetInvocationException ex) when (ex.InnerException is not null)
			{
				throw ex.InnerException;
			}
			finally
			{
				IsBatchInProgress = false;
			}

			ProcessPendingRender();
		});

		if (result.IsFaulted && result.Exception is not null)
		{
			HandleException(result.Exception);
		}

		AssertNoUnhandledExceptions();

		return result;

		static void SetDirectParametersViaComponentState(BunitRenderer renderer, int componentId, in ParameterView parameters)
		{
			var componentState = renderer.GetComponentState(componentId);
			CallSetDirectParameters(componentState, parameters);
		}
	}

	/// <inheritdoc/>
	protected override void ProcessPendingRender()
	{
		if (disposed)
		{
			logger.LogRenderCycleActiveAfterDispose();
			return;
		}

		// Blocks updates to the renderers internal render tree
		// while the render tree is being read elsewhere.
		// base.ProcessPendingRender calls UpdateDisplayAsync,
		// so there is no need to lock in that method.
		lock (renderTreeUpdateLock)
		{
			if (disposed)
			{
				logger.LogRenderCycleActiveAfterDispose();
				return;
			}

			base.ProcessPendingRender();
		}
	}

	/// <inheritdoc/>
	protected override Task UpdateDisplayAsync(in RenderBatch renderBatch)
	{
		var renderEvent = new RenderEvent();
		PrepareRenderEvent(renderBatch);
		ApplyRenderEvent(renderEvent);

		return Task.CompletedTask;

		void PrepareRenderEvent(in RenderBatch renderBatch)
		{
			for (var i = 0; i < renderBatch.DisposedComponentIDs.Count; i++)
			{
				var id = renderBatch.DisposedComponentIDs.Array[i];
				renderEvent.SetDisposed(id);
			}

			for (int i = 0; i < renderBatch.UpdatedComponents.Count; i++)
			{
				ref var update = ref renderBatch.UpdatedComponents.Array[i];
				renderEvent.SetUpdated(update.ComponentId, update.Edits.Count > 0);
			}

			foreach (var (_, rc) in renderedComponents)
			{
				LoadChangesIntoRenderEvent(rc.ComponentId);
			}
		}

		void LoadChangesIntoRenderEvent(int componentId)
		{
			var status = renderEvent.GetOrCreateStatus(componentId);
			if (status.FramesLoaded || status.Disposed)
				return;

			var frames = GetCurrentRenderTreeFrames(componentId);
			renderEvent.AddFrames(componentId, frames);

			for (var i = 0; i < frames.Count; i++)
			{
				ref var frame = ref frames.Array[i];
				if (frame.FrameType == RenderTreeFrameType.Component)
				{
					// If a child component of the current components has been
					// disposed, there is no reason to load the disposed components
					// render tree frames. This can also cause a stack overflow if
					// the current component was previously a child of the disposed
					// component (is that possible?)
					var childStatus = renderEvent.GetOrCreateStatus(frame.ComponentId);
					if (childStatus.Disposed)
					{
						logger.LogDisposedChildInRenderTreeFrame(componentId, frame.ComponentId);
					}
					// The assumption is that a component cannot be in multiple places at
					// once. However, in case this is not a correct assumption, this
					// ensures that a child components frames are only loaded once.
					else if (!renderEvent.GetOrCreateStatus(frame.ComponentId).FramesLoaded)
					{
						LoadChangesIntoRenderEvent(frame.ComponentId);
					}

					if (childStatus.Rendered || childStatus.Changed || childStatus.Disposed)
					{
						status.Rendered = status.Rendered || childStatus.Rendered;

						// The current component should also be marked as changed if the child component is
						// either changed or disposed, as there is a good chance that the child component
						// contained markup which is no longer visible.
						status.Changed = status.Changed || childStatus.Changed || childStatus.Disposed;
					}
				}
			}
		}
	}

	private void ApplyRenderEvent(RenderEvent renderEvent)
	{
		RenderCount++;

		foreach (var (componentId, status) in renderEvent.Statuses)
		{
			if (status.UpdatesApplied || !renderedComponents.TryGetValue(componentId, out var rc))
			{
				continue;
			}

			if (status.Disposed)
			{
				renderedComponents.Remove(componentId);
				rc.OnRender(renderEvent);
				renderEvent.SetUpdatedApplied(componentId);
				logger.LogComponentDisposed(componentId);
				continue;
			}

			if (status.UpdateNeeded)
			{
				rc.OnRender(renderEvent);
				renderEvent.SetUpdatedApplied(componentId);

				// RC can replace the instance of the component it is bound
				// to while processing the update event, e.g. during the
				// initial render of a component.
				if (componentId != rc.ComponentId)
				{
					renderedComponents.Remove(componentId);
					renderedComponents.Add(rc.ComponentId, rc);
					renderEvent.SetUpdatedApplied(rc.ComponentId);
				}

				logger.LogComponentRendered(rc.ComponentId);
			}
		}
	}

	/// <inheritdoc/>
	protected override void Dispose(bool disposing)
	{
		if (disposed)
			return;

		lock (renderTreeUpdateLock)
		{
			if (disposed)
				return;

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

			Dispatcher.InvokeAsync(() => base.Dispose(disposing));
		}
	}

	private TResult Render<TResult>(RenderFragment renderFragment, Func<int, TResult> activator)
		where TResult : RenderedFragment
	{
		ObjectDisposedException.ThrowIf(disposed, this);

		var renderTask = Dispatcher.InvokeAsync(() =>
		{
			ResetUnhandledException();

			var root = new RootComponent(renderFragment);
			var rootComponentId = AssignRootComponentId(root);
			var result = activator(rootComponentId);
			renderedComponents.Add(rootComponentId, result);
			rootComponents.Add(root);
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

	private List<RenderedComponent<TComponent>> FindComponents<TComponent>(RenderedFragment parentComponent, int resultLimit)
		where TComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(parentComponent);

		ObjectDisposedException.ThrowIf(disposed, this);

		var result = new List<RenderedComponent<TComponent>>();
		var framesCollection = new RenderTreeFrameDictionary();

		// Blocks the renderer from changing the render tree
		// while this method searches through it.
		lock (renderTreeUpdateLock)
		{
			ObjectDisposedException.ThrowIf(disposed, this);

			FindComponentsInRenderTree(parentComponent.ComponentId);
		}

		return result;

		void FindComponentsInRenderTree(int componentId)
		{
			var frames = GetOrLoadRenderTreeFrame(framesCollection, componentId);

			for (var i = 0; i < frames.Count; i++)
			{
				ref var frame = ref frames.Array[i];
				if (frame.FrameType == RenderTreeFrameType.Component)
				{
					if (frame.Component is TComponent component)
					{
						result.Add(GetOrCreateRenderedComponent(framesCollection, frame.ComponentId, component));

						if (result.Count == resultLimit)
							return;
					}

					FindComponentsInRenderTree(frame.ComponentId);

					if (result.Count == resultLimit)
						return;
				}
			}
		}
	}

	private RenderedComponent<TComponent> GetOrCreateRenderedComponent<TComponent>(RenderTreeFrameDictionary framesCollection, int componentId, TComponent component)
		where TComponent : IComponent
	{
		if (renderedComponents.TryGetValue(componentId, out var renderedComponent))
		{
			return (RenderedComponent<TComponent>)renderedComponent;
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

			if (frame.FrameType == RenderTreeFrameType.Component && !framesCollection.Contains(frame.ComponentId))
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
		ArgumentNullException.ThrowIfNull(exception);

		if (disposed)
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
		// Ensure we are not throwing an exception while a render is ongoing.
		// This could lead to the renderer being disposed which could lead to
		// tests failing that should not be failing.
		lock (renderTreeUpdateLock)
		{
			if (disposed)
				return;

			if (capturedUnhandledException is { } unhandled)
			{
				capturedUnhandledException = null;

				if (unhandled is AggregateException { InnerExceptions.Count: 1 } aggregateException)
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
}
