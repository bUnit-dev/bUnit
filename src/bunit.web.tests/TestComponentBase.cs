using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Bunit.RazorTesting;
using Bunit.Rendering;
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
	public abstract class TestComponentBase : ComponentTestFixture, IRazorTestContext
	{
		private static readonly ServiceProvider ServiceProvider = new ServiceCollection().BuildServiceProvider();
		private readonly RazorTestRenderer RazorRenderer = new RazorTestRenderer(ServiceProvider, NullLoggerFactory.Instance);
		private readonly TestContextAdapter _testContextAdapter = new TestContextAdapter();
		private bool _isDisposed = false;

		/// <inheritdoc/>
		public override TestServiceProvider Services
			=> _testContextAdapter.HasActiveContext ? _testContextAdapter.Services : base.Services;

		/// <inheritdoc/>
		public IWebRenderedFragment GetComponentUnderTest()
			=> _testContextAdapter.GetComponentUnderTest();

		/// <inheritdoc/>
		public IRenderedComponent<TComponent> GetComponentUnderTest<TComponent>() where TComponent : IComponent
			=> _testContextAdapter.GetComponentUnderTest<TComponent>();

		/// <inheritdoc/>
		public IWebRenderedFragment GetFragment(string? id = null)
			=> _testContextAdapter.GetFragment(id);

		/// <inheritdoc/>
		public IRenderedComponent<TComponent> GetFragment<TComponent>(string? id = null) where TComponent : IComponent
			=> _testContextAdapter.GetFragment<TComponent>(id);

		/// <inheritdoc/>
		public override IRenderedComponent<TComponent> RenderComponent<TComponent>(params ComponentParameter[] parameters)
			=> _testContextAdapter.HasActiveContext
				? _testContextAdapter.RenderComponent<TComponent>(parameters)
				: base.RenderComponent<TComponent>(parameters);

		/// <summary>
		/// Called by the XUnit test runner. Finds all Fixture components
		/// in the file and runs their associated tests.
		/// </summary>
		[Fact(DisplayName = "Razor test runner")]
		public async Task RazorTest()
		{
			var tests = await RazorRenderer.RenderAndGetTestComponents<RazorTest>(BuildRenderTree);

			await ExecuteFixtureTests(tests.OfType<Fixture>()).ConfigureAwait(false);
			await ExecuteSnapshotTests(tests.OfType<SnapshotTest>()).ConfigureAwait(false);
		}

		private async Task ExecuteFixtureTests(IEnumerable<Fixture> fixtures)
		{
			foreach (var fixture in fixtures)
			{
				var testData = await RazorRenderer.RenderAndGetTestComponents<FragmentBase>(fixture.ChildContent);

				_testContextAdapter.ActivateRazorTestContext(testData);

				InvokeFixtureAction(fixture, fixture.Setup);
				await InvokeFixtureAction(fixture, fixture.SetupAsync).ConfigureAwait(false);
				InvokeFixtureAction(fixture, fixture.Test);
				await InvokeFixtureAction(fixture, fixture.TestAsync).ConfigureAwait(false);

				foreach (var test in fixture.Tests)
				{
					InvokeFixtureAction(fixture, test);
				}

				foreach (var test in fixture.TestsAsync)
				{
					await InvokeFixtureAction(fixture, test).ConfigureAwait(false);
				}

				_testContextAdapter.DisposeActiveTestContext();
			}
		}

		private static void InvokeFixtureAction(Fixture fixture, Action action)
		{
			try
			{
				action();
			}
			catch (Exception ex)
			{
				throw new FixtureFailedException(fixture.Description ?? $"{action.Method.Name} failed:", ex);
			}
		}

		private static async Task InvokeFixtureAction(Fixture fixture, Func<Task> action)
		{
			try
			{
				await action().ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				throw new FixtureFailedException(fixture.Description ?? $"{action.Method.Name} failed:", ex);
			}
		}

		private async Task ExecuteSnapshotTests(IEnumerable<SnapshotTest> snapshotTests)
		{
			foreach (var snapshot in snapshotTests)
			{
				var testData = await RazorRenderer.RenderAndGetTestComponents<FragmentBase>(snapshot.ChildContent);

				var context = _testContextAdapter.ActivateSnapshotTestContext(testData);
				snapshot.Setup();
				await snapshot.SetupAsync().ConfigureAwait(false);
				var actual = context.RenderTestInput();
				var expected = context.RenderExpectedOutput();
				actual.MarkupMatches(expected, snapshot.Description);
				_testContextAdapter.DisposeActiveTestContext();
			}
		}

		/// <inheritdoc/>
		protected virtual void BuildRenderTree(RenderTreeBuilder builder) { }

		/// <inheritdoc/>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (_isDisposed)
				return;

			if (disposing)
			{
				_testContextAdapter.Dispose();
			}
			_isDisposed = true;
		}
	}
}
