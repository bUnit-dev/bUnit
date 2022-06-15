using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit.TestAssets.SampleComponents;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Shouldly;
using Xunit;

namespace Bunit.JSInterop.InvocationHandlers;

public class FocusOnNavigateHandlerTest : TestContext
{
	[Fact(DisplayName = "Can render components that calls FocusOnNavigate")]
	public void Test001()
	{
		// <FocusOnNavigate RouteData="@routeData" Selector="h1" />
		var cut = RenderComponent<App>(ps => ps
			.Add<FocusOnNavigate, RouteData>(p => p.FoundTemplate, routeData => cps => cps
				.Add(x => x.RouteData, routeData)
				.Add(x => x.Selector, "h1")));

		var focusOnNavigateComponent = cut.FindComponent<FocusOnNavigate>();

		Assert.Equal("h1", JSInterop.VerifyFocusOnNavigateInvoke().Arguments[0]);
		JSInterop.VerifyFocusOnNavigateInvoke()
			.Arguments[0]
			.ShouldBe(focusOnNavigateComponent.Instance.Selector);
	}

	[Fact(DisplayName = "Will return completed task")]
	public void Test002()
	{
		var cut = RenderComponent<App>(ps => ps
			.Add<FocusOnNavigateInternal, RouteData>(p => p.FoundTemplate, routeData => cps => cps
				.Add(x => x.RouteData, routeData)
				.Add(x => x.Selector, "h1")));

		var focusOnNavigateComponent = cut.FindComponent<FocusOnNavigateInternal>();
		Assert.True(focusOnNavigateComponent.Instance.AfterFirstRender);
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
