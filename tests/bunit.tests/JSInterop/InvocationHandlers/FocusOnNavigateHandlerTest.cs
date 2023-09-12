namespace Bunit.JSInterop.InvocationHandlers;

public class FocusOnNavigateHandlerTest : TestContext
{
	[Fact(DisplayName = "Can render components that calls FocusOnNavigate")]
	public void Test001()
	{
		// <FocusOnNavigate RouteData="@routeData" Selector="h1" />
		var cut = Render<App>(ps => ps
			.Add<FocusOnNavigate, RouteData>(p => p.FoundTemplate, routeData => cps => cps
				.Add(x => x.RouteData, routeData)
				.Add(x => x.Selector, "h1")));

		var focusOnNavigateComponent = cut.FindComponent<FocusOnNavigate>();

		var argument = JSInterop.VerifyFocusOnNavigateInvoke().Arguments[0];
		Assert.Equal("h1", argument);
		focusOnNavigateComponent.AccessInstance(c => c.Selector.ShouldBe(argument));
	}

	[Fact(DisplayName = "Will return completed task")]
	public void Test002()
	{
		var cut = Render<App>(ps => ps
			.Add<FocusOnNavigateInternal, RouteData>(p => p.FoundTemplate, routeData => cps => cps
				.Add(x => x.RouteData, routeData)
				.Add(x => x.Selector, "h1")));

		var focusOnNavigateComponent = cut.FindComponent<FocusOnNavigateInternal>();
		focusOnNavigateComponent.AccessInstance(c => c.AfterFirstRender.ShouldBeTrue());
	}

	private sealed class FocusOnNavigateInternal : FocusOnNavigate
	{
		internal bool AfterFirstRender { get; private set; }
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);
			if (firstRender)
			{
				AfterFirstRender = true;
			}
		}
	}
}
