using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Bunit.TestAssets.SampleComponents;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

using Shouldly;

using Xunit;

namespace Bunit.Rendering
{
	public class TestRendererTest
	{
		private static readonly ServiceProvider ServiceProvider = new ServiceCollection().BuildServiceProvider();

		[Fact(DisplayName = "Can render component that awaits uncompleted task in OnInitializedAsync")]
		public void Test100()
		{
			using var ctx = new TestContext();
			var tcs = new TaskCompletionSource<object>();

			var cut = ctx.RenderComponent<AsyncRenderOfSubComponentDuringInit>(parameters =>
				parameters.Add(p => p.EitherOr, tcs.Task)
			);

			cut.Find("h1").TextContent.ShouldBe("FIRST");
		}

		[Fact(DisplayName = "Can render component that awaits yielding task in OnInitializedAsync")]
		public void Test101()
		{
			using var ctx = new TestContext();

			var cut = ctx.RenderComponent<AsyncRenderOfSubComponentDuringInit>(parameters =>
				parameters.Add(p => p.EitherOr, Task.Delay(1))
			);

			cut.Find("h1").TextContent.ShouldBe("FIRST");
		}

		[Fact(DisplayName = "Can render component that awaits completed task in OnInitializedAsync")]
		public void Test102()
		{
			using var ctx = new TestContext();

			var cut = ctx.RenderComponent<AsyncRenderOfSubComponentDuringInit>(parameters =>
				parameters.Add(p => p.EitherOr, Task.CompletedTask)
			);

			cut.Find("h1").TextContent.ShouldBe("SECOND");
		}
	}
}
