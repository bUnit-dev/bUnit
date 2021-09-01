#if NET6_0_OR_GREATER
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

namespace Bunit.JSInterop.InvocationHandlers
{
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
	}
}
#endif
