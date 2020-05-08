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
			var sut = new TestRenderer(ServiceProvider, NullLoggerFactory.Instance);
			var handler = new MockRenderEventHandler(completeHandleTaskSynchronously: true);
			sut.AddRenderEventHandler(handler);

			// Act #1
			var cut = sut.RenderComponent<Simple1>(Array.Empty<ComponentParameter>());

			// Assert #1
			handler.ReceivedEvents.Count.ShouldBe(1);

			// Act #2
			await sut.InvokeAsync(() => cut.Component.SetParametersAsync(ParameterView.Empty));

			// Assert #2
			handler.ReceivedEvents.Count.ShouldBe(2);
		}

		[Fact(DisplayName = "Multiple handlers can be added to the Renderer")]
		public void Test002()
		{
			var sut = new TestRenderer(ServiceProvider, NullLoggerFactory.Instance);
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
			var sut = new TestRenderer(ServiceProvider, NullLoggerFactory.Instance);
			var handler1 = new MockRenderEventHandler(completeHandleTaskSynchronously: true);
			var handler2 = new MockRenderEventHandler(completeHandleTaskSynchronously: true);
			sut.AddRenderEventHandler(handler1);
			sut.AddRenderEventHandler(handler2);

			sut.RemoveRenderEventHandler(handler1);

			sut.RenderComponent<Simple1>(Array.Empty<ComponentParameter>());
			handler1.ReceivedEvents.ShouldBeEmpty();
			handler2.ReceivedEvents.Count.ShouldBe(1);
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
