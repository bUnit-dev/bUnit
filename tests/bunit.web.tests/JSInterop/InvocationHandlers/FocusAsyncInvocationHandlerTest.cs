#if NET5_0
using System.Threading.Tasks;
using Bunit.TestAssets.SampleComponents;
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

		[Fact(DisplayName = "Can capture two FocusAsync calls")]
		public void Test002()
		{
			var cut = RenderComponent<Wrapper>(ps => ps
				.AddChildContent<FocusingComponent>()
				.AddChildContent<FocusingComponent>());

			var inputs = cut.FindAll("input");

			var invocations = JSInterop.VerifyFocusAsyncInvoke(calledTimes: 2);
			invocations[0].Arguments[0].ShouldBeElementReferenceTo(inputs[0]);
			invocations[1].Arguments[0].ShouldBeElementReferenceTo(inputs[1]);
		}

		private class FocusingComponent : ComponentBase
		{
			private ElementReference elmRef;
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
