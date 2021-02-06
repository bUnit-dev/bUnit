using Xunit;
using Bunit;

namespace Bunit.Docs.Samples
{
  public class FallBackServiceProviderUssageExample
  {
    [Fact]
    public void FallBackServiceProviderReturns()
    {
      using var ctx = new TestContext();
      ctx.Services.AddFallbackServiceProvider(new FallbackServiceProvider());
      
      var dummyService = ctx.Services.GetService<DummyService>();

      Assert.NotNull(dummyService);
    }
  }
}