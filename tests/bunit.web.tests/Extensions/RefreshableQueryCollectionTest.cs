using Bunit.TestAssets.SampleComponents;
using Shouldly;
using Xunit;

namespace Bunit
{
	public class RefreshableQueryCollectionTest : TestContext
	{
		[Fact(DisplayName = "When the query returns no elements, the collection is empty")]
		public void Test001()
		{
			var cut = RenderComponent<Simple1>();

			var sut = new RefreshableElementCollection(cut, ".foo");

			sut.ShouldBeEmpty();
		}

		[Fact(DisplayName = "When the query returns elements, the collection contains those elements")]
		public void Test002()
		{
			var cut = RenderComponent<Simple1>();

			var sut = new RefreshableElementCollection(cut, "h1");

			sut.Count.ShouldBe(1);
			sut[0].TagName.ShouldBe("H1");
		}

		[Fact(DisplayName = "When Refresh is called, the query is run again and new elements are made available")]
		public void Test003()
		{
			var cut = RenderComponent<ClickAddsLi>();
			var sut = new RefreshableElementCollection(cut, "li");
			sut.Count.ShouldBe(0);

			cut.Find("button").Click();

			sut.Refresh();
			sut.Count.ShouldBe(1);
		}

		[Fact(DisplayName = "Enabling auto refresh automatically refreshes query when the rendered fragment renders and has changes")]
		public void Test004()
		{
			var cut = RenderComponent<ClickAddsLi>();
			var sut = new RefreshableElementCollection(cut, "li") { EnableAutoRefresh = true };
			sut.Count.ShouldBe(0);

			cut.Find("button").Click();

			sut.Count.ShouldBe(1);
		}

		[Fact(DisplayName = "Disabling auto refresh turns off automatic refreshing queries on when rendered fragment changes")]
		public void Test005()
		{
			var cut = RenderComponent<ClickAddsLi>();
			var sut = new RefreshableElementCollection(cut, "li") { EnableAutoRefresh = true };

			sut.EnableAutoRefresh = false;

			cut.Find("button").Click();

			sut.Count.ShouldBe(0);
		}
	}
}
