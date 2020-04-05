using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Bunit.RazorTesting;
using Bunit.Rendering;
using Bunit.Rendering.RenderEvents;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Bunit
{
	/// <summary>
	/// Base test class/test runner, that runs Fixtures defined in razor files.
	/// </summary>
	public abstract class TestComponentBase
	{
		private static readonly ServiceProvider ServiceProvider = new ServiceCollection().BuildServiceProvider();
		private readonly TestComponentRenderer RazorRenderer = new TestComponentRenderer(ServiceProvider, NullLoggerFactory.Instance);

		private Fixture? _activeFixtureTest;

		public RazorTest? ActiveTest { get; private set; }

		public TestServiceProvider Services => ActiveTest?.Services ?? throw new InvalidOperationException("No active Razor test.");

		public TestRenderer Renderer => ActiveTest?.Renderer ?? throw new InvalidOperationException("No active Razor test.");

		public IObservable<RenderEvent> RenderEvents => ActiveTest?.RenderEvents ?? throw new InvalidOperationException("No active Razor test.");

		public IRenderedFragment GetComponentUnderTest() => _activeFixtureTest?.GetComponentUnderTest() ?? throw new InvalidOperationException("No active Razor test.");

		public IRenderedComponent<TComponent> GetComponentUnderTest<TComponent>() where TComponent : IComponent
			=> _activeFixtureTest?.GetComponentUnderTest<TComponent>() ?? throw new InvalidOperationException("No active Razor test.");

		public IRenderedFragment GetFragment(string? id = null) => _activeFixtureTest?.GetFragment(id) ?? throw new InvalidOperationException("No active Razor test.");

		public IRenderedComponent<TComponent> GetFragment<TComponent>(string? id = null) where TComponent : IComponent
			=> _activeFixtureTest?.GetFragment<TComponent>(id) ?? throw new InvalidOperationException("No active Razor test.");

		protected virtual void BuildRenderTree(RenderTreeBuilder builder) { }

		/// <summary>
		/// Called by the XUnit test runner. Finds all Fixture components
		/// in the file and runs their associated tests.
		/// </summary>
		[Fact(DisplayName = "Razor test runner")]
		public async Task RazorTest()
		{
			var tests = await RazorRenderer.RenderAndGetTestComponents<RazorTest>(BuildRenderTree).ConfigureAwait(false);

			foreach (var test in tests)
			{
				ActiveTest = test;
				if(test is Fixture fixture) _activeFixtureTest = fixture;

				await test.RunTest().ConfigureAwait(false);

				ActiveTest=null;
				_activeFixtureTest = null;
			}
		}
	}
}
