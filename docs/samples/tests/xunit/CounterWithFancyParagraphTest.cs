#if NET5_0_OR_GREATER
using Xunit;
using Bunit;
using Moq;
using NSubstitute;

namespace Bunit.Docs.Samples
{
  public class CounterWithFancyParagraphTest
  {
    [Fact]
    public void CounterShouldIncrementWhenClicked_Moq()
    {
      using var ctx = new TestContext();      
      Mock<FancyParagraph> mock = new Mock<FancyParagraph>();
      FancyParagraph mockComponent = mock.Object;

      ctx.ComponentFactories.Add(mock.Object);
        
      var cut = ctx.RenderComponent<Counter>();

      cut.Find("button").Click();

      cut.Find("p").MarkupMatches("<p>Current count: 1</p>");
    }

    [Fact]
    public void CounterShouldIncrementWhenClicked_NSubstitute()
    {
      using var ctx = new TestContext();      
      FancyParagraph mockComponent = Substitute.For<FancyParagraph>();

      ctx.ComponentFactories.Add(mockComponent);
        
      var cut = ctx.RenderComponent<Counter>();

      cut.Find("button").Click();

      cut.Find("p").MarkupMatches("<p>Current count: 1</p>");
    }
  }
}
#endif