using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Bunit.RazorTesting;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Bunit.Rendering
{
	/// <summary>
	/// Represents a renderer specifically for rendering Razor-based test files (but not the actual tests inside).
	/// </summary>
	public class TestComponentRenderer : Renderer
	{
		private static readonly ServiceProvider ServiceProvider = new ServiceCollection().BuildServiceProvider();
		private static readonly Task CanceledRenderTask = Task.FromCanceled(new CancellationToken(canceled: true));
		private Exception? unhandledException;

		/// <inheritdoc/>
		public override Dispatcher Dispatcher { get; } = Dispatcher.CreateDefault();

		/// <summary>
		/// Initializes a new instance of the <see cref="TestComponentRenderer"/> class.
		/// </summary>
		public TestComponentRenderer() : this(ServiceProvider, NullLoggerFactory.Instance) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="TestComponentRenderer"/> class.
		/// </summary>
		public TestComponentRenderer(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
			: base(serviceProvider, loggerFactory) { }

		/// <summary>
		/// Renders an instance of the specified Razor-based test.
		/// </summary>
		/// <param name="componentType">Razor-based test to render.</param>
		/// <returns>A list of <see cref="FragmentBase"/> test definitions found in the test file.</returns>
		public IReadOnlyList<RazorTestBase> GetRazorTestsFromComponent(Type componentType)
		{
			var componentId = RenderComponent(componentType);
			return GetRazorTests<RazorTestBase>(componentId);
		}

		private int RenderComponent(Type componentType)
		{
			int componentId = -1;

			var renderTask = Dispatcher.InvokeAsync(() =>
			{
				var component = InstantiateComponent(componentType);
				componentId = AssignRootComponentId(component);
				return RenderRootComponentAsync(componentId);
			});

			if (!renderTask.IsCompleted)
			{
				renderTask.GetAwaiter().GetResult();
			}

			AssertNoUnhandledExceptions();

			return componentId;
		}

		private IReadOnlyList<TComponent> GetRazorTests<TComponent>(int fromComponentId)
		{
			var ownFrames = GetCurrentRenderTreeFrames(fromComponentId);

			if (ownFrames.Count == 0)
				return Array.Empty<TComponent>();

			var result = new List<TComponent>();

			for (var i = 0; i < ownFrames.Count; i++)
			{
				ref var frame = ref ownFrames.Array[i];
				if (frame.FrameType == RenderTreeFrameType.Component && frame.Component is TComponent component)
					result.Add(component);
			}

			return result;
		}

		/// <inheritdoc/>
		protected override Task UpdateDisplayAsync(in RenderBatch renderBatch) => CanceledRenderTask;

		/// <inheritdoc/>
		protected override void HandleException(Exception exception) => unhandledException = exception;

		private void AssertNoUnhandledExceptions()
		{
			if (unhandledException is Exception unhandled)
			{
				unhandledException = null;
				ExceptionDispatchInfo.Capture(unhandled).Throw();
			}
		}
	}
}
