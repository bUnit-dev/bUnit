using Xunit;

namespace Bunit.Docs.Samples;

public class TwoWayBindingTest : BunitContext
{
  [Fact]
  public void Test()
  {
    var currentValue = string.Empty;

    Render<TwoWayBinding>(parameters =>
      parameters.Bind(
        p => p.Value,
        currentValue,
        newValue => currentValue = newValue));
  }
}