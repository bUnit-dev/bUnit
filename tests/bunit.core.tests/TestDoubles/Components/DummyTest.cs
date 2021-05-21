#if NET5_0_OR_GREATER
using AutoFixture.Xunit2;
using Bunit.TestAssets.SampleComponents;
using Shouldly;
using Xunit;

namespace Bunit.TestDoubles.Components
{
	public class DummyTest : TestContext
	{
		[Fact(DisplayName = "Dummy<TComponent> produces no markup")]
		public void Test001()
		{
			var cut = RenderComponent<Dummy<Simple1>>();
			cut.Markup.ShouldBeEmpty();
		}

		[Theory(DisplayName = "Dummy<TComponent> captures parameters passed to TComponent")]
		[AutoData]
		public void Test002(string header, string attrValue)
		{
			var cut = RenderComponent<Dummy<Simple1>>(
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
