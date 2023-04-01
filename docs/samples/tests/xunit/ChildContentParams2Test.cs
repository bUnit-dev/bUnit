using Xunit;
using Bunit;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Bunit.Rendering;

namespace Bunit.Docs.Samples;

public class ChildContentParams2Test : TestContext
{
    [Fact]
    public void Test()
    {
        var cut = RenderComponent<ChildContentParams>(parameters => parameters
            .AddChildContent<Counter>()
        );
    }
}