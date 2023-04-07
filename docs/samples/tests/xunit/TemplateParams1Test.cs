using Xunit;
using Bunit;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Bunit.Rendering;

namespace Bunit.Docs.Samples;

public class TemplateParams1Test : TestContext
{
  [Fact]
  public void Test()
  {
    var cut = RenderComponent<TemplateParams<string>>(parameters => parameters
      .Add(p => p.Items, new[] { "Foo", "Bar", "Baz" })
      .Add(p => p.Template, item => $"<span>{item}</span>")
    );
  }
}