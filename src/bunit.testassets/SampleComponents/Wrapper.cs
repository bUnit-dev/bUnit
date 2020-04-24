using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Bunit.SampleComponents
{
	public class Wrapper : ComponentBase
	{
		[Parameter] public RenderFragment? ChildContent { get; set; }

		protected override void BuildRenderTree(RenderTreeBuilder builder) => builder.AddContent(0, ChildContent);
	}
}
