using Xunit;
using Bunit;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

using static Bunit.ComponentParameterFactory;

namespace Bunit.Docs.Samples
{
  public class VerifyMarkupExamples
  {
    [Fact]
    public void RawMarkupVerify()
    {
      using var ctx = new TestContext();

      var cut = ctx.RenderComponent<HelloWorld>();

      var renderedMarkup = cut.Markup;
      Assert.Equal("<h1>Hello world from Blazor</h1>", renderedMarkup);
    }

    [Fact]
    public void MarkupMatchesOnRenderedFragment()
    {
      using var ctx = new TestContext();

      var cut = ctx.RenderComponent<Heading>();

      cut.MarkupMatches(@"<h3 id=""heading-1337"" required>
                            Heading text
                            <small class=""mark text-muted"">Secondary text</small>
                          </h3>");
    }

    [Fact]
    public void MarkupMatchesOnNode()
    {
      using var ctx = new TestContext();

      var cut = ctx.RenderComponent<Heading>();

      var smallElm = cut.Find("small");
      smallElm.MarkupMatches(@"<small class=""mark text-muted"">Secondary text</small>");
    }

    // [Fact]
    // public void MarkupMatchesOnTextNode()
    // {
    //   using var ctx = new TestContext();

    //   var cut = ctx.RenderComponent<Heading>();

    //   var smallElmText = cut.Find("small").TextContent;
    //   smallElmText.MarkupMatches("Secondary text");
    // }

    [Fact]
    public void FindAndFindAll()
    {
      using var ctx = new TestContext();

      var cut = ctx.RenderComponent<FancyTable>();

      var tableCaption = cut.Find("caption");
      var tableCells = cut.FindAll("td:first-child");

      Assert.Empty(tableCaption.Attributes);
      Assert.Equal(2, tableCells.Count);
      Assert.All(tableCells, td => td.HasAttribute("style"));
    }

    [Fact]
    public void GetChangesSinceFirstRenderTest()
    {
      using var ctx = new TestContext();
      var cut = ctx.RenderComponent<Counter>();

      // Act - increment the counter
      cut.Find("button").Click();

      // Assert - find differences between first render and click
      var diffs = cut.GetChangesSinceFirstRender();

      // Only expect there to be one change      
      var diff = diffs.ShouldHaveSingleChange();
      // and that change should be a text 
      // change to "Current count: 1"
      diff.ShouldBeTextChange("Current count: 1");
    }

    [Fact]
    public void GetChangesSinceX()
    {
      // Arrange
      using var ctx = new TestContext();
      var cut = ctx.RenderComponent<CheckList>();
      var inputField = cut.Find("input");

      // Add first item
      inputField.Change("First item");
      inputField.KeyUp(key: "Enter");

      // Assert that first item was added correctly
      var diffs = cut.GetChangesSinceFirstRender();
      diffs.ShouldHaveSingleChange()
        .ShouldBeAddition("<li>First item</li>");

      // Save snapshot of current DOM nodes
      cut.SaveSnapshot();

      // Add a second item
      inputField.Change("Second item");
      inputField.KeyUp(key: "Enter");

      // Assert that both first and second item was added
      // since the first render
      diffs = cut.GetChangesSinceFirstRender();
      diffs.ShouldHaveChanges(
        diff => diff.ShouldBeAddition("<li>First item</li>"),
        diff => diff.ShouldBeAddition("<li>Second item</li>")
      );

      // Assert that only the second item was added 
      // since the call to SaveSnapshot()
      diffs = cut.GetChangesSinceSnapshot();
      diffs.ShouldHaveSingleChange()
        .ShouldBeAddition("<li>Second item</li>");

      // Save snapshot again of current DOM nodes
      cut.SaveSnapshot();

      // Click last item to remove it from list
      cut.Find("li:last-child").Click();

      // Assert that the second item was removed 
      // since the call to SaveSnapshot()
      diffs = cut.GetChangesSinceSnapshot();
      diffs.ShouldHaveSingleChange()
        .ShouldBeRemoval("<li>Second item</li>");
    }
  }
}