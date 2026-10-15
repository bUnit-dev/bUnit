namespace Bunit.Extensions;

public class RefreshingWrappedElementTest : BunitContext
{
	[Fact(DisplayName = "Find() throws when element doesn't exist in DOM")]
	public void Test001()
	{
		var cut = Render<Markup>(ps => ps.Add(p => p.Base, "None"));

		Should.Throw<ElementNotFoundException>(() => cut.Find("div"));
	}

	[Fact(DisplayName = "Find() returns the single matching element in DOM")]
	public void Test010()
	{
		var expected = "<div>foo</div>";
		var cut = Render<Markup>(ps => ps.Add(p => p.Base, expected));

		var actual = cut.Find("div");

		actual.MarkupMatches(expected);
	}

	[Fact(DisplayName = "Find() returns first matching element in DOM")]
	public void Test011()
	{
		var expected = "<div>foo</div>";
		var cut = Render<Markup>(ps => ps
			.Add(p => p.Base, expected)
			.Add(p => p.Optional, "<div>bar</div>")
			.Add(p => p.ShowOptional, true));

		var actual = cut.Find("div");

		actual.MarkupMatches(expected);
	}

	[Fact(DisplayName = "Find() refreshes the found element on re-renders")]
	public void Test020()
	{
		var cut = Render<Markup>(ps => ps
			.Add(p => p.Base, "<div>foo</div>")
			.Add(p => p.Optional, "<div>bar</div>")
			.Add(p => p.ShowOptional, false));

		var elm = cut.Find("div:last-child");

		// initially only foo div is rendered
		elm.TextContent.ShouldBe("foo");

		cut.Render(ps => ps.Add(p => p.ShowOptional, true));

		// after optional markup is included, the refreshed query
		// returns new div as it is now last child
		elm.TextContent.ShouldBe("bar");
	}

	[Fact(DisplayName = "Found element doesn't throw when it's removed from DOM")]
	public void Test030()
	{
		var cut = Render<HidesButton>();

		var btn = cut.Find("button");

		Should.NotThrow(() => btn.Click());
	}

	[Fact(DisplayName = "Found element throws when its properties or methods are used after it's removed from DOM")]
	public void Test031()
	{
		var cut = Render<HidesButton>();
		var btn = cut.Find("button");

		btn.Click(); // remove from dom

		Should.Throw<ElementRemovedFromDomException>(() => btn.TextContent);
	}

	[Fact(DisplayName = "Found element is never null while the component re-renders on another thread")]
	public async Task Test040()
	{
		var cut = Render<Counter>();
		var elm = cut.Find("span");
		using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
		var renders = Task.Run(async () =>
		{
			while (!cts.IsCancellationRequested)
				await cut.InvokeAsync(cut.Instance.Increment);
		});

		try
		{
			while (!cts.IsCancellationRequested)
				_ = elm.TextContent;
		}
		finally
		{
			await cts.CancelAsync();
			await renders;
		}
	}

	private sealed class Markup : ComponentBase
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

	private sealed class Counter : ComponentBase
	{
		private int count;

		public void Increment()
		{
			count++;
			StateHasChanged();
		}

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, "span");
			builder.AddContent(1, count);
			builder.CloseElement();
		}
	}
}
