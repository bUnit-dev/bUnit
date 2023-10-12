using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Bunit.Docs.Samples;
public class CustomServiceProviderFactoryUsage : TestContext {
  [Fact]
  public void CustomServiceProviderViaFactoryReturns() {
    Services.UseServiceProviderFactory(new CustomServiceProviderFactory());

    var dummyService = Services.GetService<DummyService>();

    Assert.NotNull(dummyService);
  }

  [Fact]
  public void CustomServiceProviderViaDelegateReturns() {
    Services.UseServiceProviderFactory(x => new CustomServiceProvider(x));

    var dummyService = Services.GetService<DummyService>();

    Assert.NotNull(dummyService);
  }

  [Fact]
  public void AutofacServiceProviderViaFactoryReturns() {
    void ConfigureContainer(ContainerBuilder containerBuilder) {
      containerBuilder
        .RegisterType<DummyService>()
        .AsSelf();
    }

    Services.UseServiceProviderFactory(new AutofacServiceProviderFactory(ConfigureContainer));

    //get a service which was installed in the Autofac ContainerBuilder

    var dummyService = Services.GetService<DummyService>();

    Assert.NotNull(dummyService);

    //get a service which was installed in the bUnit ServiceCollection

    var testContextBase = Services.GetService<TestContextBase>();

    Assert.NotNull(testContextBase);
    Assert.Equal(this, testContextBase);
  }

  [Fact]
  public void AutofacServiceProviderViaDelegateReturns() {
    ILifetimeScope ConfigureContainer(IServiceCollection services) {
      var containerBuilder = new ContainerBuilder();

      containerBuilder
        .RegisterType<DummyService>()
        .AsSelf();

      containerBuilder.Populate(services);

      return containerBuilder.Build();
    }

    Services.UseServiceProviderFactory(x => new AutofacServiceProvider(ConfigureContainer(x)));

    //get a service which was installed in the Autofac ContainerBuilder

    var dummyService = Services.GetService<DummyService>();

    Assert.NotNull(dummyService);

    //get a service which was installed in the bUnit ServiceCollection

    var testContextBase = Services.GetService<TestContextBase>();

    Assert.NotNull(testContextBase);
    Assert.Equal(this, testContextBase);
  }
}
