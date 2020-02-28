using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit.SampleApp.Components;
using Xunit;

namespace Bunit.SampleApp.CodeOnlyTests.Components
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
            cut.MarkupMatches($@"<aside class=""{cssClass}""><header>{header}</header></aside>");
        }

        [Fact(DisplayName = "Passing new parameters to Aside updates the rendered HTML correctly")]
        public void Test002()
        {
            // Arrange - initial render of Aside
            var cut = RenderComponent<Aside>();

            // Act - set the Header parameter and re-render the CUT
            cut.SetParametersAndRender((nameof(Aside.Header), "HEADER"));

            // Assert - Check that we have exactly one change since the first render, 
            // and that it is an addition to the DOM tree
            cut.GetChangesSinceFirstRender()
                .ShouldHaveSingleChange()
                .ShouldBeAddition("<header>HEADER</header>");

            // Arrange - Create a snapshot of the current rendered HTML for later comparisons
            cut.SaveSnapshot();

            // Act - Set the Header parameter to null again and re-render
            cut.SetParametersAndRender((nameof(Aside.Header), null));

            // Assert - Check that we have exactly one change since compared with the snapshot we took, 
            // and that it is an addition to the DOM tree.
            cut.GetChangesSinceSnapshot()
                .ShouldHaveSingleChange()
                .ShouldBeRemoval("<header>HEADER</header>");
        }

        [Fact(DisplayName = "Aside should render child markup content correctly")]
        public void Test003()
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
            cut.MarkupMatches($@"<aside>{content}</aside>");
        }

        [Fact(DisplayName = "Aside should render a child component correctly")]
        public void Test004()
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
            cut.MarkupMatches($@"<aside>
                                    <header>{outerAsideHeader}</header>
                                    <aside>
                                        <header>{nestedAsideHeader}</header>
                                    </aside>
                                 </aside>");
        }
    }
}
