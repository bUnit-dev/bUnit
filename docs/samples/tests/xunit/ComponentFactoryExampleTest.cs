using Microsoft.AspNetCore.Components.Web;
using Xunit;
using Bunit;

namespace Bunit.Docs.Samples
{
  public class ComponentFactoryExampleTest : BunitContext
  {
    [Fact]
    public void ReplacesFooWithBarDuringTest()
    {
      // Arrange
      ComponentFactories.Add(new FooBarComponentFactory());

      // Act
      var cut = Render<Wrapper>(parameters => parameters
        .AddChildContent<Foo>());

      // Assert that there are no <Foo> in render tree,
      // but there is one <Bar> in the render tree.
      Assert.Empty(cut.FindComponents<Foo>());
      Assert.Equal(1, cut.FindComponents<Bar>().Count);
    }
  }
}