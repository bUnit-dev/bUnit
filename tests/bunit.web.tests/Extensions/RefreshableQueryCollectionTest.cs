namespace Bunit;

public class RefreshableQueryCollectionTest : TestContext
{
	[Fact(DisplayName = "When the query returns no elements, the collection is empty")]
	public async Task Test001()
	{
		var cut = await RenderComponent<Simple1>();

		var sut = new RefreshableElementCollection(cut, ".foo");

		sut.ShouldBeEmpty();
	}

	[Fact(DisplayName = "When the query returns elements, the collection contains those elements")]
	public async Task Test002()
	{
		var cut = await RenderComponent<Simple1>();

		var sut = new RefreshableElementCollection(cut, "h1");

		sut.Count.ShouldBe(1);
		sut[0].TagName.ShouldBe("H1");
	}

	[Fact(DisplayName = "When Refresh is called, the query is run again and new elements are made available")]
	public async Task Test003()
	{
		var cut = await RenderComponent<ClickAddsLi>();
		var sut = new RefreshableElementCollection(cut, "li");
		sut.Count.ShouldBe(0);

		cut.Find("button").Click();

		sut.Refresh();
		sut.Count.ShouldBe(1);
	}

	[Fact(DisplayName = "Enabling auto refresh automatically refreshes query when the rendered fragment renders and has changes")]
	public async Task Test004()
	{
		var cut = await RenderComponent<ClickAddsLi>();
		var sut = new RefreshableElementCollection(cut, "li") { EnableAutoRefresh = true };
		sut.Count.ShouldBe(0);

		cut.Find("button").Click();

		sut.Count.ShouldBe(1);
	}

	[Fact(DisplayName = "Disabling auto refresh turns off automatic refreshing queries on when rendered fragment changes")]
	public async Task Test005()
	{
		var cut = await RenderComponent<ClickAddsLi>();
		var sut = new RefreshableElementCollection(cut, "li") { EnableAutoRefresh = true };

		sut.EnableAutoRefresh = false;

		cut.Find("button").Click();

		sut.Count.ShouldBe(0);
	}
}
