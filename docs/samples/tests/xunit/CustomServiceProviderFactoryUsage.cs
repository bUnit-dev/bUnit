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

}
