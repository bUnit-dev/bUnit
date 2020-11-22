#if NET5_0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit.TestAssets.SampleComponents;
using Xunit;

namespace Bunit
{
	public partial class MarkupMatchesAssertExtensionsTest : TestContext
	{
		[Fact(DisplayName = "MarkupMatches correctly ignores scoped CSS attributes")]
		public void Test_net5_001()
		{
			var cut = RenderComponent<ScopedCssElements>();

			cut.MarkupMatches("<h1>Hello Pink World!</h1>");
		}
	}
}

#endif
