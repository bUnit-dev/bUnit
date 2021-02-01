using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Bunit
{
	/// <summary>
	/// Represents a <see cref="IServiceProvider"/> and <see cref="IServiceCollection"/>
	/// as a single type used for test purposes.
	/// </summary>
	public sealed class TestServiceProvider : IServiceProvider, IServiceCollection, IDisposable
	{
		private readonly IServiceCollection serviceCollection;
		private ServiceProvider? serviceProvider;

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
		/// Get service of type T from the test provider.
		/// </summary>
		/// <typeparam name="TService">The type of service object to get.</typeparam>
		/// <returns>A service object of type T or null if there is no such service.</returns>
		public TService GetService<TService>() => (TService)GetService(typeof(TService));

		/// <inheritdoc/>
		public object GetService(Type serviceType)
		{
			if (serviceProvider is null)
				serviceProvider = serviceCollection.BuildServiceProvider();

			return serviceProvider.GetService(serviceType);
		}

		/// <inheritdoc/>
		public IEnumerator<ServiceDescriptor> GetEnumerator() => serviceCollection.GetEnumerator();

		/// <inheritdoc/>
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		/// <inheritdoc/>
		public void Dispose()
		{
			if (serviceProvider is null) return;

			var disposedTask = serviceProvider.DisposeAsync().AsTask();

			if (!disposedTask.IsCompleted)
				disposedTask.GetAwaiter().GetResult();

			serviceProvider.Dispose();
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
				throw new InvalidOperationException("Services cannot be added to provider after it has been initialized.");
		}
	}
}
