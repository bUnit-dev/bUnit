using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection;

namespace Bunit
{
	/// <summary>
	/// Represents a <see cref="IServiceProvider"/> and <see cref="IServiceCollection"/> 
	/// as a single type used for test purposes.
	/// </summary>
	[SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix")]
	public sealed class TestServiceProvider : IServiceProvider, IServiceCollection, IDisposable
	{
		private readonly IServiceCollection _serviceCollection;
		private ServiceProvider? _serviceProvider;

		/// <summary>
		/// Gets a reusable default test service provider.
		/// </summary>
		public static readonly IServiceProvider Default = new TestServiceProvider(new ServiceCollection(), true);

		/// <summary>
		/// Gets whether this <see cref="TestServiceProvider"/> has been initialized, and 
		/// no longer will accept calls to the <c>AddService</c>'s methods.
		/// </summary>
		public bool IsProviderInitialized => _serviceProvider is { };

		/// <inheritdoc/>
		public int Count => _serviceCollection.Count;

		/// <inheritdoc/>
		public bool IsReadOnly => IsProviderInitialized || _serviceCollection.IsReadOnly;

		/// <inheritdoc/>
		public ServiceDescriptor this[int index]
		{
			get => _serviceCollection[index];
			set
			{
				CheckInitializedAndThrow();
				_serviceCollection[index] = value;
			}
		}

		/// <summary>
		/// Creates an instance of the <see cref="TestServiceProvider"/> and sets its service collection to the
		/// provided <paramref name="initialServiceCollection"/>, if any.
		/// </summary>
		/// <param name="initialServiceCollection"></param>
		public TestServiceProvider(IServiceCollection? initialServiceCollection = null) : this(initialServiceCollection ?? new ServiceCollection(), false)
		{
		}

		private TestServiceProvider(IServiceCollection initialServiceCollection, bool initializeProvider)
		{
			_serviceCollection = initialServiceCollection;
			if (initializeProvider)
				_serviceProvider = _serviceCollection.BuildServiceProvider();
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
			if (_serviceProvider is null)
				_serviceProvider = _serviceCollection.BuildServiceProvider();

			return _serviceProvider.GetService(serviceType);
		}

		/// <inheritdoc/>
		public IEnumerator<ServiceDescriptor> GetEnumerator() => _serviceCollection.GetEnumerator();

		/// <inheritdoc/>
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		/// <inheritdoc/>
		public void Dispose()
		{
			_serviceProvider?.Dispose();
		}

		/// <inheritdoc/>
		public int IndexOf(ServiceDescriptor item) => _serviceCollection.IndexOf(item);
		/// <inheritdoc/>
		public void Insert(int index, ServiceDescriptor item)
		{
			CheckInitializedAndThrow();
			_serviceCollection.Insert(index, item);
		}
		/// <inheritdoc/>
		public void RemoveAt(int index)
		{
			CheckInitializedAndThrow();
			_serviceCollection.RemoveAt(index);
		}

		/// <inheritdoc/>
		public void Add(ServiceDescriptor item)
		{
			CheckInitializedAndThrow();
			_serviceCollection.Add(item);
		}
		/// <inheritdoc/>
		public void Clear()
		{
			CheckInitializedAndThrow();
			_serviceCollection.Clear();
		}

		/// <inheritdoc/>
		public bool Contains(ServiceDescriptor item) => _serviceCollection.Contains(item);
		/// <inheritdoc/>
		public void CopyTo(ServiceDescriptor[] array, int arrayIndex) => _serviceCollection.CopyTo(array, arrayIndex);
		/// <inheritdoc/>
		public bool Remove(ServiceDescriptor item)
		{
			CheckInitializedAndThrow();
			return _serviceCollection.Remove(item);
		}

		private void CheckInitializedAndThrow()
		{
			if (IsProviderInitialized)
				throw new InvalidOperationException("Services cannot be added to provider after it has been initialized.");
		}
	}
}
