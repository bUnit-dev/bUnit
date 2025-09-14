---
uid: render-modes
title: Render modes and RendererInfo
---

# Support for render modes and `RendererInfo`
This article explains how to emulate different render modes and `RendererInfo` in bUnit tests.

Render modes in Blazor Web Apps determine the hosting model and interactivity of components. A render mode can be applied to a component using the `@rendermode` directive. The [`RendererInfo`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.rendererinfo?view=aspnetcore-9.0) allows the application to determine the interactivity and location of the component. For more details, see the [Blazor render modes](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/render-modes?view=aspnetcore-9.0) documentation.

## Setting the render mode for a component under test
Setting the render mode can be done via the <xref:Bunit.ComponentParameterCollectionBuilder`1.SetAssignedRenderMode(Microsoft.AspNetCore.Components.IComponentRenderMode)> method when writing in a C# file. In a razor file use the `@rendermode` directive. Both take an [`IComponentRenderMode`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.icomponentrendermode?view=aspnetcore-9.0) object as a parameter. Normally this is one of the following types:
 * [`InteractiveAutoRenderMode`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.web.interactiveautorendermode?view=aspnetcore-9.0)
 * [`InteractiveServerRendeMode`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.web.interactiveserverrendermode?view=aspnetcore-9.0)
 * [`InteractiveWebAssemblyRenderMode`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.web.interactivewebassemblyrendermode?view=aspnetcore-9.0)

For ease of use the [`RenderMode`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.web.rendermode?view=aspnetcore-9.0) class defines all three of them.

For example `MovieComponent.razor`:
```razor
@if (AssignedRenderMode is null)
{
    // The render mode is Static Server
    <form action="/movies">
        <input type="text" name="titleFilter" />
        <input type="submit" value="Search" />
    </form>
}
else
{
    // The render mode is Interactive Server, WebAssembly, or Auto
    <input @bind="titleFilter" />
    <button @onclick="FilterMovies">Search</button>
}
```

The following example shows how to test the above component to check both render modes:

# [C# test code](#tab/csharp)

```csharp
[Fact]
public void InteractiveServer()
{
  // Act
  var cut = RenderComponent<MovieComponent>(ps => ps
    .SetAssignedRenderMode(RenderMode.InteractiveServer));
  
  // Assert
  cut.MarkupMatches("""
                    <input diff:ignoreAttributes />
                    <button>Search</button>
                    """);
}

[Fact]
public void StaticRendering()
{
  // Act
  var cut = RenderComponent<MovieComponent>();
  // This is the same behavior as:
  // var cut = RenderComponent<MovieComponent>(ps => ps
  //  .SetAssignedRenderMode(null));
  
  // Assert
  cut.MarkupMatches("""
                    <form action="/movies">
                    <input type="text" name="titleFilter" />
                    <input type="submit" value="Search" />
                    </form>
                    """);
}
```

# [Razor test code](#tab/razor)

```razor
@inherits TestContext
@code {
  [Fact]
  public void InteractiveServer()
  {
    // Act
    var cut = Render(@<MovieComponent @rendermode="RenderMode.InteractiveServer" />);

    // Assert
    cut.MarkupMatches(@<text>
                        <input diff:ignoreAttributes />
                        <button>Search</button>
                       </text>);
  }

  [Fact]
  public void StaticRendering()
  {
    // Act
    var cut = Render(@<MovieComponent />);

    // Assert
    cut.MarkupMatches(@<form action="/movies">
                          <input type="text" name="titleFilter" />
                          <input type="submit" value="Search" />
                        </form>);
  }
}
```

***

## Setting the `RendererInfo` during testing
To control the `ComponentBase.RendererInfo` property during testing, use the <xref:Bunit.BunitContext.SetRendererInfo(Microsoft.AspNetCore.Components.RendererInfo)> method on the `TestContext` class. The `SetRendererInfo` method takes an nullable `RendererInfo` object as a parameter. Passing `null` will set the `ComponentBase.RendererInfo` to `null`. 

A component (`AssistentComponent.razor`) might check if interactivity is given to enable a button:

```razor
@if (RendererInfo.IsInteractive)
{
    <p>Hey I am your assistant</p>
}
else
{
    <p>Loading...</p>
}
```

In the test, you can set the `RendererInfo` to enable or disable the button:

```csharp
[Fact]
public void SimulatingPreRenderingOnBlazorServer()
{
  // Arrange
  SetRendererInfo(new RendererInfo(rendererName: "Static", isInteractive: false));

  // Act
  var cut = RenderComponent<AssistentComponent>();

  // Assert
  cut.MarkupMatches("<p>Loading...</p>");
}

[Fact]
public void SimulatingInteractiveServerRendering()
{
  // Arrange
  SetRendererInfo(new RendererInfo(rendererMode: "Server", isInteractive: true));

  // Act
  var cut = RenderComponent<AssistentComponent>();

  // Assert
  cut.MarkupMatches("<p>Hey I am your assistant</p>");
}
```

> [!NOTE]
> If a component under test uses the `ComponentBase.RendererInfo` property and the `SetRendererInfo` on `TestContext` hasn't been passed in a `RendererInfo` object, the renderer will throw an exception.