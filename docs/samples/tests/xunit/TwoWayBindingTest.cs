using Xunit;

namespace Bunit.Docs.Samples;

public class TwoWayBindingTest
{
  [Fact]
  public void Test()
  {
    using var ctx = new TestContext();
    var currentValue = string.Empty;

    ctx.RenderComponent<TwoWayBinding>(parameters =>
      parameters.Bind(
        p => p.Value,
        currentValue,
        newValue => currentValue = newValue));
  }
}