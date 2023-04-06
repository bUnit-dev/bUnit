using Xunit;
using Bunit;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Bunit.Rendering;

namespace Bunit.Docs.Samples;

public class EventCallbackParamsTest : TestContext
{
  [Fact]
  public void Test()
  {
    Action<MouseEventArgs> onClickHandler = _ => { };
    Action onSomethingHandler = () => { };

    var cut = RenderComponent<EventCallbackParams>(parameters => parameters
      .Add(p => p.OnClick, onClickHandler)
      .Add(p => p.OnSomething, onSomethingHandler)
    );
  }
}