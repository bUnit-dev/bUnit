namespace Bunit.Roles;

public class RoleQueryExtensionsTest : BunitContext
{
	[Fact(DisplayName = "Should return element with explicit role attribute")]
	public void Test001()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<div role="button">Click me</div>"""));

		var element = cut.FindByRole(AriaRole.Button);

		element.ShouldNotBeNull();
		element.TextContent.ShouldBe("Click me");
	}

	[Fact(DisplayName = "Should throw RoleNotFoundException when role does not exist")]
	public void Test002()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<div>Just a div</div>"""));

		var exception = Should.Throw<RoleNotFoundException>(() => cut.FindByRole(AriaRole.Button));
		exception.Role.ShouldBe(AriaRole.Button);
	}

	[Fact(DisplayName = "FindAllByRole should return empty collection when no matches")]
	public void Test003()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<div>Just a div</div>"""));

		var elements = cut.FindAllByRole(AriaRole.Button);

		elements.ShouldBeEmpty();
	}

	[Fact(DisplayName = "FindAllByRole should return all matching elements")]
	public void Test004()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<button>Button 1</button>
				<button>Button 2</button>
				<button>Button 3</button>
			"""));

		var elements = cut.FindAllByRole(AriaRole.Button);

		elements.Count.ShouldBe(3);
	}	[Fact(DisplayName = "Should find button element by implicit button role")]
	public void Test010()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<button>Click me</button>"""));

		var element = cut.FindByRole(AriaRole.Button);

		element.ShouldNotBeNull();
		element.TagName.ShouldBe("BUTTON", StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "Should find input type=button by implicit button role")]
	public void Test011()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<input type="button" value="Click me" />"""));

		var element = cut.FindByRole(AriaRole.Button);

		element.ShouldNotBeNull();
		element.GetAttribute("type").ShouldBe("button");
	}

	[Fact(DisplayName = "Should find input type=submit by implicit button role")]
	public void Test012()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<input type="submit" value="Submit" />"""));

		var element = cut.FindByRole(AriaRole.Button);

		element.ShouldNotBeNull();
		element.GetAttribute("type").ShouldBe("submit");
	}

	[Fact(DisplayName = "Should find input type=reset by implicit button role")]
	public void Test013()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<input type="reset" value="Reset" />"""));

		var element = cut.FindByRole(AriaRole.Button);

		element.ShouldNotBeNull();
		element.GetAttribute("type").ShouldBe("reset");
	}

	[Fact(DisplayName = "Should find summary element by implicit button role")]
	public void Test014()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<details><summary>Details</summary>Content</details>"""));

		var element = cut.FindByRole(AriaRole.Button);

		element.ShouldNotBeNull();
		element.TagName.ShouldBe("SUMMARY", StringCompareShould.IgnoreCase);
	}
	
	[Fact(DisplayName = "Should find anchor with href by implicit link role")]
	public void Test020()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<a href="/home">Home</a>"""));

		var element = cut.FindByRole(AriaRole.Link);

		element.ShouldNotBeNull();
		element.TagName.ShouldBe("A", StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "Should not find anchor without href by link role")]
	public void Test021()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<a>Not a link</a>"""));

		var elements = cut.FindAllByRole(AriaRole.Link);

		elements.ShouldBeEmpty();
	}

	[Theory(DisplayName = "Should find heading elements by implicit heading role")]
	[InlineData("h1")]
	[InlineData("h2")]
	[InlineData("h3")]
	[InlineData("h4")]
	[InlineData("h5")]
	[InlineData("h6")]
	public void Test030(string headingTag)
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent($"<{headingTag}>Heading</{headingTag}>"));

		var element = cut.FindByRole(AriaRole.Heading);

		element.ShouldNotBeNull();
		element.TagName.ShouldBe(headingTag, StringCompareShould.IgnoreCase);
	}

	[Theory(DisplayName = "Should filter headings by level option")]
	[InlineData(1, "h1")]
	[InlineData(2, "h2")]
	[InlineData(3, "h3")]
	[InlineData(4, "h4")]
	[InlineData(5, "h5")]
	[InlineData(6, "h6")]
	public void Test031(int level, string expectedTag)
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<h1>Level 1</h1>
				<h2>Level 2</h2>
				<h3>Level 3</h3>
				<h4>Level 4</h4>
				<h5>Level 5</h5>
				<h6>Level 6</h6>
			"""));

		var element = cut.FindByRole(AriaRole.Heading, o => o.Level = level);

		element.ShouldNotBeNull();
		element.TagName.ShouldBe(expectedTag, StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "Should find all headings when no level specified")]
	public void Test032()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<h1>Level 1</h1>
				<h2>Level 2</h2>
				<h3>Level 3</h3>
			"""));

		var elements = cut.FindAllByRole(AriaRole.Heading);

		elements.Count.ShouldBe(3);
	}

	[Fact(DisplayName = "Should respect aria-level attribute for heading level")]
	public void Test033()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<div role="heading" aria-level="2">Custom Heading</div>"""));

		var element = cut.FindByRole(AriaRole.Heading, o => o.Level = 2);

		element.ShouldNotBeNull();
		element.TextContent.ShouldBe("Custom Heading");
	}
	
	[Fact(DisplayName = "Should find checkbox input by implicit checkbox role")]
	public void Test040()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<input type="checkbox" />"""));

		var element = cut.FindByRole(AriaRole.Checkbox);

		element.ShouldNotBeNull();
		element.GetAttribute("type").ShouldBe("checkbox");
	}

	[Fact(DisplayName = "Should filter checkbox by checked state")]
	public void Test041()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<input type="checkbox" id="unchecked" />
				<input type="checkbox" id="checked" checked />
			"""));

		var element = cut.FindByRole(AriaRole.Checkbox, o => o.Checked = true);

		element.ShouldNotBeNull();
		element.Id.ShouldBe("checked");
	}

	[Fact(DisplayName = "Should filter checkbox by unchecked state")]
	public void Test042()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<input type="checkbox" id="unchecked" />
				<input type="checkbox" id="checked" checked />
			"""));

		var element = cut.FindByRole(AriaRole.Checkbox, o => o.Checked = false);

		element.ShouldNotBeNull();
		element.Id.ShouldBe("unchecked");
	}

	[Fact(DisplayName = "Should find radio input by implicit radio role")]
	public void Test050()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<input type="radio" name="choice" />"""));

		var element = cut.FindByRole(AriaRole.Radio);

		element.ShouldNotBeNull();
		element.GetAttribute("type").ShouldBe("radio");
	}

	[Fact(DisplayName = "Should find text input by implicit textbox role")]
	public void Test060()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<input type="text" />"""));

		var element = cut.FindByRole(AriaRole.Textbox);

		element.ShouldNotBeNull();
		element.GetAttribute("type").ShouldBe("text");
	}

	[Fact(DisplayName = "Should find textarea by implicit textbox role")]
	public void Test061()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<textarea></textarea>"""));

		var element = cut.FindByRole(AriaRole.Textbox);

		element.ShouldNotBeNull();
		element.TagName.ShouldBe("TEXTAREA", StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "Should find email input by implicit textbox role")]
	public void Test062()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<input type="email" />"""));

		var element = cut.FindByRole(AriaRole.Textbox);

		element.ShouldNotBeNull();
		element.GetAttribute("type").ShouldBe("email");
	}

	[Theory(DisplayName = "Should find list elements by implicit list role")]
	[InlineData("ul")]
	[InlineData("ol")]
	public void Test070(string listTag)
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent($"<{listTag}><li>Item</li></{listTag}>"));

		var element = cut.FindByRole(AriaRole.List);

		element.ShouldNotBeNull();
		element.TagName.ShouldBe(listTag, StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "Should find li element by implicit listitem role")]
	public void Test071()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<ul><li>Item</li></ul>"""));

		var element = cut.FindByRole(AriaRole.Listitem);

		element.ShouldNotBeNull();
		element.TagName.ShouldBe("LI", StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "Should find nav element by implicit navigation role")]
	public void Test080()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<nav><a href="/">Home</a></nav>"""));

		var element = cut.FindByRole(AriaRole.Navigation);

		element.ShouldNotBeNull();
		element.TagName.ShouldBe("NAV", StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "Should find main element by implicit main role")]
	public void Test081()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<main>Content</main>"""));

		var element = cut.FindByRole(AriaRole.Main);

		element.ShouldNotBeNull();
		element.TagName.ShouldBe("MAIN", StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "Should find aside element by implicit complementary role")]
	public void Test082()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<aside>Sidebar</aside>"""));

		var element = cut.FindByRole(AriaRole.Complementary);

		element.ShouldNotBeNull();
		element.TagName.ShouldBe("ASIDE", StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "Should filter by accessible name from aria-label")]
	public void Test090()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<button aria-label="Submit form">Submit</button>
				<button aria-label="Cancel form">Cancel</button>
			"""));

		var element = cut.FindByRole(AriaRole.Button, o => o.Name = "Submit form");

		element.ShouldNotBeNull();
		element.TextContent.ShouldBe("Submit");
	}

	[Fact(DisplayName = "Should filter by accessible name from button text content")]
	public void Test091()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<button>Submit</button>
				<button>Cancel</button>
			"""));

		var element = cut.FindByRole(AriaRole.Button, o => o.Name = "Submit");

		element.ShouldNotBeNull();
		element.TextContent.ShouldBe("Submit");
	}

	[Fact(DisplayName = "Should filter by accessible name from aria-labelledby")]
	public void Test092()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<span id="label1">First Label</span>
				<button aria-labelledby="label1">Button 1</button>
				<span id="label2">Second Label</span>
				<button aria-labelledby="label2">Button 2</button>
			"""));

		var element = cut.FindByRole(AriaRole.Button, o => o.Name = "First Label");

		element.ShouldNotBeNull();
		element.TextContent.ShouldBe("Button 1");
	}

	[Fact(DisplayName = "Should throw RoleNotFoundException when name does not match")]
	public void Test093()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<button>Submit</button>"""));

		var exception = Should.Throw<RoleNotFoundException>(() =>
			cut.FindByRole(AriaRole.Button, o => o.Name = "NonExistent"));
		exception.AccessibleName.ShouldBe("NonExistent");
	}

	[Theory(DisplayName = "Should respect ComparisonType for name filter")]
	[InlineData(StringComparison.OrdinalIgnoreCase, true)]
	[InlineData(StringComparison.Ordinal, false)]
	public void Test094(StringComparison comparison, bool shouldFind)
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<button>Submit</button>"""));

		if (shouldFind)
		{
			var element = cut.FindByRole(AriaRole.Button, o =>
			{
				o.Name = "SUBMIT";
				o.ComparisonType = comparison;
			});
			element.ShouldNotBeNull();
		}
		else
		{
			Should.Throw<RoleNotFoundException>(() =>
				cut.FindByRole(AriaRole.Button, o =>
				{
					o.Name = "SUBMIT";
					o.ComparisonType = comparison;
				}));
		}
	}

	[Fact(DisplayName = "Should filter by accessible name from linked label")]
	public void Test095()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<label for="email">Email Address</label>
				<input type="text" id="email" />
			"""));

		var element = cut.FindByRole(AriaRole.Textbox, o => o.Name = "Email Address");

		element.ShouldNotBeNull();
		element.Id.ShouldBe("email");
	}

	[Fact(DisplayName = "Should exclude aria-hidden elements by default")]
	public void Test100()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<button aria-hidden="true">Hidden</button>
				<button>Visible</button>
			"""));

		var elements = cut.FindAllByRole(AriaRole.Button);

		elements.Count.ShouldBe(1);
		elements[0].TextContent.ShouldBe("Visible");
	}

	[Fact(DisplayName = "Should include aria-hidden elements when Hidden option is true")]
	public void Test101()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<button aria-hidden="true">Hidden</button>
				<button>Visible</button>
			"""));

		var elements = cut.FindAllByRole(AriaRole.Button, o => o.Hidden = true);

		elements.Count.ShouldBe(2);
	}

	[Fact(DisplayName = "Should exclude elements with hidden attribute by default")]
	public void Test102()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<button hidden>Hidden</button>
				<button>Visible</button>
			"""));

		var elements = cut.FindAllByRole(AriaRole.Button);

		elements.Count.ShouldBe(1);
		elements[0].TextContent.ShouldBe("Visible");
	}

	[Fact(DisplayName = "Should filter by pressed state")]
	public void Test110()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<button aria-pressed="true">Pressed</button>
				<button aria-pressed="false">Not Pressed</button>
			"""));

		var element = cut.FindByRole(AriaRole.Button, o => o.Pressed = true);

		element.ShouldNotBeNull();
		element.TextContent.ShouldBe("Pressed");
	}

	[Fact(DisplayName = "Should filter by expanded state")]
	public void Test120()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<button aria-expanded="true">Expanded</button>
				<button aria-expanded="false">Collapsed</button>
			"""));

		var element = cut.FindByRole(AriaRole.Button, o => o.Expanded = true);

		element.ShouldNotBeNull();
		element.TextContent.ShouldBe("Expanded");
	}

	[Fact(DisplayName = "Should detect expanded state from details open attribute")]
	public void Test121()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<details open>
					<summary>Open Details</summary>
					Content
				</details>
				<details>
					<summary>Closed Details</summary>
					Content
				</details>
			"""));

		var element = cut.FindByRole(AriaRole.Group, o => o.Expanded = true);

		element.ShouldNotBeNull();
		element.HasAttribute("open").ShouldBeTrue();
	}

	[Fact(DisplayName = "Should filter by selected state")]
	public void Test130()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<div role="tablist">
					<button role="tab" aria-selected="true">Tab 1</button>
					<button role="tab" aria-selected="false">Tab 2</button>
				</div>
			"""));

		var element = cut.FindByRole(AriaRole.Tab, o => o.Selected = true);

		element.ShouldNotBeNull();
		element.TextContent.ShouldBe("Tab 1");
	}

	[Fact(DisplayName = "Should filter by busy state")]
	public void Test140()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<button aria-busy="true">Loading</button>
				<button aria-busy="false">Ready</button>
			"""));

		var element = cut.FindByRole(AriaRole.Button, o => o.Busy = true);

		element.ShouldNotBeNull();
		element.TextContent.ShouldBe("Loading");
	}

	[Fact(DisplayName = "Should filter by current state with boolean")]
	public void Test150()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<a href="/" aria-current="true">Current</a>
				<a href="/about" aria-current="false">About</a>
			"""));

		var element = cut.FindByRole(AriaRole.Link, o => o.Current = true);

		element.ShouldNotBeNull();
		element.TextContent.ShouldBe("Current");
	}

	[Fact(DisplayName = "Should filter by current state with token value")]
	public void Test151()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<a href="/" aria-current="page">Home</a>
				<a href="/about" aria-current="false">About</a>
			"""));

		var element = cut.FindByRole(AriaRole.Link, o => o.Current = "page");

		element.ShouldNotBeNull();
		element.TextContent.ShouldBe("Home");
	}

	[Fact(DisplayName = "Should combine multiple filters")]
	public void Test160()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<button aria-pressed="true">Bold</button>
				<button aria-pressed="false">Italic</button>
				<button aria-pressed="true">Underline</button>
			"""));

		var element = cut.FindByRole(AriaRole.Button, o =>
		{
			o.Pressed = true;
			o.Name = "Bold";
		});

		element.ShouldNotBeNull();
		element.TextContent.ShouldBe("Bold");
	}

	[Fact(DisplayName = "Should find elements by combobox role for select")]
	public void Test170()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<select>
					<option>Option 1</option>
					<option>Option 2</option>
				</select>
			"""));

		var element = cut.FindByRole(AriaRole.Combobox);

		element.ShouldNotBeNull();
		element.TagName.ShouldBe("SELECT", StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "Should find elements by listbox role for select with multiple")]
	public void Test171()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<select multiple>
					<option>Option 1</option>
					<option>Option 2</option>
				</select>
			"""));

		var element = cut.FindByRole(AriaRole.Listbox);

		element.ShouldNotBeNull();
		element.TagName.ShouldBe("SELECT", StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "Should find img element by img role")]
	public void Test180()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<img src="test.png" alt="Test image" />"""));

		var element = cut.FindByRole(AriaRole.Img);

		element.ShouldNotBeNull();
		element.GetAttribute("alt").ShouldBe("Test image");
	}

	[Fact(DisplayName = "Should find img with empty alt as presentation role")]
	public void Test181()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<img src="test.png" alt="" />"""));

		var element = cut.FindByRole(AriaRole.Presentation);

		element.ShouldNotBeNull();
	}

	[Fact(DisplayName = "Should filter img by accessible name from alt")]
	public void Test182()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<img src="logo.png" alt="Company Logo" />
				<img src="banner.png" alt="Banner Image" />
			"""));

		var element = cut.FindByRole(AriaRole.Img, o => o.Name = "Company Logo");

		element.ShouldNotBeNull();
		element.GetAttribute("src").ShouldBe("logo.png");
	}

	[Fact(DisplayName = "Should find table element by table role")]
	public void Test190()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<table>
					<tr><th>Header</th></tr>
					<tr><td>Cell</td></tr>
				</table>
			"""));

		var element = cut.FindByRole(AriaRole.Table);

		element.ShouldNotBeNull();
		element.TagName.ShouldBe("TABLE", StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "Should find row elements by row role")]
	public void Test191()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<table>
					<tr><td>Row 1</td></tr>
					<tr><td>Row 2</td></tr>
				</table>
			"""));

		var elements = cut.FindAllByRole(AriaRole.Row);

		elements.Count.ShouldBe(2);
	}

	[Fact(DisplayName = "Should find cell elements by cell role")]
	public void Test192()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<table>
					<tr><td>Cell 1</td><td>Cell 2</td></tr>
				</table>
			"""));

		var elements = cut.FindAllByRole(AriaRole.Cell);

		elements.Count.ShouldBe(2);
	}

	[Fact(DisplayName = "Should find progress element by progressbar role")]
	public void Test200()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<progress value="50" max="100">50%</progress>"""));

		var element = cut.FindByRole(AriaRole.Progressbar);

		element.ShouldNotBeNull();
		element.TagName.ShouldBe("PROGRESS", StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "Should find meter element by meter role")]
	public void Test201()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<meter value="0.6">60%</meter>"""));

		var element = cut.FindByRole(AriaRole.Meter);

		element.ShouldNotBeNull();
		element.TagName.ShouldBe("METER", StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "Should find slider input by slider role")]
	public void Test202()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<input type="range" min="0" max="100" />"""));

		var element = cut.FindByRole(AriaRole.Slider);

		element.ShouldNotBeNull();
		element.GetAttribute("type").ShouldBe("range");
	}

	[Fact(DisplayName = "Should find spinbutton input by spinbutton role")]
	public void Test203()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<input type="number" />"""));

		var element = cut.FindByRole(AriaRole.Spinbutton);

		element.ShouldNotBeNull();
		element.GetAttribute("type").ShouldBe("number");
	}

	[Fact(DisplayName = "Should find dialog element by dialog role")]
	public void Test210()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<dialog open>Dialog content</dialog>"""));

		var element = cut.FindByRole(AriaRole.Dialog);

		element.ShouldNotBeNull();
		element.TagName.ShouldBe("DIALOG", StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "Should find search input by searchbox role")]
	public void Test220()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<input type="search" />"""));

		var element = cut.FindByRole(AriaRole.Searchbox);

		element.ShouldNotBeNull();
		element.GetAttribute("type").ShouldBe("search");
	}
}
