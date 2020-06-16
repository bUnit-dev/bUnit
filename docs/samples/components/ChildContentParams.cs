using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Bunit.Docs.Samples
{
  public class ChildContentParams : ComponentBase
  {
    [Parameter]
    public RenderFragment ChildContent { get; set; }
  }
}
