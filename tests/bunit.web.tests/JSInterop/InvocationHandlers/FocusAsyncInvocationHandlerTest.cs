#if NET5_0
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Xunit;

namespace Bunit.JSInterop.InvocationHandlers
{
	public class FocusAsyncInvocationHandlerTest : TestContext
	{
		[Fact(DisplayName = "Can render components that calls FocusAsync")]
		public void Test001()
		{
			var cut = RenderComponent<FocusingComponent>();
			var input = cut.Find("input");
			JSInterop.VerifyFocusAsyncInvoke().Arguments[0].ShouldBeElementReferenceTo(input);
		}

		class FocusingComponent : ComponentBase
		{
			ElementReference elmRef;
			protected override async Task OnAfterRenderAsync(bool firstRender)
			{
				if (firstRender)
				{
					await elmRef.FocusAsync();
				}
			}

			protected override void BuildRenderTree(RenderTreeBuilder builder)
			{
				builder.OpenElement(1, "input");
				builder.AddElementReferenceCapture(2, x => elmRef = x);
				builder.CloseElement();
			}
		}
	}
}
#endif
