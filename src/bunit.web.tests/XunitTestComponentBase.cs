using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Bunit.RazorTesting;
using Bunit.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Bunit
{
	public abstract class XunitTestComponentBase<TTestComponent> : IComponent, IRazorTestContext
	{
		private static readonly ServiceProvider ServiceProvider = new ServiceCollection().BuildServiceProvider();
		private static readonly RazorTestRenderer RazorRenderer = new RazorTestRenderer(ServiceProvider, NullLoggerFactory.Instance);

		public TestServiceProvider Services => throw new NotImplementedException();

		public IObservable<RenderEvent> RenderEvents => throw new NotImplementedException();

		public static IEnumerable<RazorTest[]> RazorTestsSource()
		{
			var type = typeof(TTestComponent);
			var tests = RazorRenderer.GetRazorTestsFromComponent(type).GetAwaiter().GetResult();

			foreach (var test in tests)
			{
				yield return new[] { test };
			}
		}

		[SkippableTheory(DisplayName = "")]
		[MemberData(nameof(RazorTestsSource))]
		public void RazorTests(RazorTest Test)
		{
			Skip.IfNot(Test.Skip is null, Test.Skip);
			Assert.NotNull(Test);
		}

		void IComponent.Attach(RenderHandle renderHandle)
		{
			RazorRenderer.Dispatcher.InvokeAsync(() => renderHandle.Render(BuildRenderTree)).Wait();
		}

		Task IComponent.SetParametersAsync(ParameterView parameters) => Task.CompletedTask;
		protected virtual void BuildRenderTree(RenderTreeBuilder builder) { }

		public IRenderedFragment GetComponentUnderTest() => throw new NotImplementedException();
		public IRenderedComponent<TComponent> GetComponentUnderTest<TComponent>() where TComponent : IComponent => throw new NotImplementedException();
		public IRenderedFragment GetFragment(string? id = null) => throw new NotImplementedException();
		public IRenderedComponent<TComponent> GetFragment<TComponent>(string? id = null) where TComponent :  IComponent => throw new NotImplementedException();
		public INodeList CreateNodes(string markup) => throw new NotImplementedException();
		public IRenderedComponent<TComponent> RenderComponent<TComponent>(params ComponentParameter[] parameters) where TComponent : IComponent
			=> throw new NotImplementedException();
		public void Dispose() { } // => throw new NotImplementedException();
	}
}
