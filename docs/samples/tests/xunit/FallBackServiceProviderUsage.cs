using Xunit;
using Bunit;

namespace Bunit.Docs.Samples;

public class FallBackServiceProviderUsageExample : TestContext
{
  [Fact]
  public void FallBackServiceProviderReturns()
  {
    Services.SetFallbackServiceProvider(new FallbackServiceProvider());

    var dummyService = Services.GetService<DummyService>();

    Assert.NotNull(dummyService);
  }
}