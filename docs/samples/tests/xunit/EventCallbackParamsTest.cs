using Xunit;
using Bunit;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Bunit.Rendering;

namespace Bunit.Docs.Samples
{
  public class EventCallbackParamsTest
  {
    [Fact]
    public void Test()
    {
      using var ctx = new TestContext();

      Action<MouseEventArgs> onClickHandler = _ => { };
      Action onSomethingHandler = () => { };

      var cut = ctx.RenderComponent<EventCallbackParams>(parameters => parameters
        .Add(p => p.OnClick, onClickHandler)
        .Add(p => p.OnSomething, onSomethingHandler)
      );
    }
  }
}