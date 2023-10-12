using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bunit.Docs.Samples;

public sealed class CustomServiceProvider : IServiceProvider, IServiceScopeFactory, IServiceScope {
  private readonly IServiceProvider _serviceProvider;

  public CustomServiceProvider(IServiceCollection serviceDescriptors)
    => _serviceProvider = serviceDescriptors.BuildServiceProvider();

  public object GetService(Type serviceType) {
    if (serviceType == typeof(IServiceScope) || serviceType == typeof(IServiceScopeFactory))
      return this;

    if (serviceType == typeof(DummyService))
      return new DummyService();

    return _serviceProvider.GetService(serviceType);
  }

  void IDisposable.Dispose() { }
  public IServiceScope CreateScope() => this;
  IServiceProvider IServiceScope.ServiceProvider => this;

}

public sealed class CustomServiceProviderFactoryContainerBuilder {
  private readonly IServiceCollection _serviceDescriptors;

  public CustomServiceProviderFactoryContainerBuilder(IServiceCollection serviceDescriptors)
    => this._serviceDescriptors = serviceDescriptors;

  public IServiceProvider Build()
    => new CustomServiceProvider(_serviceDescriptors);
}

public sealed class CustomServiceProviderFactory : IServiceProviderFactory<CustomServiceProviderFactoryContainerBuilder> {
  public CustomServiceProviderFactoryContainerBuilder CreateBuilder(IServiceCollection services)
    => new CustomServiceProviderFactoryContainerBuilder(services);

  public IServiceProvider CreateServiceProvider(CustomServiceProviderFactoryContainerBuilder containerBuilder)
    => containerBuilder.Build();
}