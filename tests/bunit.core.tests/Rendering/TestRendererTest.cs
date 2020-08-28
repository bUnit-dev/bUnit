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

		[Fact(DisplayName = "Renderer notifies handlers of render events")]
		public async Task Test001()
		{
			// Arrange
			using var sut = new TestRenderer(ServiceProvider, NullLoggerFactory.Instance);
			var handler = new MockRenderEventHandler(completeHandleTaskSynchronously: true);
			sut.AddRenderEventHandler(handler);

			// Act #1
			var cut = sut.RenderComponent<Simple1>(Array.Empty<ComponentParameter>());

			// Assert #1
			handler.ReceivedEvents.Count.ShouldBe(1);

			// Act #2
			await sut.Dispatcher.InvokeAsync(() => cut.Component.SetParametersAsync(ParameterView.Empty));

			// Assert #2
			handler.ReceivedEvents.Count.ShouldBe(2);
		}

		[Fact(DisplayName = "Multiple handlers can be added to the Renderer")]
		public void Test002()
		{
			using var sut = new TestRenderer(ServiceProvider, NullLoggerFactory.Instance);
			var handler1 = new MockRenderEventHandler(completeHandleTaskSynchronously: true);
			var handler2 = new MockRenderEventHandler(completeHandleTaskSynchronously: true);

			sut.AddRenderEventHandler(handler1);
			sut.AddRenderEventHandler(handler2);

			sut.RenderComponent<Simple1>(Array.Empty<ComponentParameter>());
			handler1.ReceivedEvents.Count.ShouldBe(1);
			handler2.ReceivedEvents.Count.ShouldBe(1);
		}

		[Fact(DisplayName = "Handler is not invoked if removed from Renderer")]
		public void Test003()
		{
			using var sut = new TestRenderer(ServiceProvider, NullLoggerFactory.Instance);
			var handler1 = new MockRenderEventHandler(completeHandleTaskSynchronously: true);
			var handler2 = new MockRenderEventHandler(completeHandleTaskSynchronously: true);
			sut.AddRenderEventHandler(handler1);
			sut.AddRenderEventHandler(handler2);

			sut.RemoveRenderEventHandler(handler1);

			sut.RenderComponent<Simple1>(Array.Empty<ComponentParameter>());
			handler1.ReceivedEvents.ShouldBeEmpty();
			handler2.ReceivedEvents.Count.ShouldBe(1);
		}

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

		class MockRenderEventHandler : IRenderEventHandler
		{
			private TaskCompletionSource<object?> _handleTask = new TaskCompletionSource<object?>();
			private readonly bool _completeHandleTaskSynchronously;

			public List<RenderEvent> ReceivedEvents { get; set; } = new List<RenderEvent>();

			public MockRenderEventHandler(bool completeHandleTaskSynchronously)
			{
				if (completeHandleTaskSynchronously)
					SetCompleted();
				_completeHandleTaskSynchronously = completeHandleTaskSynchronously;
			}

			public Task Handle(RenderEvent renderEvent)
			{
				ReceivedEvents.Add(renderEvent);
				return _handleTask.Task;
			}

			public void SetCompleted()
			{
				if (_completeHandleTaskSynchronously)
					return;

				var existing = _handleTask;
				_handleTask = new TaskCompletionSource<object?>();
				existing.SetResult(null);
			}
		}
	}
}
