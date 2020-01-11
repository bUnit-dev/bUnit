using Egil.RazorComponents.Testing.Mocking.JSInterop;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Egil.RazorComponents.Testing
{
    /// <summary>
    /// Represents a <see cref="IServiceProvider"/> and <see cref="IServiceCollection"/> 
    /// as a single type used for test purposes.
    /// </summary>
    public sealed class TestServiceProvider : IServiceProvider, IDisposable
    {
        private readonly ServiceCollection _serviceCollection = new ServiceCollection();
        private ServiceProvider? _serviceProvider;

        public TestServiceProvider()
        {
            _serviceCollection.AddSingleton<IJSRuntime, DefaultJsRuntime>();
        }

        /// <summary>
        /// Gets whether this <see cref="TestServiceProvider"/> has been initialized, and 
        /// no longer will accept calls to the <c>AddService</c>'s methods.
        /// </summary>
        public bool IsProviderInitialized => _serviceProvider is { };

        /// <summary>
        /// Adds a singleton service of the type specified in TService with an implementation
        /// type specified in TImplementation using the factory specified in implementationFactory
        /// to this <see cref="TestServiceProvider"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public TestServiceProvider AddService<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory)
            where TService : class
            where TImplementation : class, TService
        {
            CheckInitializedAndThrow();
            _serviceCollection.AddSingleton<TService, TImplementation>(implementationFactory);
            return this;
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <typeparamref name="TService"/> with a factory specified
        /// in <paramref name="implementationFactory"/> to this <see cref="TestServiceProvider"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public TestServiceProvider AddService<TService>(Func<IServiceProvider, TService> implementationFactory) where TService : class
        {
            CheckInitializedAndThrow();
            _serviceCollection.AddSingleton<TService>(implementationFactory);
            return this;
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <typeparamref name="TService"/> to this <see cref="TestServiceProvider"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public TestServiceProvider AddService<TService>() where TService : class
        {
            CheckInitializedAndThrow();
            _serviceCollection.AddSingleton<TService>();
            return this;
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <paramref name="serviceType"/> to this <see cref="TestServiceProvider"/>.
        /// </summary>
        /// <param name="serviceType">The type of the service to register and the implementation to use.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public TestServiceProvider AddService(Type serviceType)
        {
            CheckInitializedAndThrow();
            _serviceCollection.AddSingleton(serviceType);
            return this;
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <typeparamref name="TService"/> with an implementation
        /// type specified in <typeparamref name="TService"/> to this <see cref="TestServiceProvider"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public TestServiceProvider AddService<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            CheckInitializedAndThrow();
            _serviceCollection.AddSingleton<TService, TImplementation>();
            return this;
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <paramref name="serviceType"/> with a factory
        /// specified in <paramref name="implementationFactory"/> to this <see cref="TestServiceProvider"/>.
        /// </summary>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns></returns>
        public TestServiceProvider AddService(Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            CheckInitializedAndThrow();
            _serviceCollection.AddSingleton(serviceType, implementationFactory);
            return this;
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <paramref name="serviceType"/> with an implementation
        /// of the type specified in <paramref name="implementationType"/> to this <see cref="TestServiceProvider"/>.
        /// </summary>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>        
        public TestServiceProvider AddService(Type serviceType, Type implementationType)
        {
            CheckInitializedAndThrow();
            _serviceCollection.AddSingleton(serviceType, implementationType);
            return this;
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <typeparamref name="TService"/> with an instance specified in 
        /// <paramref name="implementationInstance"/> to this <see cref="TestServiceProvider"/>.
        /// </summary>
        /// <param name="implementationInstance">The instance of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public TestServiceProvider AddService<TService>(TService implementationInstance) where TService : class
        {
            CheckInitializedAndThrow();
            _serviceCollection.AddSingleton<TService>(implementationInstance);
            return this;
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <paramref name="serviceType"/> with an instance specified in 
        /// <paramref name="implementationInstance"/> to this <see cref="TestServiceProvider"/>.
        /// </summary>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationInstance">The instance of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public TestServiceProvider AddService(Type serviceType, object implementationInstance)
        {
            CheckInitializedAndThrow();
            _serviceCollection.AddSingleton(serviceType, implementationInstance);
            return this;
        }

        /// <summary>
        /// Get service of type T from the test provider.
        /// </summary>
        /// <typeparam name="TService">The type of service object to get.</typeparam>
        /// <returns>A service object of type T or null if there is no such service.</returns>        
        public TService GetService<TService>()
        {
            if (_serviceProvider is null)
                _serviceProvider = _serviceCollection.BuildServiceProvider();
            return _serviceProvider.GetService<TService>();
        }

        /// <inheritdoc/>
        public object GetService(Type serviceType)
        {
            if (_serviceProvider is null)
                _serviceProvider = _serviceCollection.BuildServiceProvider();

            return _serviceProvider.GetService(serviceType);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _serviceProvider?.Dispose();
        }

        private void CheckInitializedAndThrow()
        {
            if (IsProviderInitialized)
                throw new InvalidOperationException("New services cannot be added to provider after it has been initialized.");
        }
    }
}
