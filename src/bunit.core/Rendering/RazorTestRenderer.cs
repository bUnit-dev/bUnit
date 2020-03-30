using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using Bunit.RazorTesting;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.Logging;

namespace Bunit.Rendering
{
	/// <summary>
	/// Represents a renderer specifically for rendering Razor-based test files (but not the actual tests inside).
	/// </summary>
	public class RazorTestRenderer : Renderer
	{
		private Exception? _unhandledException;

		/// <inheritdoc/>
		public override Dispatcher Dispatcher { get; } = Dispatcher.CreateDefault();

		/// <summary>
		/// Creates an instance of the <see cref="RazorTestRenderer"/>.
		/// </summary>
		public RazorTestRenderer(IServiceProvider serviceProvider, ILoggerFactory loggerFactory) : base(serviceProvider, loggerFactory) { }

		/// <summary>
		/// Renders an instance of the specified Razor-based test.
		/// </summary>
		/// <param name="componentType">Razor-based test to render.</param>
		/// <returns>A list of <see cref="FragmentBase"/> test definitions found in the test file.</returns>
		public async Task<IReadOnlyList<RazorTest>> GetRazorTestsFromComponent(Type componentType)
		{
			var componentId = await Dispatcher.InvokeAsync(() => RenderComponent(componentType)).ConfigureAwait(false);
			AssertNoUnhandledExceptions();
			return GetRazorTests(componentId);
		}

		private async Task<int> RenderComponent(Type componentType)
		{
			var component = InstantiateComponent(componentType);
			var componentId = AssignRootComponentId(component);
			await RenderRootComponentAsync(componentId).ConfigureAwait(false);
			return componentId;
		}

		private IReadOnlyList<RazorTest> GetRazorTests(int fromComponentId)
		{
			var result = new List<RazorTest>();
			var ownFrames = GetCurrentRenderTreeFrames(fromComponentId);
			for (var i = 0; i < ownFrames.Count; i++)
			{
				ref var frame = ref ownFrames.Array[i];
				if (frame.FrameType == RenderTreeFrameType.Component)
					if (frame.Component is RazorTest component)
						result.Add(component);
			}
			return result;
		}

		/// <inheritdoc/>
		protected override Task UpdateDisplayAsync(in RenderBatch renderBatch) => Task.CompletedTask;

		/// <inheritdoc/>
		protected override void HandleException(Exception exception) => _unhandledException = exception;

		private void AssertNoUnhandledExceptions()
		{
			if (_unhandledException is { } unhandled)
			{
				_unhandledException = null;
				ExceptionDispatchInfo.Capture(unhandled).Throw();
			}
		}
	}
}
