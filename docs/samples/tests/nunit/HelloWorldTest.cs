namespace Bunit.Docs.Samples;

using Bunit;
using NUnit.Framework;

public class HelloWorldTest : BunitTestContext
{
    [Test]
    public void HelloWorldComponentRendersCorrectly()
    {
        // Act
        var cut = Render<HelloWorld>();

        // Assert
        cut.MarkupMatches("<h1>Hello world from Blazor</h1>");
    }
}