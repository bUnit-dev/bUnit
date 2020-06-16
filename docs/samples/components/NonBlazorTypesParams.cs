using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Bunit.Docs.Samples
{
  public class NonBlazorTypesParams : ComponentBase
  {
    [Parameter]
    public int Numbers { get; set; }

    [Parameter]
    public List<string> Lines { get; set; }
  }
}
