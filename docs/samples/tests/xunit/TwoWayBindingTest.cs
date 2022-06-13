using Xunit;

namespace Bunit.Docs.Samples;

public class TwoWayBindingTest
{
  [Fact]
  public void Test()
  {
    using var ctx = new TestContext();
    var value = string.Empty;

    ctx.RenderComponent<TwoWayBinding>(parameters =>
      parameters.Bind(
        p => p.Value,
        value,
        newValue => value = newValue));
  }
}