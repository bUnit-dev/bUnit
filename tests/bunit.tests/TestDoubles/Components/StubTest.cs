namespace Bunit.TestDoubles.Components;

public class StubTest : TestContext
{
	[Fact(DisplayName = "Stub<TComponent> renders nothing without a replacement template")]
	public void Test001()
	{
		var cut = Render<Stub<Simple1>>();

		cut.Nodes.Length.ShouldBe(0);
	}

	[Theory(DisplayName = "Stub<TComponent> captures parameters passed to TComponent")]
	[AutoData]
	public void Test002(string header, string attrValue)
	{
		var cut = Render<Stub<Simple1>>(ps => ps
			.AddUnmatched(nameof(Simple1.Header), header)
			.AddUnmatched(nameof(Simple1.AttrValue), attrValue));

		cut.AccessInstance(c => c
			.Parameters
			.ShouldSatisfyAllConditions(
				ps => ps.ShouldContain(x => x.Key == nameof(Simple1.Header) && header.Equals(x.Value)),
				ps => ps.ShouldContain(x => x.Key == nameof(Simple1.AttrValue) && attrValue.Equals(x.Value)),
				ps => ps.Count.ShouldBe(2)));
	}
}
