using Microsoft.Extensions.DependencyInjection;
using System;

namespace Egil.RazorComponents.Testing
{
    public sealed class TestServiceProvider : IServiceProvider, IDisposable
    {
        private readonly ServiceCollection _serviceCollection = new ServiceCollection();
        private ServiceProvider? _serviceProvider;

        public bool IsProviderInitialized => _serviceProvider is { };

        public TestServiceProvider AddService<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory)
            where TService : class
            where TImplementation : class, TService
        {
            CheckInitializedAndThrow();
            _serviceCollection.AddSingleton<TService, TImplementation>(implementationFactory);
            return this;
        }

        public TestServiceProvider AddService<TService>(Func<IServiceProvider, TService> implementationFactory) where TService : class
        {
            CheckInitializedAndThrow();
            _serviceCollection.AddSingleton<TService>(implementationFactory);
            return this;
        }

        public TestServiceProvider AddService<TService>() where TService : class
        {
            CheckInitializedAndThrow();
            _serviceCollection.AddSingleton<TService>();
            return this;
        }

        public TestServiceProvider AddService(Type serviceType)
        {
            CheckInitializedAndThrow();
            _serviceCollection.AddSingleton(serviceType);
            return this;
        }

        public TestServiceProvider AddService<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            CheckInitializedAndThrow();
            _serviceCollection.AddSingleton<TService, TImplementation>();
            return this;
        }

        public TestServiceProvider AddService(Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            CheckInitializedAndThrow();
            _serviceCollection.AddSingleton(serviceType, implementationFactory);
            return this;
        }

        public TestServiceProvider AddService(Type serviceType, Type implementationType)
        {
            CheckInitializedAndThrow();
            _serviceCollection.AddSingleton(serviceType, implementationType);
            return this;
        }

        public TestServiceProvider AddService<TService>(TService implementationInstance) where TService : class
        {
            CheckInitializedAndThrow();
            _serviceCollection.AddSingleton<TService>(implementationInstance);
            return this;
        }

        public TestServiceProvider AddService(Type serviceType, object implementationInstance)
        {
            CheckInitializedAndThrow();
            _serviceCollection.AddSingleton(serviceType, implementationInstance);
            return this;
        }

        public TService GetService<TService>()
        {
            if (_serviceProvider is null)
                _serviceProvider = _serviceCollection.BuildServiceProvider();
            return _serviceProvider.GetService<TService>();
        }
        public object GetService(Type serviceType)
        {
            if (_serviceProvider is null)
                _serviceProvider = _serviceCollection.BuildServiceProvider();

            return _serviceProvider.GetService(serviceType);
        }

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
