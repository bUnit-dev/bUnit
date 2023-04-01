using Xunit;
using Bunit;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Bunit.Rendering;

namespace Bunit.Docs.Samples;

public class RenderFragmentParams3Test : TestContext
{
    [Fact]
    public void Test()
    {
        var cut = RenderComponent<RenderFragmentParams>(parameters => parameters
            .Add<Alert>(p => p.Content, alertParameters => alertParameters
                .Add(p => p.Heading, "Alert heading")
                .Add(p => p.Type, AlertType.Warning)
                .AddChildContent("<p>Hello World</p>")
            )
        );
    }
}