---
uid: render-modes
title: Support for render modes and renderer info    
---

# Support for render modes and the renderer info
Render modes in Blazor Web Apps determine the hosting model and interactivity of components. The render mode for example can be applied to a component using the `@rendermode` directive. The [`RendererInfo`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.rendererinfo?view=aspnetcore-9.0) allows the application to determine the interactivity and location of the component. For more details, check out the [Blazor render modes](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/render-modes?view=aspnetcore-9.0) documentation.

## Setting the render mode for a component under test
Setting the render mode can be done via the <xref:Bunit.TestContext.SetAssignedRenderMode(Microsoft.AspNetCore.Components.IComponentRenderMode)> method. The `SetAssignedRenderMode` method takes an [`IComponentRenderMode`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.icomponentrendermode?view=aspnetcore-9.0) object as a parameter. Normally this is one of the following types:
 * [`InteractiveAutoRenderMode`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.web.interactiveautorendermode?view=aspnetcore-9.0)
 * [`InteractiveServerRendeMode`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.web.interactiveserverrendermode?view=aspnetcore-9.0)
 * [`InteractiveWebAssemblyRenderMode`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.web.interactivewebassemblyrendermode?view=aspnetcore-9.0)

For ease of use the [`RenderMode`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.web.rendermode?view=aspnetcore-9.0) class defines all three of them.


> [!NOTE]
> Strictly speaking, this feature is available since `net8.0`. With `net9.0` Blazor exposes the information on `ComponentBase` via the `AssignedRenderMode` property.

This methods emulates the behavior of the `@rendermode` directive in a test environment. The following example shows how to set the render mode for a component under test:

```razor
@inherits TestContext

@code {
  [Fact]
  public void WhenRenderModeNonInteractive_DisableButton()
  {
    // Arrange
    var cut = RenderComponent<MyComponent>(ps => ps.SetAssignedRenderMode(RenderMode.InteractiveServer));
```

## Setting the `RendererInfo` for a component under test
To set the `RendererInfo` for a component under test, use the `SetRendererInfo` method on the `TestContext` class. The `SetRendererInfo` method takes an optional `RendererInfo` object as a parameter. A component might check if interactivity is given to enable a button:

```razor
<button @onclick="Send" disabled="@(!RendererInfo.IsInteractive)">
    Send
</button>
```

In the test, you can set the `RendererInfo` to enabl or disable the button:

```csharp
@inherits TestContext

@code {
  [Fact]
  public void WhenRenderModeNonInteractive_DisableButton()
  {
    // Arrange
    SetRendererInfo(new RendererInfo("Server", false));

    // Act
    var cut = RenderComponent<MyComponent>();

    // Assert
    cut.Find("button").Attributes["disabled"].ShouldBe("disabled");
  }
}
```