using Xunit;
using Bunit;

namespace Bunit.Docs.Samples;

public class FallBackServiceProviderUsageExample : TestContext
{
  [Fact]
  public void FallBackServiceProviderReturns()
  {
    Services.AddFallbackServiceProvider(new FallbackServiceProvider());

    var dummyService = Services.GetService<DummyService>();

    Assert.NotNull(dummyService);
  }
}