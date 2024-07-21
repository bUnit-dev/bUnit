using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using Microsoft.Extensions.Logging;

namespace Bunit.Rendering;

/// <summary>
/// Represents a bUnit <see cref="BunitRenderer"/> used to render Blazor components and fragments during bUnit tests.
/// </summary>
public sealed class BunitRenderer : Renderer
{
	private readonly BunitServiceProvider services;
	private readonly List<Task> disposalTasks = [];
	private static readonly ConcurrentDictionary<Type, ConstructorInfo> ComponentActivatorCache = new();

	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_isBatchInProgress")]
	private static extern ref bool GetIsBatchInProgressField(Renderer renderer);

	[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "SetDirectParameters")]
	private static extern void CallSetDirectParameters(ComponentState componentState, ParameterView parameters);

	private readonly object renderTreeUpdateLock = new();

	private readonly HashSet<int> returnedRenderedComponentIds = new();
	private readonly List<BunitRootComponent> rootComponents = new();
	private readonly ILogger<BunitRenderer> logger;
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
	internal int RenderCount { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="BunitRenderer"/> class.
	/// </summary>
	public BunitRenderer(BunitServiceProvider services, ILoggerFactory loggerFactory)
		: base(services, loggerFactory, new BunitComponentActivator(services.GetRequiredService<ComponentFactoryCollection>(), null))
	{
		this.services = services;
		logger = loggerFactory.CreateLogger<BunitRenderer>();
		ElementReferenceContext = new WebElementReferenceContext(services.GetRequiredService<IJSRuntime>());
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BunitRenderer"/> class.
	/// </summary>
	public BunitRenderer(BunitServiceProvider services, ILoggerFactory loggerFactory, IComponentActivator componentActivator)
		: base(services, loggerFactory, new BunitComponentActivator(services.GetRequiredService<ComponentFactoryCollection>(), componentActivator))
	{
		this.services = services;
		logger = loggerFactory.CreateLogger<BunitRenderer>();
		ElementReferenceContext = new WebElementReferenceContext(services.GetRequiredService<IJSRuntime>());
	}

	/// <summary>
	/// Renders the <paramref name="renderFragment"/>.
	/// </summary>
	/// <param name="renderFragment">The <see cref="Microsoft.AspNetCore.Components.RenderFragment"/> to render.</param>
	/// <returns>A <see cref="IRenderedComponent{TComponent}"/> that provides access to the rendered <paramref name="renderFragment"/>.</returns>
	public IRenderedComponent<IComponent> RenderFragment(RenderFragment renderFragment)
		=> Render(renderFragment);

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
	public IRenderedComponent<TComponent> FindComponent<TComponent>(IRenderedComponent<IComponent> parentComponent)
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
	public IReadOnlyList<IRenderedComponent<TComponent>> FindComponents<TComponent>(IRenderedComponent<IComponent> parentComponent)
		where TComponent : IComponent
		=> FindComponents<TComponent>(parentComponent, int.MaxValue);

	/// <summary>
	/// Disposes all components rendered by the <see cref="BunitRenderer" />.
	/// </summary>
	public Task DisposeComponents()
	{
		ObjectDisposedException.ThrowIf(disposed, this);
		Task? returnTask;

		lock (renderTreeUpdateLock)
		{
			returnTask = Dispatcher.InvokeAsync(async () =>
			{
				ResetUnhandledException();

				foreach (var root in rootComponents)
				{
					root.Detach();
				}

				await Task.WhenAll(disposalTasks).ConfigureAwait(false);
				disposalTasks.Clear();
			});

			rootComponents.Clear();
			AssertNoUnhandledExceptions();
		}

		return returnTask;
	}
	/// <inheritdoc/>
	protected override ComponentState CreateComponentState(int componentId, IComponent component, ComponentState? parentComponentState)
	{
		ArgumentNullException.ThrowIfNull(component);

		var TComponent = component.GetType();
		var renderedComponentType = typeof(RenderedComponent<>).MakeGenericType(TComponent);
		var renderedComponent = CreateComponentInstance();

		Debug.Assert(renderedComponent is not null, "RenderedComponent should not be null");

		return (ComponentState)renderedComponent;

		object CreateComponentInstance()
		{
			var constructorInfo = ComponentActivatorCache.GetOrAdd(renderedComponentType, type
				=> type.GetConstructor(
			[
				typeof(BunitRenderer),
				typeof(int),
				typeof(IComponent),
				typeof(IServiceProvider),
				typeof(ComponentState)
			])!);

			Debug.Assert(constructorInfo is not null, "Could not find ConstructorInfo");

			return constructorInfo.Invoke([this, componentId, component, services, parentComponentState]);
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
	protected override void AddPendingTask(ComponentState? componentState, Task task)
	{
		if (componentState is null)
		{
			ArgumentNullException.ThrowIfNull(task);
			AddDisposalTaskToQueue();
		}

		base.AddPendingTask(componentState, task);

		void AddDisposalTaskToQueue()
		{
			var t = task;
			t = task.ContinueWith(_ =>
			{
				disposalTasks.Remove(t);
			}, TaskScheduler.Current);

			disposalTasks.Add(t);
		}
	}

	internal Task SetDirectParametersAsync<TComponent>(IRenderedComponent<TComponent> renderedComponent, ParameterView parameters)
		where TComponent : IComponent
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
		for (var i = 0; i < renderBatch.DisposedComponentIDs.Count; i++)
		{
			var id = renderBatch.DisposedComponentIDs.Array[i];
			returnedRenderedComponentIds.Remove(id);
		}

		for (var i = 0; i < renderBatch.UpdatedComponents.Count; i++)
		{
			var diff = renderBatch.UpdatedComponents.Array[i];
			var componentState = GetRenderedComponent(diff.ComponentId);
			componentState.RenderCount++;

			componentState.IsDirty = true;

			if (componentState.Root is not null)
			{
				componentState.Root.IsDirty = true;
			}
		}

		foreach (var item in rootComponents)
		{
			var root = GetRenderedComponent(item);
			if (root.IsDirty)
			{
				root.UpdateMarkup();
			}
		}

		foreach (var renderedComponentId in returnedRenderedComponentIds)
		{
			var renderedComponent = GetRenderedComponent(renderedComponentId);
			if (renderedComponent.IsDirty)
			{
				renderedComponent.UpdateMarkup();
			}
		}

		return Task.CompletedTask;
	}

	/// <inheritdoc/>
	internal new ArrayRange<RenderTreeFrame> GetCurrentRenderTreeFrames(int componentId)
		=> base.GetCurrentRenderTreeFrames(componentId);

	/// <inheritdoc/>
	internal IRenderedComponent GetRenderedComponent(int componentId)
		=> (IRenderedComponent)GetComponentState(componentId);

	/// <inheritdoc/>
	internal IRenderedComponent GetRenderedComponent(IComponent component)
		=> (IRenderedComponent)GetComponentState(component);

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
				returnedRenderedComponentIds.Clear();
				disposalTasks.Clear();
				unhandledExceptionTsc.TrySetCanceled();
			}

			Dispatcher.InvokeAsync(() => base.Dispose(disposing));
		}
	}

	private IRenderedComponent<BunitRootComponent> Render(RenderFragment renderFragment)
	{
		ObjectDisposedException.ThrowIf(disposed, this);

		var renderTask = Dispatcher.InvokeAsync(() =>
		{
			ResetUnhandledException();

			var root = new BunitRootComponent(renderFragment);
			var rootComponentId = AssignRootComponentId(root);
			returnedRenderedComponentIds.Add(rootComponentId);
			rootComponents.Add(root);
			root.Render();
			return rootComponentId;
		});

		int componentId = -1;

		if (!renderTask.IsCompleted)
		{
			logger.LogAsyncInitialRender();
			componentId = renderTask.GetAwaiter().GetResult();
		}
		else
		{
			componentId = renderTask.Result;
		}

		var result = GetRenderedComponent<BunitRootComponent>(componentId);

		logger.LogInitialRenderCompleted(result.ComponentId);

		AssertNoUnhandledExceptions();

		return result;
	}

	private List<IRenderedComponent<TComponent>> FindComponents<TComponent>(IRenderedComponent<IComponent> parentComponent, int resultLimit)
		where TComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(parentComponent);
		ObjectDisposedException.ThrowIf(disposed, this);

		var result = resultLimit == int.MaxValue
			? new List<IRenderedComponent<TComponent>>()
			: new List<IRenderedComponent<TComponent>>(resultLimit);

		// Blocks the renderer from changing the render tree
		// while this method searches through it.
		lock (renderTreeUpdateLock)
		{
			ObjectDisposedException.ThrowIf(disposed, this);
			FindComponentsInRenderTree(parentComponent.ComponentId);
			foreach (var rc in result)
			{
				((IRenderedComponent)rc).UpdateMarkup();
			}
		}

		return result;

		void FindComponentsInRenderTree(int componentId)
		{
			var frames = GetCurrentRenderTreeFrames(componentId);

			for (var i = 0; i < frames.Count; i++)
			{
				ref var frame = ref frames.Array[i];
				if (frame.FrameType == RenderTreeFrameType.Component)
				{
					if (frame.Component is TComponent)
					{
						result.Add(GetRenderedComponent<TComponent>(frame.ComponentId));

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

	private IRenderedComponent<TComponent> GetRenderedComponent<TComponent>(int componentId)
		where TComponent : IComponent
	{
		var result = GetComponentState(componentId);
		returnedRenderedComponentIds.Add(result.ComponentId);
		return (IRenderedComponent<TComponent>)result;
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
		if (!framesCollection.TryGetValue(componentId, out var frames))
		{
			frames = GetCurrentRenderTreeFrames(componentId);
			framesCollection.Add(componentId, frames);
		}

		return frames;
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
