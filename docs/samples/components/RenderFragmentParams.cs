using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Bunit.Docs.Samples
{
  public class RenderFragmentParams : ComponentBase
  {
    [Parameter]
    public RenderFragment Content { get; set; }
  }
}
