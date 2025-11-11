using Xunit;
using Bunit;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Bunit.Docs.Samples;

public class SemanticHtmlTest : BunitContext
{
  [Fact]
  public void InitialHtmlIsCorrect()
  {
    // Arrange - renders the Heading component
    var cut = Render<Heading>();

    // Assert
    // Here we specify expected HTML from CUT.
    var expectedHtml = @"<h3 id:regex=""heading-\d{4}"" required>
                            Heading text
                            <small diff:ignore></small>
                           </h3>";

    // Here we use the HTML diffing library to assert that the rendered HTML
    // from CUT is semantically the same as the expected HTML string above.
    cut.MarkupMatches(expectedHtml);
  }
}