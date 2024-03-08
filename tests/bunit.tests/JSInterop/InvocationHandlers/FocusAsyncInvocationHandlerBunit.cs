namespace Bunit.JSInterop.InvocationHandlers;

public class FocusAsyncInvocationHandlerBunit : BunitContext
{
	[Fact(DisplayName = "Can render components that calls FocusAsync")]
	public void Test001()
	{
		var cut = Render<FocusingComponent>();
		var input = cut.Find("input");
		JSInterop.VerifyFocusAsyncInvoke().Arguments[0].ShouldBeElementReferenceTo(input);
	}

	[Fact(DisplayName = "Can capture two FocusAsync calls")]
	public void Test002()
	{
		var cut = Render<Wrapper>(ps => ps
			.AddChildContent<FocusingComponent>()
			.AddChildContent<FocusingComponent>());

		var inputs = cut.FindAll("input");

		var invocations = JSInterop.VerifyFocusAsyncInvoke(calledTimes: 2);
		invocations[0].Arguments[0].ShouldBeElementReferenceTo(inputs[0]);
		invocations[1].Arguments[0].ShouldBeElementReferenceTo(inputs[1]);
	}

	[Fact(DisplayName = "Will return completed task")]
	public void Test003()
	{
		var cut = Render<FocusingComponent>();
		Assert.True(cut.Instance.AfterFirstRender);
	}

	private sealed class FocusingComponent : ComponentBase
	{
		private ElementReference elmRef;
		internal bool AfterFirstRender { get; private set; }
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await elmRef.FocusAsync();
				AfterFirstRender = true;
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
