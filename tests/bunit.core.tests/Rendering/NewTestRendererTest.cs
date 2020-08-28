using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bunit.Rendering;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using Shouldly;

using Xunit;

namespace Bunit.Rendering
{
	public class NoChildNoParams : ComponentBase
	{
		public const string MARKUP = "<h1>hello world</h1>";
		protected override void BuildRenderTree(RenderTreeBuilder builder)
			=> builder.AddMarkupContent(0, MARKUP);
	}

	internal class TestRenderedFragment : IRenderedComponent
	{
		private readonly object _renderLock = new object();

		protected int ComponentId { get; set; }

		public string Markup { get; private set; } = string.Empty;

		public int RenderCount { get; private set; }

		public TestRenderedFragment(int componentId)
		{
			ComponentId = componentId;
		}

		void IRenderedComponent.OnRender(IReadOnlyDictionary<int, ArrayRange<RenderTreeFrame>> currentRenderTree)
		{
			lock (_renderLock)
			{
				OnRender(currentRenderTree);
				RenderCount++;
				Markup = HewHtmlizer.GetHtml(ComponentId, currentRenderTree);
			}
		}

		protected virtual void OnRender(IReadOnlyDictionary<int, ArrayRange<RenderTreeFrame>> currentRenderTree) { }
	}

	internal class TestRenderedComponent<TComponent> : TestRenderedFragment where TComponent : IComponent
	{
		private TComponent _instance = default!;

		public TComponent Instance => _instance ?? throw new InvalidOperationException("Component has not rendered yet...");

		public TestRenderedComponent(int componentId) : base(componentId)
		{
		}

		protected override void OnRender(IReadOnlyDictionary<int, ArrayRange<RenderTreeFrame>> currentRenderTree)
		{
			if (_instance is null)
			{
				var frames = currentRenderTree[ComponentId];
				if (frames.Array[0].Component is TComponent wrapperComponent)
				{
					_instance = wrapperComponent;
					ComponentId = frames.Array[0].ComponentId;
				}
				else
				{
					throw new InvalidOperationException("Component instance not found at expected position in render tree.");
				}
			}
		}
	}

	internal sealed class TestRenderedComponentActivator : IRenderedComponentActivator
	{
		public IRenderedComponent CreateRenderedComponent<T>(int componentId) where T : IComponent
			=> new TestRenderedComponent<T>(componentId);
		public IRenderedComponent CreateRenderedComponent(int componentId)
			=> new TestRenderedFragment(componentId);
	}

	public class NewTestRendererTest
	{
		private ILoggerFactory LoggerFactory { get; } = NullLoggerFactory.Instance;
		private TestServiceProvider Services { get; } = new TestServiceProvider();
		private IRenderedComponentActivator Activator { get; } = new TestRenderedComponentActivator();

		[Fact(DisplayName = "Can render fragment without children and no parameters")]
		public void Test001()
		{
			const string MARKUP = "<h1>hello world</h1>";
			using var sut = new NewTestRenderer(Activator, Services, LoggerFactory);

			var cut = sut.RenderFragment<TestRenderedFragment>(builder => builder.AddMarkupContent(0, MARKUP));

			cut.RenderCount.ShouldBe(1);
			cut.Markup.ShouldBe(MARKUP);
		}

		[Fact(DisplayName = "Can render component without children and no parameters")]
		public void Test002()
		{
			const string MARKUP = "<h1>hello world</h1>";
			using var sut = new NewTestRenderer(Activator, Services, LoggerFactory);

			var cut = sut.RenderComponent<TestRenderedComponent<NoChildNoParams>, NoChildNoParams>(Array.Empty<ComponentParameter>());

			cut.RenderCount.ShouldBe(1);
			cut.Markup.ShouldBe(MARKUP);
			cut.Instance.ShouldBeOfType<NoChildNoParams>();
		}

		// renderer rethrows unhandled exceptions

		// rendered fragment updates RenderCount on rerenders
		// rendered component updates RenderCount on rerenders

		// rendered fragment updates RenderCount, markup on rerenders with changes in render tree
		// rendered component updates RenderCount, markup on rerenders with changes in render tree

		// rendered fragment updates RenderCount, markup on rerenders from child components with changes in render tree
		// rendered component updates RenderCount, markup on rerenders from child components with changes in render tree

		// rendered fragment updates RenderCount, markup on rerenders when child components are disposed
		// rendered component updates RenderCount, markup on rerenders when child components are disposed

		// rendered fragment marked as disposed when it is removed from render tree
		// rendered component marked as disposed when it is removed from render tree

		// GetComponent (FindCompnent) and GetComponents (FindComponents) tests....
	}
}
