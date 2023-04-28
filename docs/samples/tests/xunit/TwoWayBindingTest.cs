using Xunit;

namespace Bunit.Docs.Samples;

public class TwoWayBindingTest : TestContext
{
  [Fact]
  public void Test()
  {
    var currentValue = string.Empty;

    RenderComponent<TwoWayBinding>(parameters =>
      parameters.Bind(
        p => p.Value,
        currentValue,
        newValue => currentValue = newValue));
  }
}