using Xunit;
using Bunit;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Bunit.Docs.Samples;

public class ReRenderTest : BunitContext
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
  public void RenderAgainUsingRenderAgainUsingSetParametersAndRender()
  {
    // Arrange - renders the Item component
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
    // Arrange - renders the Calc component
    var cut = Render<Calc>();

    // Indirectly re-renders through the call to StateHasChanged
    // in the Calculate(x, y) method.
    cut.InvokeAsync(() => cut.Instance.Calculate(1, 2));

    cut.MarkupMatches("<output>3</output>");
  }

  [Fact]
  public async Task RendersViaInvokeAsyncWithReturnValue()
  {
    // Arrange - renders the CalcWithReturnValue component
    var cut = Render<CalcWithReturnValue>();

    // Indirectly re-renders and returns a value.
    var result = await cut.InvokeAsync(() => cut.Instance.Calculate(1, 2));

    Assert.Equal(3, result);
    cut.MarkupMatches("<output>3</output>");
  }

  [Fact]
  public async Task RendersViaInvokeAsyncWithLoading()
  {
    // Arrange - renders the CalcWithLoading component
    var cut = Render<CalcWithLoading>();

    // Indirectly re-renders and returns the task returned by Calculate().
    // The explicit <Task> here is important, otherwise the call to Calculate()
    // will be awaited automatically.
    var task = await cut.InvokeAsync<CalcWithLoading, Task>(() => cut.Instance.Calculate(1, 2));
    cut.MarkupMatches("<output>Loading</output>");

    // Wait for the task to complete.
    await task;
    cut.WaitForAssertion(() => cut.MarkupMatches("<output>3</output>"));
  }
}