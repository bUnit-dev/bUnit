using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Bunit.Docs.Samples;

public class WaitForComponentTest : BunitContext
{
    [Fact]
    public void WaitForComponent_WaitsForSingleComponent()
    {
        var cut = Render<AsyncComponentLoader>(parameters => parameters
            .Add(p => p.Items, ["Item 1"]));

        var listItem = cut.WaitForComponent<ListItem>();

        Assert.Equal("Item 1", listItem.Find(".list-item").TextContent);
    }

    [Fact]
    public void WaitForComponents_WaitsForMultipleComponents()
    {
        var items = new List<string> { "Item 1", "Item 2", "Item 3" };
        var cut = Render<AsyncComponentLoader>(parameters => parameters
            .Add(p => p.Items, items));

        var listItems = cut.WaitForComponents<ListItem>();

        Assert.Equal(3, listItems.Count);
        Assert.Equal("Item 1", listItems.ElementAt(0).Find(".list-item").TextContent);
        Assert.Equal("Item 2", listItems.ElementAt(1).Find(".list-item").TextContent);
        Assert.Equal("Item 3", listItems.ElementAt(2).Find(".list-item").TextContent);
    }

    [Fact]
    public void WaitForComponents_WaitsForSpecificCount()
    {
        var items = new List<string> { "Item 1", "Item 2", "Item 3", "Item 4", "Item 5" };
        var cut = Render<AsyncComponentLoader>(parameters => parameters
            .Add(p => p.Items, items));

        var listItems = cut.WaitForComponents<ListItem>(5);

        Assert.Equal(5, listItems.Count);
    }
}
