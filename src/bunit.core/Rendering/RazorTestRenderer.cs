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
			return GetRazorTests<RazorTest>(componentId);
		}

		/// <summary>
		/// Renders the provided <paramref name="renderFragment"/>.
		/// </summary>
		/// <typeparam name="TComponent">The type of components to find in the render tree after renderinger.</typeparam>
		/// <param name="renderFragment">The <see cref="RenderFragment"/> to render.</param>
		/// <returns>A list of <typeparamref name="TComponent"/> found in the <paramref name="renderFragment"/>'s render tree.</returns>
		public async Task<IReadOnlyList<TComponent>> RenderAndGetTestComponents<TComponent>(RenderFragment renderFragment)
			where TComponent : FragmentBase
		{
			var componentId = await RenderFragmentInsideWrapper(renderFragment);
			return GetRazorTests<TComponent>(componentId);
		}

		private async Task<int> RenderComponent(Type componentType)
		{
			var component = InstantiateComponent(componentType);
			var componentId = AssignRootComponentId(component);
			await RenderRootComponentAsync(componentId).ConfigureAwait(false);
			return componentId;
		}

		private async Task<int> RenderFragmentInsideWrapper(RenderFragment renderFragment)
		{
			var wrapper = new WrapperComponent();
			var wrapperId = AssignRootComponentId(wrapper);

			await Dispatcher.InvokeAsync(() => wrapper.Render(renderFragment)).ConfigureAwait(false);

			AssertNoUnhandledExceptions();

			return wrapperId;
		}

		private IReadOnlyList<TComponent> GetRazorTests<TComponent>(int fromComponentId)
			where TComponent : FragmentBase
		{
			var result = new List<TComponent>();
			var ownFrames = GetCurrentRenderTreeFrames(fromComponentId);
			for (var i = 0; i < ownFrames.Count; i++)
			{
				ref var frame = ref ownFrames.Array[i];
				if (frame.FrameType == RenderTreeFrameType.Component)
					if (frame.Component is TComponent component)
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

		private class WrapperComponent : IComponent
		{
			private RenderHandle _renderHandle;

			public void Attach(RenderHandle renderHandle) => _renderHandle = renderHandle;

			public Task SetParametersAsync(ParameterView parameters) => throw new InvalidOperationException($"WrapperComponent shouldn't receive any parameters");

			public void Render(RenderFragment renderFragment)
			{
				_renderHandle.Render(renderFragment);
			}
		}
	}
}
