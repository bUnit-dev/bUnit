using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Egil.RazorComponents.Testing.SampleApp.Components;
using Xunit;

namespace Egil.RazorComponents.Testing.SampleApp.CodeOnlyTests.Components
{
public class AsideTest : ComponentTestFixture
{
    [Fact(DisplayName = "Aside should render header and additional parameters correctly")]
    public void Test001()
    {
        // Arrange
        var header = "Hello testers";
        var cssClass = "some-class";

        // Act - render the Aside component with two parameters (passed as pairs of name, value tuples).
        // Note the use of the nameof operator to get the name of the Header parameter. This
        // helps keeps the test passing if the name of the parameter is refactored.
        //
        // This is equivalent to the follow Razor code:
        //
        // <Aside Header="Hello testers" class="some-class">
        // </Aside>
        var cut = RenderComponent<Aside>(
            (nameof(Aside.Header), header),
            ("class", cssClass)
        );

        // Assert - verify that the rendered HTML from the Aside component matches the expected output.
        cut.ShouldBe($@"<aside class=""{cssClass}""><header>{header}</header></aside>");
    }

        [Fact(DisplayName = "Aside should render child markup content correctly")]
        public void Test002()
        {
            // Arrange
            var content = "<p>I like simple tests and I cannot lie</p>";

            // Act 
            // Act - render the Aside component with a child content parameter,
            // which is constructed through the ChildContent helper method.
            //
            // This is equivalent to the follow Razor code:
            //
            // <Aside>
            //     <p>I like simple tests and I cannot lie</p>
            // </Aside>
            var cut = RenderComponent<Aside>(
                ChildContent(content)
            );

            // Assert - verify that the rendered HTML from the Aside component matches the expected output.
            cut.ShouldBe($@"<aside>{content}</aside>");
        }

        [Fact(DisplayName = "Aside should render a child component correctly")] 
        public void Test003()
        {
            // Arrange - set up test data
            var outerAsideHeader = "Hello outside";
            var nestedAsideHeader = "Hello inside";

            // Act - render the Aside component, passing a header to it 
            // and a component to its child content. The ChildContent helper
            // method will pass the parameters it is given to the nested Aside 
            // component. 
            //
            // This is equivalent to the follow Razor code:
            //
            // <Aside Header="Hello outside">
            //     <Aside Header="Hello inside"></Aside>
            // </Aside>
            var cut = RenderComponent<Aside>(
                (nameof(Aside.Header), outerAsideHeader),
                ChildContent<Aside>(
                    (nameof(Aside.Header), nestedAsideHeader)
                )
            );

            // Assert - verify that the rendered HTML from the Aside component matches the expected output.
            cut.ShouldBe($@"<aside>
                                <header>{outerAsideHeader}</header>
                                <aside>
                                    <header>{nestedAsideHeader}</header>
                                </aside>
                            </aside>");
        }        
    }
}
