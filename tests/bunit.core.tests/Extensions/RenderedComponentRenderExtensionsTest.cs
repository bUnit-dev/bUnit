using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit.TestAssets.SampleComponents;
using Shouldly;
using Xunit;

namespace Bunit
{
    public class RenderedComponentRenderExtensionsTest : TestContext
    {
		[Fact(DisplayName = "SetParametersAndRender rethrows exceptions from SetParameterAsync")]
		public void Test001()
		{
			var cut = RenderComponent<ThrowsOnParameterSet>();

			var exception = Should.Throw<InvalidOperationException>(
				() => cut.SetParametersAndRender(ps => ps.Add(qt => qt.Value, "something")));
			exception.InnerException.ShouldBeOfType<ArgumentOutOfRangeException>();
		}
	}
}
