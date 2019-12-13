using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace Egil.RazorComponents.Testing
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
        public IRenderedFragment RenderTestInput()
        {
            var fragment = _testData.OfType<TestInput>().Single();
            return new RenderedFragment(this, fragment.ChildContent);
        }

        /// <summary>
        /// Renders the expected output.
        /// </summary>       
        public IRenderedFragment RenderExpectedOutput()
        {
            var fragment = _testData.OfType<ExpectedOutput>().Single();
            return new RenderedFragment(this, fragment.ChildContent);
        }
    }
}
