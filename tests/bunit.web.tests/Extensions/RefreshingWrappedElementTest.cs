using Bunit.TestAssets.SampleComponents;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Shouldly;
using Xunit;

namespace Bunit.Extensions
{
	public class RefreshingWrappedElementTest : TestContext
	{
		[Fact(DisplayName = "Find() throws when element doesn't exist in DOM")]
		public void Test001()
		{
			var cut = RenderComponent<Markup>(ps => ps.Add(p => p.Base, "None"));

			Should.Throw<ElementNotFoundException>(() => cut.Find("div"));
		}

		[Fact(DisplayName = "Find() returns the single matching element in DOM")]
		public void Test010()
		{
			var expected = "<div>foo</div>";
			var cut = RenderComponent<Markup>(ps => ps.Add(p => p.Base, expected));

			var actual = cut.Find("div");

			actual.MarkupMatches(expected);
		}

		[Fact(DisplayName = "Find() returns first matching element in DOM")]
		public void Test011()
		{
			var expected = "<div>foo</div>";
			var cut = RenderComponent<Markup>(ps => ps
				.Add(p => p.Base, expected)
				.Add(p => p.Optional, "<div>bar</div>")
				.Add(p => p.ShowOptional, true));

			var actual = cut.Find("div");

			actual.MarkupMatches(expected);
		}

		[Fact(DisplayName = "Find() refreshes the found element on re-renders")]
		public void Test020()
		{
			var cut = RenderComponent<Markup>(ps => ps
				.Add(p => p.Base, "<div>foo</div>")
				.Add(p => p.Optional, "<div>bar</div>")
				.Add(p => p.ShowOptional, false));

			var elm = cut.Find("div:last-child");

			// initially only foo div is rendered
			elm.TextContent.ShouldBe("foo");

			cut.SetParametersAndRender(ps => ps.Add(p => p.ShowOptional, true));

			// after optional markup is included, the refreshed query
			// returns new div as it is now last child
			elm.TextContent.ShouldBe("bar");
		}

		[Fact(DisplayName = "Found element doesn't throw when it's removed from DOM")]
		public void Test030()
		{
			var cut = RenderComponent<HidesButton>();

			var btn = cut.Find("button");

			Should.NotThrow(() => btn.Click());
		}

		[Fact(DisplayName = "Found element throws when its properties or methods are used after it's removed from DOM")]
		public void Test031()
		{
			var cut = RenderComponent<HidesButton>();
			var btn = cut.Find("button");

			btn.Click(); // remove from dom

			Should.Throw<ElementRemovedFromDomException>(() => btn.TextContent);
		}

		private class Markup : ComponentBase
		{
			[Parameter] public string Base { get; set; } = string.Empty;
			[Parameter] public string? Optional { get; set; }
			[Parameter] public bool ShowOptional { get; set; }

			protected override void BuildRenderTree(RenderTreeBuilder builder)
			{
				builder.AddMarkupContent(0, Base);
				if (ShowOptional)
					builder.AddMarkupContent(1, Optional);
			}
		}
	}
}
