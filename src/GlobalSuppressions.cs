using System.Diagnostics.CodeAnalysis;
[assembly: SuppressMessage("Reliability", "BL0006:The types in 'Microsoft.AspNetCore.Components.RenderTree' are not recommended for use outside of the Blazor framework. These type definitions will change in future releases.",
    Justification = "I will take the chance",
    Scope = "namespaceanddescendants",
    Target = "Egil.RazorComponents.Testing")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", 
    "CA1303:Do not pass literals as localized parameters", 
    Justification = "<Pending>",
    Scope = "namespaceanddescendants",
    Target = "Egil.RazorComponents.Testing")]

[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>", Scope = "member", Target = "~M:Egil.RazorComponents.Testing.Expect.BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>", Scope = "member", Target = "~M:Egil.RazorComponents.Testing.Given.BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder)")]