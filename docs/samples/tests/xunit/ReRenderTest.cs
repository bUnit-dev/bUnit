using Xunit;
using Bunit;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;


namespace Bunit.Docs.Samples;

public class ReRenderTest : TestContext
{
  [Fact]
  public void RenderAgainUsingRender()
  {
    // Arrange - renders the Heading component
    var cut = Render<Heading>();
    Assert.Equal(1, cut.RenderCount);

    // Re-render without new parameters
    cut.Render();

    Assert.Equal(2, cut.RenderCount);
  }

  [Fact]
  public void RenderAgainUsingSetParametersAndRender()
  {
    // Arrange - renders the Heading component
    var cut = Render<Item>(parameters => parameters
      .Add(p => p.Value, "Foo")
    );
    cut.MarkupMatches("<span>Foo</span>");

    // Re-render with new parameters
    cut.Render(parameters => parameters
      .Add(p => p.Value, "Bar")
    );

    cut.MarkupMatches("<span>Bar</span>");
  }

  [Fact]
  public void RendersViaInvokeAsync()
  {
    // Arrange - renders the Heading component
    var cut = Render<Calc>();

    // Indirectly re-renders through the call to StateHasChanged
    // in the Calculate(x, y) method.
    cut.AccessInstance(c => c.Calculate(1, 2));

    cut.MarkupMatches("<output>3</output>");
  }
}