using System.Collections;

namespace Bunit;

/// <summary>
/// Represents a <see cref="IServiceProvider"/> and <see cref="IServiceCollection"/>
/// as a single type used for test purposes.
/// </summary>
public sealed class TestServiceProvider : IServiceProvider, IServiceCollection, IDisposable
{
	private readonly static ServiceProviderOptions DefaultServiceProviderOptions = new() { ValidateScopes = true };
	private readonly IServiceCollection serviceCollection;
	private IServiceProvider? rootServiceProvider;
	private IServiceScope? serviceScope;
	private IServiceProvider? serviceProvider;
	private IServiceProvider? fallbackServiceProvider;
	private ServiceProviderOptions options = DefaultServiceProviderOptions;

	/// <summary>
	/// Gets a value indicating whether this <see cref="TestServiceProvider"/> has been initialized, and
	/// no longer will accept calls to the <c>AddService</c>'s methods.
	/// </summary>
	public bool IsProviderInitialized => serviceProvider is not null;

	/// <inheritdoc/>
	public int Count => serviceCollection.Count;

	/// <inheritdoc/>
	public bool IsReadOnly => IsProviderInitialized || serviceCollection.IsReadOnly;

	/// <inheritdoc/>
	public ServiceDescriptor this[int index]
	{
		get => serviceCollection[index];
		set
		{
			CheckInitializedAndThrow();
			serviceCollection[index] = value;
		}
	}

	/// <summary>
	/// Gets or sets the <see cref="ServiceProviderOptions"/> used when the <see cref="IServiceProvider"/> is created.
	/// </summary>
	public ServiceProviderOptions Options
	{
		get => options;
		set => options = value ?? DefaultServiceProviderOptions;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="TestServiceProvider"/> class
	/// and sets its service collection to the provided <paramref name="initialServiceCollection"/>, if any.
	/// </summary>
	public TestServiceProvider(IServiceCollection? initialServiceCollection = null)
		: this(initialServiceCollection ?? new ServiceCollection(), initializeProvider: false)
	{
	}

	private TestServiceProvider(IServiceCollection initialServiceCollection, bool initializeProvider)
	{
		serviceCollection = initialServiceCollection;
		if (initializeProvider)
			serviceProvider = serviceCollection.BuildServiceProvider();
	}

	/// <summary>
	/// Add a fall back service provider that provides services when the default returns null.
	/// </summary>
	/// <param name="serviceProvider">The fallback service provider.</param>
	public void AddFallbackServiceProvider(IServiceProvider serviceProvider)
		=> fallbackServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

	/// <summary>
	/// Get service of type T from the test provider.
	/// </summary>
	/// <typeparam name="TService">The type of service object to get.</typeparam>
	/// <returns>A service object of type T or null if there is no such service.</returns>
	public TService? GetService<TService>() => (TService?)GetService(typeof(TService))!;

#if NETSTANDARD2_1
	/// <inheritdoc/>
	[return: MaybeNull]
	public object GetService(Type serviceType)
		=> GetServiceInternal(serviceType);
#else
	/// <inheritdoc/>
	public object? GetService(Type serviceType)
		=> GetServiceInternal(serviceType);
#endif

	private object? GetServiceInternal(Type serviceType)
	{
		if (serviceProvider is null)
		{
			serviceCollection.AddSingleton<TestServiceProvider>(this);
			rootServiceProvider = serviceCollection.BuildServiceProvider(options);
			serviceScope = rootServiceProvider.CreateScope();
			serviceProvider = serviceScope.ServiceProvider;
		}

		var result = serviceProvider.GetService(serviceType);

		if (result is null && fallbackServiceProvider is not null)
			result = fallbackServiceProvider.GetService(serviceType);

		return result;
	}

	/// <inheritdoc/>
	public IEnumerator<ServiceDescriptor> GetEnumerator() => serviceCollection.GetEnumerator();

	/// <inheritdoc/>
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	/// <inheritdoc/>
	public void Dispose()
	{
		(serviceScope as IAsyncDisposable)?.DisposeAsync().AsTask().GetAwaiter().GetResult();
		serviceScope?.Dispose();
		(rootServiceProvider as IAsyncDisposable)?.DisposeAsync().AsTask().GetAwaiter().GetResult();
		(rootServiceProvider as IDisposable)?.Dispose();
	}

	/// <inheritdoc/>
	public int IndexOf(ServiceDescriptor item) => serviceCollection.IndexOf(item);

	/// <inheritdoc/>
	public void Insert(int index, ServiceDescriptor item)
	{
		CheckInitializedAndThrow();
		serviceCollection.Insert(index, item);
	}

	/// <inheritdoc/>
	public void RemoveAt(int index)
	{
		CheckInitializedAndThrow();
		serviceCollection.RemoveAt(index);
	}

	/// <inheritdoc/>
	public void Add(ServiceDescriptor item)
	{
		CheckInitializedAndThrow();
		serviceCollection.Add(item);
	}

	/// <inheritdoc/>
	public void Clear()
	{
		CheckInitializedAndThrow();
		serviceCollection.Clear();
	}

	/// <inheritdoc/>
	public bool Contains(ServiceDescriptor item) => serviceCollection.Contains(item);

	/// <inheritdoc/>
	public void CopyTo(ServiceDescriptor[] array, int arrayIndex) => serviceCollection.CopyTo(array, arrayIndex);

	/// <inheritdoc/>
	public bool Remove(ServiceDescriptor item)
	{
		CheckInitializedAndThrow();
		return serviceCollection.Remove(item);
	}

	private void CheckInitializedAndThrow()
	{
		if (IsProviderInitialized)
		{
			throw new InvalidOperationException(
				$"New services/implementations cannot be registered with the " +
				$"Services provider in a TestContext, after the first services has been retrieved " +
				$"from it using e.g. the GetService or GetRequiredService methods. " +
				$"This typically happens when a component is rendered, so make " +
				$"sure all services are added before that.");
		}
	}
}
