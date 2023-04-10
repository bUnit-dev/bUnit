using AngleSharp.Dom;
using Shouldly;
using Xunit;

namespace AngleSharpWrappers
{
	public class ElementWrapperTest
	{
		private HtmlParser Parser { get; } = new HtmlParser();

		[Fact(DisplayName = "QuerySelectorAll works the same with wrapped and non-wrapped element")]
		public void Test001()
		{
			var elm = Parser.Parse(
				"<div>" +
				"<div id=\"foo\"></div>" +
				"<div id=\"bar\"></div>" +
				"</div>").OfType<IElement>().Single();

			var sut = WrapperFactory.Create(new TestFactory<IElement>(() => elm));

			var sutQueryRes = sut.QuerySelectorAll("div");
			var elmQueryRes = elm.QuerySelectorAll("div");

			sutQueryRes.ShouldBe(elmQueryRes);
		}

		[Fact(DisplayName = "QuerySelector works the same with wrapped and non-wrapped element")]
		public void Test002()
		{
			var elm = Parser.Parse(
				"<div>" +
				"<div id=\"foo\"></div>" +
				"<div id=\"bar\"></div>" +
				"</div>").OfType<IElement>().Single();

			var sut = WrapperFactory.Create(new TestFactory<IElement>(() => elm));

			var sutQueryRes = sut.QuerySelector("#foo");
			var elmQueryRes = elm.QuerySelector("#foo");

			sutQueryRes.ShouldBe(elmQueryRes);
		}
	}
}
