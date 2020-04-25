using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
	public abstract class TestComponentBaseOld
	{
		private readonly TestComponentRenderer RazorRenderer = new TestComponentRenderer();

		/// <summary>
		/// Renders the component to the supplied <see cref="RenderTreeBuilder"/>.
		/// </summary>
		/// <param name="builder">The builder to use for rendering.</param>
		protected virtual void BuildRenderTree(RenderTreeBuilder builder) { }

		/// <summary>
		/// Called by the XUnit test runner. Finds all Fixture components
		/// in the file and runs their associated tests.
		/// </summary>
		[Fact(DisplayName = "Razor test runner")]
		public async Task RazorTest()
		{
			IReadOnlyList<RazorTest> tests = await RazorRenderer
				.RenderAndGetTestComponents<RazorTest>(BuildRenderTree)
				.ConfigureAwait(false);

			foreach (var test in tests.Where(x => x.Skip is null))
			{
				await test.RunTest().ConfigureAwait(false);
			}
		}
	}
}
