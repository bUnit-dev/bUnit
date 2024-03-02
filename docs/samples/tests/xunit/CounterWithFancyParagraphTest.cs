using Xunit;
using Bunit;
using Moq;
using NSubstitute;

namespace Bunit.Docs.Samples
{
  public class CounterWithFancyParagraphTest : TestContext
  {
    [Fact]
    public void CounterShouldIncrementWhenClicked_Moq()
    {
      Mock<FancyParagraph> mock = new Mock<FancyParagraph>();
      FancyParagraph mockComponent = mock.Object;

      ComponentFactories.Add(mock.Object);

      var cut = RenderComponent<Counter>();

      cut.Find("button").Click();

      cut.Find("p").MarkupMatches("<p>Current count: 1</p>");
    }

    [Fact]
    public void CounterShouldIncrementWhenClicked_NSubstitute()
    {
      FancyParagraph mockComponent = Substitute.For<FancyParagraph>();

      ComponentFactories.Add(mockComponent);

      var cut = RenderComponent<Counter>();

      cut.Find("button").Click();

      cut.Find("p").MarkupMatches("<p>Current count: 1</p>");
    }
  }
}
