#if NET5_0_OR_GREATER
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bunit.TestAssets.SampleComponents;
using Shouldly;
using Xunit;

namespace Bunit.TestDoubles.Components
{
	public class StubTest : TestContext
	{
		[Fact(DisplayName = "Stub<TComponent> renders element with name of TComponent")]
		public void Test001()
		{
			var cut = RenderComponent<Stub<Simple1>>();

			cut.MarkupMatches("<Simple1></Simple1>");
		}

		[Theory(DisplayName = "Stub<TComponent> captures parameters passed to TComponent")]
		[AutoData]
		public void Test002(string header, string attrValue)
		{
			var cut = RenderComponent<Stub<Simple1>>(
				(nameof(Simple1.Header), header),
				(nameof(Simple1.AttrValue), attrValue));

			cut.Instance.Parameters
				.ShouldSatisfyAllConditions(
					ps => ps.ShouldContain(x => x.Key == nameof(Simple1.Header) && header.Equals(x.Value)),
					ps => ps.ShouldContain(x => x.Key == nameof(Simple1.AttrValue) && attrValue.Equals(x.Value)),
					ps => ps.Count.ShouldBe(2));
		}
	}
}
#endif
