using System.Collections.Generic;
using System.Linq;
using Bunit.RazorTesting;
using Microsoft.AspNetCore.Components;

namespace Bunit
{
	/// <summary>
	/// Represents a test context used in snapshot testing.
	/// </summary>
	public class SnapshotTestContext : TestContext
	{
		private readonly IReadOnlyList<FragmentBase> _testData;

		/// <inheritdoc/>
		public SnapshotTestContext(IReadOnlyList<FragmentBase> testData)
		{
			_testData = testData;
		}

		/// <summary>
		/// Renders the test input.
		/// </summary>       
		public IWebRenderedFragment RenderTestInput()
		{
			var fragment = _testData.OfType<TestInput>().Single();
			var renderId = Renderer.RenderFragment(fragment.ChildContent).GetAwaiter().GetResult();
			return new RenderedFragment(Services, renderId);
		}

		/// <summary>
		/// Renders the expected output.
		/// </summary>       
		public IWebRenderedFragment RenderExpectedOutput()
		{
			var fragment = _testData.OfType<ExpectedOutput>().Single();
			var renderId = Renderer.RenderFragment(fragment.ChildContent).GetAwaiter().GetResult();
			return new RenderedFragment(Services, renderId);
		}
	}
}
