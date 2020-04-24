using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Bunit.SampleComponents
{
	public class TwoComponentWrapper : ComponentBase
	{
		[Parameter]
		public RenderFragment? First { get; set; }

		[Parameter]
		public RenderFragment? Second { get; set; }

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, "div");
			builder.AddAttribute(1, "class", "first");
			builder.AddContent(2, First);
			builder.CloseElement();

			builder.OpenElement(10, "div");
			builder.AddAttribute(11, "class", "second");
			builder.AddContent(12, Second);
			builder.CloseElement();
		}
	}
}
