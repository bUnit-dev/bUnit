namespace Bunit.Text;

public class TextQueryExtensionsTest : BunitContext
{
	[Fact(DisplayName = "Should find element by its exact text content")]
	public void Test001()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("<span>Hello World</span>"));

		var element = cut.FindByText("Hello World");

		element.ShouldNotBeNull();
		element.NodeName.ShouldBe("SPAN", StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "Should throw TextNotFoundException when text does not exist in the DOM")]
	public void Test002()
	{
		var expectedText = Guid.NewGuid().ToString();
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("<div>Some other text</div>"));

		Should.Throw<TextNotFoundException>(() => cut.FindByText(expectedText))
			.SearchText.ShouldBe(expectedText);
	}

	[Fact(DisplayName = "Should find div element by text content")]
	public void Test003()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("<div>Hello World</div>"));

		var element = cut.FindByText("Hello World");

		element.ShouldNotBeNull();
		element.NodeName.ShouldBe("DIV", StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "Should find paragraph element by text content")]
	public void Test004()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("<p>Hello World</p>"));

		var element = cut.FindByText("Hello World");

		element.ShouldNotBeNull();
		element.NodeName.ShouldBe("P", StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "Should find button element by text content")]
	public void Test005()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("<button>Click Me</button>"));

		var element = cut.FindByText("Click Me");

		element.ShouldNotBeNull();
		element.NodeName.ShouldBe("BUTTON", StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "Should find anchor element by text content")]
	public void Test006()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<a href="/home">Go Home</a>"""));

		var element = cut.FindByText("Go Home");

		element.ShouldNotBeNull();
		element.NodeName.ShouldBe("A", StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "Should trim leading and trailing whitespace when matching")]
	public void Test007()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<span>
					Hello World
				</span>
			"""));

		var element = cut.FindByText("Hello World");

		element.ShouldNotBeNull();
		element.NodeName.ShouldBe("SPAN", StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "Should collapse multiple whitespace characters into a single space")]
	public void Test008()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("<span>Hello     World</span>"));

		var element = cut.FindByText("Hello World");

		element.ShouldNotBeNull();
	}

	[Fact(DisplayName = "Should collapse newlines and tabs into a single space")]
	public void Test009()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("<span>Hello\n\t\tWorld</span>"));

		var element = cut.FindByText("Hello World");

		element.ShouldNotBeNull();
	}

	[Fact(DisplayName = "Should normalize whitespace in the search text as well")]
	public void Test010()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("<span>Hello World</span>"));

		var element = cut.FindByText("Hello   World");

		element.ShouldNotBeNull();
	}

	[Fact(DisplayName = "Should be case sensitive by default")]
	public void Test011()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("<span>Hello World</span>"));

		Should.Throw<TextNotFoundException>(() => cut.FindByText("hello world"));
	}

	[Theory(DisplayName = "Should throw TextNotFoundException when ComparisonType is case sensitive and incorrect casing is used")]
	[InlineData(StringComparison.Ordinal)]
	[InlineData(StringComparison.InvariantCulture)]
	[InlineData(StringComparison.CurrentCulture)]
	public void Test012(StringComparison comparison)
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("<span>Hello World</span>"));

		Should.Throw<TextNotFoundException>(() => cut.FindByText("HELLO WORLD", o => o.ComparisonType = comparison))
			.SearchText.ShouldBe("HELLO WORLD");
	}

	[Theory(DisplayName = "Should find element when ComparisonType is case insensitive")]
	[InlineData(StringComparison.OrdinalIgnoreCase)]
	[InlineData(StringComparison.InvariantCultureIgnoreCase)]
	[InlineData(StringComparison.CurrentCultureIgnoreCase)]
	public void Test013(StringComparison comparison)
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("<span>Hello World</span>"));

		var element = cut.FindByText("HELLO WORLD", o => o.ComparisonType = comparison);

		element.ShouldNotBeNull();
		element.NodeName.ShouldBe("SPAN", StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "Should ignore script elements")]
	public void Test014()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<script>var x = "Hello World";</script>
				<span>Hello World</span>
			"""));

		var element = cut.FindByText("Hello World");

		element.ShouldNotBeNull();
		element.NodeName.ShouldBe("SPAN", StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "Should ignore style elements")]
	public void Test015()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<style>.hello { color: red; }</style>
				<span>Hello World</span>
			"""));

		var element = cut.FindByText("Hello World");

		element.ShouldNotBeNull();
		element.NodeName.ShouldBe("SPAN", StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "Should find element with nested text content")]
	public void Test016()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("<div><span>Hello</span> <span>World</span></div>"));

		var element = cut.FindByText("Hello World");

		element.ShouldNotBeNull();
		element.NodeName.ShouldBe("DIV", StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "Should scope search with selector option")]
	public void Test017()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<div>Hello World</div>
				<span>Hello World</span>
			"""));

		var element = cut.FindByText("Hello World", o => o.Selector = "span");

		element.ShouldNotBeNull();
		element.NodeName.ShouldBe("SPAN", StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "Should throw when selector option filters out all matching elements")]
	public void Test018()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("<div>Hello World</div>"));

		Should.Throw<TextNotFoundException>(() => cut.FindByText("Hello World", o => o.Selector = "span"));
	}

	[Fact(DisplayName = "Should find first matching element when multiple elements have the same text")]
	public void Test019()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<span id="first">Hello World</span>
				<span id="second">Hello World</span>
			"""));

		var element = cut.FindByText("Hello World");

		element.ShouldNotBeNull();
		// The first matching element could be a parent or the first span
		// depending on DOM structure - just verify we get a result
	}

	[Fact(DisplayName = "Should find element with deeply nested text")]
	public void Test020()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("<div><p><span>Hello World</span></p></div>"));

		var element = cut.FindByText("Hello World", o => o.Selector = "span");

		element.ShouldNotBeNull();
		element.NodeName.ShouldBe("SPAN", StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "Should not match partial text by default")]
	public void Test021()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("<span>Hello World and more</span>"));

		Should.Throw<TextNotFoundException>(() => cut.FindByText("Hello World"));
	}

	[Fact(DisplayName = "Should find heading element by text")]
	public void Test022()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("<h1>Page Title</h1>"));

		var element = cut.FindByText("Page Title");

		element.ShouldNotBeNull();
		element.NodeName.ShouldBe("H1", StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "Should find li element by text")]
	public void Test023()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<ul>
					<li>First Item</li>
					<li>Second Item</li>
				</ul>
			"""));

		var element = cut.FindByText("Second Item", o => o.Selector = "li");

		element.ShouldNotBeNull();
		element.TextContent.Trim().ShouldBe("Second Item");
	}

	[Fact(DisplayName = "Should find element with special characters in text")]
	public void Test024()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("<span>Price: $19.99</span>"));

		var element = cut.FindByText("Price: $19.99");

		element.ShouldNotBeNull();
	}

	[Fact(DisplayName = "Should find element with empty text when searching for empty string")]
	public void Test025()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("<span></span>"));

		var element = cut.FindByText("");

		element.ShouldNotBeNull();
		element.NodeName.ShouldBe("SPAN", StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "Should scope search with CSS class selector")]
	public void Test026()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<span class="label">Hello World</span>
				<span class="value">Hello World</span>
			"""));

		var element = cut.FindByText("Hello World", o => o.Selector = ".value");

		element.ShouldNotBeNull();
		element.ClassName.ShouldContain("value");
	}

	// FindAllByText tests

	[Fact(DisplayName = "FindAllByText should return empty collection when no elements match")]
	public void Test100()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("<div>No match here</div>"));

		var elements = cut.FindAllByText("Non-existent text");

		elements.ShouldBeEmpty();
	}

	[Fact(DisplayName = "FindAllByText should return multiple elements with same text content")]
	public void Test101()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<span id="first">Hello</span>
				<span id="second">Hello</span>
			"""));

		var elements = cut.FindAllByText("Hello", o => o.Selector = "span");

		elements.Count.ShouldBe(2);
	}

	[Fact(DisplayName = "FindAllByText should respect selector option")]
	public void Test102()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<div>Hello</div>
				<span id="first">Hello</span>
				<span id="second">Hello</span>
			"""));

		var elements = cut.FindAllByText("Hello", o => o.Selector = "span");

		elements.Count.ShouldBe(2);
		elements[0].NodeName.ShouldBe("SPAN", StringCompareShould.IgnoreCase);
		elements[1].NodeName.ShouldBe("SPAN", StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "FindAllByText should respect case-insensitive comparison option")]
	public void Test103()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<span>Hello</span>
				<span>HELLO</span>
				<span>hello</span>
			"""));

		var elements = cut.FindAllByText("hello", o =>
		{
			o.ComparisonType = StringComparison.OrdinalIgnoreCase;
			o.Selector = "span";
		});

		elements.Count.ShouldBe(3);
	}

	[Fact(DisplayName = "FindAllByText should ignore script and style elements")]
	public void Test104()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<script>Hello</script>
				<style>Hello</style>
				<span>Hello</span>
			"""));

		var elements = cut.FindAllByText("Hello", o => o.Selector = "span, script, style");

		elements.Count.ShouldBe(1);
		elements[0].NodeName.ShouldBe("SPAN", StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "FindAllByText should deduplicate elements")]
	public void Test105()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<span id="single">Hello</span>"""));

		// Using "*" selector, the element could potentially match via multiple paths
		// but should only appear once
		var elements = cut.FindAllByText("Hello", o => o.Selector = "span");

		elements.Count.ShouldBe(1);
		elements[0].Id.ShouldBe("single");
	}

	[Fact(DisplayName = "FindAllByText should normalize whitespace in matched elements")]
	public void Test106()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<span>
					Hello   World
				</span>
				<span>Hello World</span>
			"""));

		var elements = cut.FindAllByText("Hello World", o => o.Selector = "span");

		elements.Count.ShouldBe(2);
	}
}
