using Bunit.JSInterop;
using Bunit.JSInterop.InvocationHandlers.Implementation;

namespace Bunit;

/// <summary>
/// Represents an bUnit's implementation of Blazor's JSInterop.
/// </summary>
public class BunitJSInterop
{
	private readonly Dictionary<Type, List<object>> handlers = new();
	private JSRuntimeMode mode;
	private BunitContext? owningContext;
	private TimeSpan standaloneDefaultWaitTimeout = TimeSpan.FromSeconds(1);

	/// <summary>
	/// Gets a dictionary of all <see cref="List{JSRuntimeInvocation}"/> this mock has observed.
	/// </summary>
	public JSRuntimeInvocationDictionary Invocations { get; } = new();

	/// <summary>
	/// Gets or sets whether the <see cref="BunitJSInterop"/> is running in <see cref="JSRuntimeMode.Loose"/> or
	/// <see cref="JSRuntimeMode.Strict"/>.
	/// </summary>
	public virtual JSRuntimeMode Mode { get => mode; set => mode = value; }

	/// <summary>
	/// Gets the mocked <see cref="IJSRuntime"/> instance.
	/// </summary>
	public IJSRuntime JSRuntime { get; }

	/// <summary>
	/// Gets or sets the timeout used by unconfigured invocation handlers. Defers to the owning
	/// <see cref="BunitContext.DefaultWaitTimeout"/> when attached.
	/// </summary>
	internal TimeSpan DefaultWaitTimeout
	{
		get => owningContext?.DefaultWaitTimeout ?? standaloneDefaultWaitTimeout;
		set
		{
			if (owningContext is not null)
				owningContext.DefaultWaitTimeout = value;
			else
				standaloneDefaultWaitTimeout = value;
		}
	}

	/// <summary>
	/// Attaches this instance to its owning <see cref="BunitContext"/>.
	/// </summary>
	internal void AttachToContext(BunitContext context) => owningContext = context;

	/// <summary>
	/// Initializes a new instance of the <see cref="BunitJSInterop"/> class.
	/// </summary>
	public BunitJSInterop()
	{
		mode = JSRuntimeMode.Strict;
		JSRuntime = new BunitJSRuntime(this);
		AddCustomHandlers();
	}

	/// <summary>
	/// Adds an invocation handler to bUnit's JSInterop. Can be used to register
	/// custom invocation handlers.
	/// </summary>
	public void AddInvocationHandler<TResult>(JSRuntimeInvocationHandlerBase<TResult> handler)
	{
		ArgumentNullException.ThrowIfNull(handler);

		var resultType = typeof(TResult);

		if (!handlers.ContainsKey(resultType))
			handlers.Add(resultType, new List<object>());

		handlers[resultType].Add(handler);
		handler.AttachTo(this);
	}

	internal ValueTask<TValue> HandleInvocation<TValue>(JSRuntimeInvocation invocation)
	{
		RegisterInvocation(invocation);
		return TryHandlePlannedInvocation<TValue>(invocation)
			?? new ValueTask<TValue>(default(TValue)!);
	}

	private ValueTask<TValue>? TryHandlePlannedInvocation<TValue>(JSRuntimeInvocation invocation)
	{
		ValueTask<TValue>? result = default;

		if (TryGetHandlerFor<TValue>(invocation) is JSRuntimeInvocationHandlerBase<TValue> handler)
		{
			var task = handler.HandleAsync(invocation);
			result = new ValueTask<TValue>(task);
		}
		else if (Mode == JSRuntimeMode.Strict)
		{
			throw new JSRuntimeUnhandledInvocationException(invocation);
		}

		return result;
	}

	internal virtual void RegisterInvocation(JSRuntimeInvocation invocation)
	{
		Invocations.RegisterInvocation(invocation);
	}

	internal JSRuntimeInvocationHandlerBase<TResult>? TryGetHandlerFor<TResult>(JSRuntimeInvocation invocation, Predicate<JSRuntimeInvocationHandlerBase<TResult>>? handlerPredicate = null)
	{
		var resultType = typeof(TResult);
		handlerPredicate ??= _ => true;

		if (!handlers.TryGetValue(resultType, out var plannedInvocations))
		{
			return default;
		}

		// Search from the latest added handler for a result type
		// and find the first handler that can handle an invocation
		// and is not a catch all handler.
		for (int i = plannedInvocations.Count - 1; i >= 0; i--)
		{
			var candidate = (JSRuntimeInvocationHandlerBase<TResult>)plannedInvocations[i];

			if (!candidate.IsCatchAllHandler && handlerPredicate(candidate) && candidate.CanHandle(invocation))
			{
				return candidate;
			}
		}

		// If none of the non catch all handlers can handle,
		// search for the latest added handler catch all handler that can, if any.
		for (int i = plannedInvocations.Count - 1; i >= 0; i--)
		{
			var candidate = (JSRuntimeInvocationHandlerBase<TResult>)plannedInvocations[i];

			if (candidate.IsCatchAllHandler && handlerPredicate(candidate) && candidate.CanHandle(invocation))
			{
				return candidate;
			}
		}

		return default;
	}

	private void AddCustomHandlers()
	{
		AddInvocationHandler(new FocusAsyncInvocationHandler());
		AddInvocationHandler(new VirtualizeJSRuntimeInvocationHandler());
		AddInvocationHandler(new LooseModeJSObjectReferenceInvocationHandler(this));
		AddInvocationHandler(new InputFileInvocationHandler());
		AddInvocationHandler(new FocusOnNavigateHandler());
		AddInvocationHandler(new NavigationLockDisableNavigationPromptInvocationHandler());
		AddInvocationHandler(new NavigationLockEnableNavigationPromptInvocationHandler());
	}
}
