using Bunit.TestAssets.BlazorE2E;

namespace Bunit.Roles;

public class RoleQueryExtensionsTest : BunitContext
{
	[Fact(DisplayName = "should detect roles")]
	public void Should_detect_roles()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<button>Hello</button>
				<select multiple="" size="2"></select>
				<select></select>
				<h3>Heading</h3>
				<details><summary>Hello</summary></details>
				<div role="dialog">I am a dialog</div>
				"""));

		var buttons = cut.FindAllByRole(AriaRole.Button);
		buttons.Count.ShouldBe(1);
		buttons[0].OuterHtml.ShouldBe("<button>Hello</button>");

		var listboxes = cut.FindAllByRole(AriaRole.Listbox);
		listboxes.Count.ShouldBe(1);
		listboxes[0].NodeName.ShouldBe("SELECT");

		var comboboxes = cut.FindAllByRole(AriaRole.Combobox);
		comboboxes.Count.ShouldBe(1);
		comboboxes[0].NodeName.ShouldBe("SELECT");

		var headings = cut.FindAllByRole(AriaRole.Heading);
		headings.Count.ShouldBe(1);
		headings[0].OuterHtml.ShouldBe("<h3>Heading</h3>");

		var groups = cut.FindAllByRole(AriaRole.Group);
		groups.Count.ShouldBe(1);
		groups[0].NodeName.ShouldBe("DETAILS");

		var dialogs = cut.FindAllByRole(AriaRole.Dialog);
		dialogs.Count.ShouldBe(1);
		dialogs[0].OuterHtml.ShouldBe("""<div role="dialog">I am a dialog</div>""");

		var menuitems = cut.FindAllByRole(AriaRole.Menuitem);
		menuitems.Count.ShouldBe(0);
	}

	[Fact(DisplayName = "should support explicit role overriding implicit")]
	public void Should_support_explicit_role_overriding_implicit()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<div role="button">Click</div>
				<button role="tab">Tab</button>
				"""));

		var buttons = cut.FindAllByRole(AriaRole.Button);
		buttons.Count.ShouldBe(1);
		buttons[0].OuterHtml.ShouldBe("""<div role="button">Click</div>""");

		var tabs = cut.FindAllByRole(AriaRole.Tab);
		tabs.Count.ShouldBe(1);
		tabs[0].OuterHtml.ShouldBe("""<button role="tab">Tab</button>""");
	}

	[Fact(DisplayName = "should throw when no match")]
	public void Should_throw_when_no_match()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<div>No roles here</div>"""));

		var exception = Should.Throw<RoleNotFoundException>(() => cut.FindByRole(AriaRole.Button));
		exception.Role.ShouldBe(AriaRole.Button);
		exception.Message.ShouldContain("button");
	}

	[Fact(DisplayName = "should return empty collection when no match")]
	public void Should_return_empty_collection_when_no_match()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<div>No roles here</div>"""));

		var elements = cut.FindAllByRole(AriaRole.Button);
		elements.ShouldBeEmpty();
	}

	[Fact(DisplayName = "should support name")]
	public void Should_support_name()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<div role="button" aria-label=" Hello "></div>
				<div role="button" aria-label="Hallo"></div>
				"""));

		// Exact name match with whitespace trimming — Playwright trims accessible names
		var helloButtons = cut.FindAllByRole(AriaRole.Button, o => o.Name = "Hello");
		helloButtons.Count.ShouldBe(1);
		helloButtons[0].GetAttribute("aria-label").ShouldBe(" Hello ");

		// Case-insensitive name match
		var caseInsensitive = cut.FindAllByRole(AriaRole.Button, o =>
		{
			o.Name = "hello";
			o.NameComparisonType = StringComparison.OrdinalIgnoreCase;
		});
		caseInsensitive.Count.ShouldBe(1);
		caseInsensitive[0].GetAttribute("aria-label").ShouldBe(" Hello ");

		// Substring match with Exact=false
		var substringMatch = cut.FindAllByRole(AriaRole.Button, o =>
		{
			o.Name = "all";
			o.Exact = false;
		});
		substringMatch.Count.ShouldBe(1);
		substringMatch[0].GetAttribute("aria-label").ShouldBe("Hallo");

		// All buttons returned without name filter
		var allButtons = cut.FindAllByRole(AriaRole.Button);
		allButtons.Count.ShouldBe(2);
	}

	[Fact(DisplayName = "should support output accessible name")]
	public void Should_support_output_accessible_name()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<label>Output1<output>output</output></label>"""));

		var result = cut.FindByRole(AriaRole.Status, o => o.Name = "Output1");
		result.NodeName.ShouldBe("OUTPUT");
	}

	[Fact(DisplayName = "should support native controls name")]
	public void Should_support_native_controls_name()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<label for="text1">TEXT1</label><input id="text1" type="text" />
				<input id="text2" type="text" title="TEXT2" />
				<input id="text3" type="text" placeholder="TEXT3" />

				<button id="button3">BUTTON3</button>
				<button id="button4" title="BUTTON4"></button>

				<input id="image2" type="image" alt="IMAGE2" />
				"""));

		cut.FindByRole(AriaRole.Textbox, o => o.Name = "TEXT1").Id.ShouldBe("text1");
		cut.FindByRole(AriaRole.Textbox, o => o.Name = "TEXT2").Id.ShouldBe("text2");
		cut.FindByRole(AriaRole.Textbox, o => o.Name = "TEXT3").Id.ShouldBe("text3");
		cut.FindByRole(AriaRole.Button, o => o.Name = "BUTTON3").Id.ShouldBe("button3");
		cut.FindByRole(AriaRole.Button, o => o.Name = "BUTTON4").Id.ShouldBe("button4");
		cut.FindByRole(AriaRole.Button, o => o.Name = "IMAGE2").Id.ShouldBe("image2");
	}

	[Fact(DisplayName = "should support native controls labelled-by")]
	public void Should_support_native_controls_labelled_by()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<label id="for-text1">TEXT1</label><input aria-labelledby="for-text1" id="text1" type="text" />
				<label id="for-button1">BUTTON1</label><button aria-labelledby="for-button1" id="button1">MORE1</button>
				<label id="for-button2">BUTTON2</label><button aria-labelledby="for-button2 button2" id="button2">MORE2</button>
				"""));

		cut.FindByRole(AriaRole.Textbox, o => o.Name = "TEXT1").Id.ShouldBe("text1");
		cut.FindByRole(AriaRole.Button, o => o.Name = "BUTTON1").Id.ShouldBe("button1");
		cut.FindByRole(AriaRole.Button, o => o.Name = "BUTTON2 MORE2").Id.ShouldBe("button2");
	}

	[Fact(DisplayName = "should support level")]
	public void Should_support_level()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<h1>Hello</h1>
				<h3>Hi</h3>
				<div role="heading" aria-level="5">Bye</div>
				"""));

		var level1 = cut.FindAllByRole(AriaRole.Heading, o => o.Level = 1);
		level1.Count.ShouldBe(1);
		level1[0].OuterHtml.ShouldBe("<h1>Hello</h1>");

		var level3 = cut.FindAllByRole(AriaRole.Heading, o => o.Level = 3);
		level3.Count.ShouldBe(1);
		level3[0].OuterHtml.ShouldBe("<h3>Hi</h3>");

		var level5 = cut.FindAllByRole(AriaRole.Heading, o => o.Level = 5);
		level5.Count.ShouldBe(1);
		level5[0].OuterHtml.ShouldBe("""<div role="heading" aria-level="5">Bye</div>""");
	}

	[Fact(DisplayName = "should support selected")]
	public void Should_support_selected()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<select>
					<option>Hi</option>
					<option selected>Hello</option>
				</select>
				<div>
					<div role="option" aria-selected="true">Hi</div>
					<div role="option" aria-selected="false">Hello</div>
				</div>
				"""));

		var selectedTrue = cut.FindAllByRole(AriaRole.Option, o => o.Selected = true);
		selectedTrue.Count.ShouldBe(2);
		selectedTrue[0].TextContent.ShouldBe("Hello");
		selectedTrue[1].TextContent.ShouldBe("Hi");

		var selectedFalse = cut.FindAllByRole(AriaRole.Option, o => o.Selected = false);
		selectedFalse.Count.ShouldBe(2);
		selectedFalse[0].TextContent.ShouldBe("Hi");
		selectedFalse[1].TextContent.ShouldBe("Hello");
	}

	[Fact(DisplayName = "should support checked")]
	public void Should_support_checked()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<input type="checkbox" />
				<input type="checkbox" checked />
				<div role="checkbox" aria-checked="true">Hi</div>
				<div role="checkbox" aria-checked="false">Hello</div>
				<div role="checkbox">Unknown</div>
				"""));

		var checkedTrue = cut.FindAllByRole(AriaRole.Checkbox, o => o.Checked = true);
		checkedTrue.Count.ShouldBe(2);

		var checkedFalse = cut.FindAllByRole(AriaRole.Checkbox, o => o.Checked = false);
		checkedFalse.Count.ShouldBe(3);

		var all = cut.FindAllByRole(AriaRole.Checkbox);
		all.Count.ShouldBe(5);
	}

	[Fact(DisplayName = "should support pressed")]
	public void Should_support_pressed()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<button>Hi</button>
				<button aria-pressed="true">Hello</button>
				<button aria-pressed="false">Bye</button>
				<button aria-pressed="mixed">Mixed</button>
				"""));

		var pressedTrue = cut.FindAllByRole(AriaRole.Button, o => o.Pressed = true);
		pressedTrue.Count.ShouldBe(1);
		pressedTrue[0].TextContent.ShouldBe("Hello");

		var pressedFalse = cut.FindAllByRole(AriaRole.Button, o => o.Pressed = false);
		pressedFalse.Count.ShouldBe(3);

		var allButtons = cut.FindAllByRole(AriaRole.Button);
		allButtons.Count.ShouldBe(4);
	}

	[Fact(DisplayName = "should support expanded")]
	public void Should_support_expanded()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<div role="treeitem">Hi</div>
				<div role="treeitem" aria-expanded="true">Hello</div>
				<div role="treeitem" aria-expanded="false">Bye</div>
				"""));

		var all = cut.FindAllByRole(AriaRole.Treeitem);
		all.Count.ShouldBe(3);

		var expandedTrue = cut.FindAllByRole(AriaRole.Treeitem, o => o.Expanded = true);
		expandedTrue.Count.ShouldBe(1);
		expandedTrue[0].TextContent.ShouldBe("Hello");

		var expandedFalse = cut.FindAllByRole(AriaRole.Treeitem, o => o.Expanded = false);
		expandedFalse.Count.ShouldBe(1);
		expandedFalse[0].TextContent.ShouldBe("Bye");
	}

	[Fact(DisplayName = "should support disabled")]
	public void Should_support_disabled()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<button>Hi</button>
				<button disabled>Bye</button>
				<button aria-disabled="true">Hello</button>
				<button aria-disabled="false">Oh</button>
				<fieldset disabled>
					<button>Yay</button>
				</fieldset>
				<select>
					<optgroup disabled>
						<option>one</option>
					</optgroup>
					<optgroup>
						<option>two</option>
					</optgroup>
					<option disabled>three</option>
				</select>
				"""));

		var disabledButtons = cut.FindAllByRole(AriaRole.Button, o => o.Disabled = true);
		disabledButtons.Count.ShouldBe(3);
		disabledButtons[0].TextContent.ShouldBe("Bye");
		disabledButtons[1].TextContent.ShouldBe("Hello");
		disabledButtons[2].TextContent.ShouldBe("Yay");

		var enabledButtons = cut.FindAllByRole(AriaRole.Button, o => o.Disabled = false);
		enabledButtons.Count.ShouldBe(2);
		enabledButtons[0].TextContent.ShouldBe("Hi");
		enabledButtons[1].TextContent.ShouldBe("Oh");

		var disabledOptions = cut.FindAllByRole(AriaRole.Option, o => o.Disabled = true);
		disabledOptions.Count.ShouldBe(2);
		disabledOptions[0].TextContent.ShouldBe("one");
		disabledOptions[1].TextContent.ShouldBe("three");

		var enabledOptions = cut.FindAllByRole(AriaRole.Option, o => o.Disabled = false);
		enabledOptions.Count.ShouldBe(1);
		enabledOptions[0].TextContent.ShouldBe("two");
	}

	[Fact(DisplayName = "should support disabled fieldset")]
	public void Should_support_disabled_fieldset()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<fieldset disabled>
					<input />
					<button data-testid="inside-fieldset-element">x</button>
					<legend>
						<button data-testid="inside-legend-element">legend</button>
					</legend>
				</fieldset>

				<fieldset disabled>
					<legend>
						<div>
							<button data-testid="nested-inside-legend-element">x</button>
						</div>
					</legend>
				</fieldset>

				<fieldset disabled>
					<div></div>
					<legend>
						<button data-testid="first-legend-element">x</button>
					</legend>
					<legend>
						<button data-testid="second-legend-element">x</button>
					</legend>
				</fieldset>

				<fieldset disabled>
					<fieldset>
						<button data-testid="deep-button">x</button>
					</fieldset>
				</fieldset>
				"""));

		// Button inside legend of disabled fieldset should be enabled
		cut.FindAllByRole(AriaRole.Button, o => o.Disabled = false)
			.ShouldContain(b => b.GetAttribute("data-testid") == "inside-legend-element");

		// Button nested inside legend should also be enabled
		cut.FindAllByRole(AriaRole.Button, o => o.Disabled = false)
			.ShouldContain(b => b.GetAttribute("data-testid") == "nested-inside-legend-element");

		// First legend — button should be enabled
		cut.FindAllByRole(AriaRole.Button, o => o.Disabled = false)
			.ShouldContain(b => b.GetAttribute("data-testid") == "first-legend-element");

		// Second legend — button should be disabled
		cut.FindAllByRole(AriaRole.Button, o => o.Disabled = true)
			.ShouldContain(b => b.GetAttribute("data-testid") == "second-legend-element");

		// Deeply nested fieldset — button should be disabled
		cut.FindAllByRole(AriaRole.Button, o => o.Disabled = true)
			.ShouldContain(b => b.GetAttribute("data-testid") == "deep-button");

		// Button inside fieldset (not in legend) should be disabled
		cut.FindAllByRole(AriaRole.Button, o => o.Disabled = true)
			.ShouldContain(b => b.GetAttribute("data-testid") == "inside-fieldset-element");
	}

	[Fact(DisplayName = "should throw ArgumentNullException when renderedComponent is null")]
	public void Should_throw_when_rendered_component_is_null()
	{
		IRenderedComponent<IComponent>? nullComponent = null;

		Should.Throw<ArgumentNullException>(() => nullComponent!.FindByRole(AriaRole.Button));
		Should.Throw<ArgumentNullException>(() => nullComponent!.FindAllByRole(AriaRole.Button));
	}

	[Fact(DisplayName = "should support case-insensitive explicit roles")]
	public void Should_support_case_insensitive_explicit_roles()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<div role="BUTTON">Upper</div>
				<div role="Button">Mixed</div>
				<div role="button">Lower</div>
				"""));

		var buttons = cut.FindAllByRole(AriaRole.Button);
		buttons.Count.ShouldBe(3);
	}

	[Fact(DisplayName = "should map img with empty alt to presentation role")]
	public void Should_map_img_with_empty_alt_to_presentation()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<img alt="" />
				<img alt="A photo" />
				<img />
				"""));

		var presentations = cut.FindAllByRole(AriaRole.Presentation);
		presentations.Count.ShouldBe(1);
		presentations[0].GetAttribute("alt").ShouldBe(string.Empty);

		var images = cut.FindAllByRole(AriaRole.Img);
		images.Count.ShouldBe(2);
	}

	[Fact(DisplayName = "should map anchor with href to link and without to generic")]
	public void Should_map_anchor_with_and_without_href()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<a href="/home">Home</a>
				<a>No link</a>
				"""));

		var links = cut.FindAllByRole(AriaRole.Link);
		links.Count.ShouldBe(1);
		links[0].TextContent.ShouldBe("Home");

		var generics = cut.FindAllByRole(AriaRole.Generic);
		generics.ShouldContain(e => e.TextContent == "No link");
	}

	[Fact(DisplayName = "should map input types to correct roles")]
	public void Should_map_input_types_to_correct_roles()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<input type="text" />
				<input type="email" />
				<input type="tel" />
				<input type="url" />
				<input type="password" />
				<input />
				<input type="checkbox" />
				<input type="radio" />
				<input type="range" />
				<input type="number" />
				<input type="search" />
				<input type="button" />
				<input type="submit" />
				<input type="reset" />
				<input type="image" alt="img" />
				<input type="hidden" />
				"""));

		// text, email, tel, url, password, and default (no type) all map to Textbox
		cut.FindAllByRole(AriaRole.Textbox).Count.ShouldBe(6);

		cut.FindAllByRole(AriaRole.Checkbox).Count.ShouldBe(1);
		cut.FindAllByRole(AriaRole.Radio).Count.ShouldBe(1);
		cut.FindAllByRole(AriaRole.Slider).Count.ShouldBe(1);
		cut.FindAllByRole(AriaRole.Spinbutton).Count.ShouldBe(1);
		cut.FindAllByRole(AriaRole.Searchbox).Count.ShouldBe(1);

		// button, submit, reset, image all map to Button
		cut.FindAllByRole(AriaRole.Button).Count.ShouldBe(4);
	}

	[Fact(DisplayName = "should not map hidden input to any role")]
	public void Should_not_map_hidden_input_to_any_role()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<input type="hidden" />"""));

		cut.FindAllByRole(AriaRole.Textbox).ShouldBeEmpty();
	}

	[Fact(DisplayName = "should map form with accessible name to form role")]
	public void Should_map_form_with_accessible_name_to_form_role()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<form aria-label="Search">Search form</form>
				<form>Unlabelled form</form>
				"""));

		var forms = cut.FindAllByRole(AriaRole.Form);
		forms.Count.ShouldBe(1);
		forms[0].TextContent.ShouldBe("Search form");
	}

	[Fact(DisplayName = "should map section with accessible name to region role")]
	public void Should_map_section_with_accessible_name_to_region_role()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<section aria-label="Main content">Content</section>
				<section>Unlabelled</section>
				"""));

		var regions = cut.FindAllByRole(AriaRole.Region);
		regions.Count.ShouldBe(1);
		regions[0].TextContent.ShouldBe("Content");
	}

	[Fact(DisplayName = "should map semantic HTML elements to implicit roles")]
	public void Should_map_semantic_html_elements()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<nav>Nav</nav>
				<article>Article</article>
				<aside>Aside</aside>
				<footer>Footer</footer>
				<header>Header</header>
				<hr />
				<figure>Figure</figure>
				<main>Main</main>
				<li>ListItem</li>
				"""));

		cut.FindAllByRole(AriaRole.Navigation).Count.ShouldBe(1);
		cut.FindAllByRole(AriaRole.Article).Count.ShouldBe(1);
		cut.FindAllByRole(AriaRole.Complementary).Count.ShouldBe(1);
		cut.FindAllByRole(AriaRole.ContentInfo).Count.ShouldBe(1);
		cut.FindAllByRole(AriaRole.Banner).Count.ShouldBe(1);
		cut.FindAllByRole(AriaRole.Separator).Count.ShouldBe(1);
		cut.FindAllByRole(AriaRole.Figure).Count.ShouldBe(1);
		cut.FindAllByRole(AriaRole.Main).Count.ShouldBe(1);
		cut.FindAllByRole(AriaRole.Listitem).Count.ShouldBe(1);
	}

	[Fact(DisplayName = "should map list elements to list role")]
	public void Should_map_list_elements_to_list_role()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<ul><li>a</li></ul>
				<ol><li>b</li></ol>
				<menu><li>c</li></menu>
				"""));

		cut.FindAllByRole(AriaRole.List).Count.ShouldBe(3);
	}

	[Fact(DisplayName = "should map table elements to correct roles")]
	public void Should_map_table_elements_to_correct_roles()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<table>
					<thead><tr><th>Header</th></tr></thead>
					<tbody><tr><td>Cell</td></tr></tbody>
					<tfoot><tr><td>Footer</td></tr></tfoot>
				</table>
				"""));

		cut.FindAllByRole(AriaRole.Table).Count.ShouldBe(1);
		cut.FindAllByRole(AriaRole.Rowgroup).Count.ShouldBe(3);
		cut.FindAllByRole(AriaRole.Row).Count.ShouldBe(3);
		cut.FindAllByRole(AriaRole.ColumnHeader).Count.ShouldBe(1);
		cut.FindAllByRole(AriaRole.Cell).Count.ShouldBe(2);
	}

	[Fact(DisplayName = "should map meter, progress, textarea, and datalist")]
	public void Should_map_additional_elements()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<meter value="0.5">50%</meter>
				<progress value="0.5">50%</progress>
				<textarea></textarea>
				<datalist><option>One</option></datalist>
				"""));

		cut.FindAllByRole(AriaRole.Meter).Count.ShouldBe(1);
		cut.FindAllByRole(AriaRole.Progressbar).Count.ShouldBe(1);
		cut.FindAllByRole(AriaRole.Textbox).Count.ShouldBe(1);
	}

	[Fact(DisplayName = "should use first token of space-separated role attribute")]
	public void Should_use_first_token_of_space_separated_role()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<div role="button link">Multi-role</div>
				"""));

		var buttons = cut.FindAllByRole(AriaRole.Button);
		buttons.Count.ShouldBe(1);
		buttons[0].TextContent.ShouldBe("Multi-role");

		// Should NOT match the second token
		cut.FindAllByRole(AriaRole.Link).ShouldBeEmpty();
	}

	[Fact(DisplayName = "should support native details open as expanded")]
	public void Should_support_native_details_open_as_expanded()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""
				<details><summary>Closed</summary></details>
				<details open><summary>Open</summary></details>
				"""));

		var expanded = cut.FindAllByRole(AriaRole.Group, o => o.Expanded = true);
		expanded.Count.ShouldBe(1);

		var collapsed = cut.FindAllByRole(AriaRole.Group, o => o.Expanded = false);
		collapsed.Count.ShouldBe(1);
	}

	[Fact(DisplayName = "should include filter details in RoleNotFoundException message")]
	public void Should_include_filter_details_in_exception_message()
	{
		var cut = Render<Wrapper>(ps =>
			ps.AddChildContent("""<div>Nothing</div>"""));

		var exception = Should.Throw<RoleNotFoundException>(() =>
			cut.FindByRole(AriaRole.Checkbox, o =>
			{
				o.Name = "Accept";
				o.Checked = true;
				o.Disabled = false;
			}));

		exception.Message.ShouldContain("checkbox");
		exception.Message.ShouldContain("name: 'Accept'");
		exception.Message.ShouldContain("checked: true");
		exception.Message.ShouldContain("disabled: false");
	}

	[Fact(DisplayName = "should reflect latest value when element rerenders")]
	public void Should_reflect_latest_value_when_element_rerenders()
	{
		var cut = Render<RoleQueryCounter>();

		var input = cut.FindByRole(AriaRole.Textbox, o => o.Name = "Re-rendered input");
		input.GetAttribute("value").ShouldBe("0");

		cut.Find("#increment-button").Click();
		input.GetAttribute("value").ShouldBe("1");
	}
}
