namespace Bunit;

/// <summary>
/// Represents WAI-ARIA roles used to identify the type of an element.
/// </summary>
public enum AriaRole
{
	/// <summary>Represents the WAI-ARIA "alert" role.</summary>
	Alert,

	/// <summary>Represents the WAI-ARIA "alertdialog" role.</summary>
	AlertDialog,

	/// <summary>Represents the WAI-ARIA "application" role.</summary>
	Application,

	/// <summary>Represents the WAI-ARIA "article" role.</summary>
	Article,

	/// <summary>Represents the WAI-ARIA "banner" role.</summary>
	Banner,

	/// <summary>Represents the WAI-ARIA "blockquote" role.</summary>
	Blockquote,

	/// <summary>Represents the WAI-ARIA "button" role.</summary>
	Button,

	/// <summary>Represents the WAI-ARIA "caption" role.</summary>
	Caption,

	/// <summary>Represents the WAI-ARIA "cell" role.</summary>
	Cell,

	/// <summary>Represents the WAI-ARIA "checkbox" role.</summary>
	Checkbox,

	/// <summary>Represents the WAI-ARIA "code" role.</summary>
	Code,

	/// <summary>Represents the WAI-ARIA "columnheader" role.</summary>
	ColumnHeader,

	/// <summary>Represents the WAI-ARIA "combobox" role.</summary>
	Combobox,

	/// <summary>Represents the WAI-ARIA "complementary" role.</summary>
	Complementary,

	/// <summary>Represents the WAI-ARIA "contentinfo" role.</summary>
	ContentInfo,

	/// <summary>Represents the WAI-ARIA "definition" role.</summary>
	Definition,

	/// <summary>Represents the WAI-ARIA "deletion" role.</summary>
	Deletion,

	/// <summary>Represents the WAI-ARIA "dialog" role.</summary>
	Dialog,

	/// <summary>Represents the WAI-ARIA "directory" role.</summary>
	Directory,

	/// <summary>Represents the WAI-ARIA "document" role.</summary>
	Document,

	/// <summary>Represents the WAI-ARIA "emphasis" role.</summary>
	Emphasis,

	/// <summary>Represents the WAI-ARIA "feed" role.</summary>
	Feed,

	/// <summary>Represents the WAI-ARIA "figure" role.</summary>
	Figure,

	/// <summary>Represents the WAI-ARIA "form" role.</summary>
	Form,

	/// <summary>Represents the WAI-ARIA "generic" role.</summary>
	Generic,

	/// <summary>Represents the WAI-ARIA "grid" role.</summary>
	Grid,

	/// <summary>Represents the WAI-ARIA "gridcell" role.</summary>
	GridCell,

	/// <summary>Represents the WAI-ARIA "group" role.</summary>
	Group,

	/// <summary>Represents the WAI-ARIA "heading" role.</summary>
	Heading,

	/// <summary>Represents the WAI-ARIA "img" role.</summary>
	Img,

	/// <summary>Represents the WAI-ARIA "insertion" role.</summary>
	Insertion,

	/// <summary>Represents the WAI-ARIA "link" role.</summary>
	Link,

	/// <summary>Represents the WAI-ARIA "list" role.</summary>
	List,

	/// <summary>Represents the WAI-ARIA "listbox" role.</summary>
	Listbox,

	/// <summary>Represents the WAI-ARIA "listitem" role.</summary>
	Listitem,

	/// <summary>Represents the WAI-ARIA "log" role.</summary>
	Log,

	/// <summary>Represents the WAI-ARIA "main" role.</summary>
	Main,

	/// <summary>Represents the WAI-ARIA "mark" role.</summary>
	Mark,

	/// <summary>Represents the WAI-ARIA "marquee" role.</summary>
	Marquee,

	/// <summary>Represents the WAI-ARIA "math" role.</summary>
	Math,

	/// <summary>Represents the WAI-ARIA "menu" role.</summary>
	Menu,

	/// <summary>Represents the WAI-ARIA "menubar" role.</summary>
	Menubar,

	/// <summary>Represents the WAI-ARIA "menuitem" role.</summary>
	Menuitem,

	/// <summary>Represents the WAI-ARIA "menuitemcheckbox" role.</summary>
	Menuitemcheckbox,

	/// <summary>Represents the WAI-ARIA "menuitemradio" role.</summary>
	Menuitemradio,

	/// <summary>Represents the WAI-ARIA "meter" role.</summary>
	Meter,

	/// <summary>Represents the WAI-ARIA "navigation" role.</summary>
	Navigation,

	/// <summary>Represents the WAI-ARIA "none" role.</summary>
	None,

	/// <summary>Represents the WAI-ARIA "note" role.</summary>
	Note,

	/// <summary>Represents the WAI-ARIA "option" role.</summary>
	Option,

	/// <summary>Represents the WAI-ARIA "paragraph" role.</summary>
	Paragraph,

	/// <summary>Represents the WAI-ARIA "presentation" role.</summary>
	Presentation,

	/// <summary>Represents the WAI-ARIA "progressbar" role.</summary>
	Progressbar,

	/// <summary>Represents the WAI-ARIA "radio" role.</summary>
	Radio,

	/// <summary>Represents the WAI-ARIA "radiogroup" role.</summary>
	Radiogroup,

	/// <summary>Represents the WAI-ARIA "region" role.</summary>
	Region,

	/// <summary>Represents the WAI-ARIA "row" role.</summary>
	Row,

	/// <summary>Represents the WAI-ARIA "rowgroup" role.</summary>
	Rowgroup,

	/// <summary>Represents the WAI-ARIA "rowheader" role.</summary>
	Rowheader,

	/// <summary>Represents the WAI-ARIA "scrollbar" role.</summary>
	Scrollbar,

	/// <summary>Represents the WAI-ARIA "search" role.</summary>
	Search,

	/// <summary>Represents the WAI-ARIA "searchbox" role.</summary>
	Searchbox,

	/// <summary>Represents the WAI-ARIA "separator" role.</summary>
	Separator,

	/// <summary>Represents the WAI-ARIA "slider" role.</summary>
	Slider,

	/// <summary>Represents the WAI-ARIA "spinbutton" role.</summary>
	Spinbutton,

	/// <summary>Represents the WAI-ARIA "status" role.</summary>
	Status,

	/// <summary>Represents the WAI-ARIA "strong" role.</summary>
	Strong,

	/// <summary>Represents the WAI-ARIA "subscript" role.</summary>
	Subscript,

	/// <summary>Represents the WAI-ARIA "superscript" role.</summary>
	Superscript,

	/// <summary>Represents the WAI-ARIA "switch" role.</summary>
	Switch,

	/// <summary>Represents the WAI-ARIA "tab" role.</summary>
	Tab,

	/// <summary>Represents the WAI-ARIA "table" role.</summary>
	Table,

	/// <summary>Represents the WAI-ARIA "tablist" role.</summary>
	Tablist,

	/// <summary>Represents the WAI-ARIA "tabpanel" role.</summary>
	Tabpanel,

	/// <summary>Represents the WAI-ARIA "term" role.</summary>
	Term,

	/// <summary>Represents the WAI-ARIA "textbox" role.</summary>
	Textbox,

	/// <summary>Represents the WAI-ARIA "time" role.</summary>
	Time,

	/// <summary>Represents the WAI-ARIA "timer" role.</summary>
	Timer,

	/// <summary>Represents the WAI-ARIA "toolbar" role.</summary>
	Toolbar,

	/// <summary>Represents the WAI-ARIA "tooltip" role.</summary>
	Tooltip,

	/// <summary>Represents the WAI-ARIA "tree" role.</summary>
	Tree,

	/// <summary>Represents the WAI-ARIA "treegrid" role.</summary>
	Treegrid,

	/// <summary>Represents the WAI-ARIA "treeitem" role.</summary>
	Treeitem,
}

internal static class AriaRoleExtensions
{
	internal static string ToRoleString(this AriaRole role) => role switch
	{
		AriaRole.Alert => "alert",
		AriaRole.AlertDialog => "alertdialog",
		AriaRole.Application => "application",
		AriaRole.Article => "article",
		AriaRole.Banner => "banner",
		AriaRole.Blockquote => "blockquote",
		AriaRole.Button => "button",
		AriaRole.Caption => "caption",
		AriaRole.Cell => "cell",
		AriaRole.Checkbox => "checkbox",
		AriaRole.Code => "code",
		AriaRole.ColumnHeader => "columnheader",
		AriaRole.Combobox => "combobox",
		AriaRole.Complementary => "complementary",
		AriaRole.ContentInfo => "contentinfo",
		AriaRole.Definition => "definition",
		AriaRole.Deletion => "deletion",
		AriaRole.Dialog => "dialog",
		AriaRole.Directory => "directory",
		AriaRole.Document => "document",
		AriaRole.Emphasis => "emphasis",
		AriaRole.Feed => "feed",
		AriaRole.Figure => "figure",
		AriaRole.Form => "form",
		AriaRole.Generic => "generic",
		AriaRole.Grid => "grid",
		AriaRole.GridCell => "gridcell",
		AriaRole.Group => "group",
		AriaRole.Heading => "heading",
		AriaRole.Img => "img",
		AriaRole.Insertion => "insertion",
		AriaRole.Link => "link",
		AriaRole.List => "list",
		AriaRole.Listbox => "listbox",
		AriaRole.Listitem => "listitem",
		AriaRole.Log => "log",
		AriaRole.Main => "main",
		AriaRole.Mark => "mark",
		AriaRole.Marquee => "marquee",
		AriaRole.Math => "math",
		AriaRole.Menu => "menu",
		AriaRole.Menubar => "menubar",
		AriaRole.Menuitem => "menuitem",
		AriaRole.Menuitemcheckbox => "menuitemcheckbox",
		AriaRole.Menuitemradio => "menuitemradio",
		AriaRole.Meter => "meter",
		AriaRole.Navigation => "navigation",
		AriaRole.None => "none",
		AriaRole.Note => "note",
		AriaRole.Option => "option",
		AriaRole.Paragraph => "paragraph",
		AriaRole.Presentation => "presentation",
		AriaRole.Progressbar => "progressbar",
		AriaRole.Radio => "radio",
		AriaRole.Radiogroup => "radiogroup",
		AriaRole.Region => "region",
		AriaRole.Row => "row",
		AriaRole.Rowgroup => "rowgroup",
		AriaRole.Rowheader => "rowheader",
		AriaRole.Scrollbar => "scrollbar",
		AriaRole.Search => "search",
		AriaRole.Searchbox => "searchbox",
		AriaRole.Separator => "separator",
		AriaRole.Slider => "slider",
		AriaRole.Spinbutton => "spinbutton",
		AriaRole.Status => "status",
		AriaRole.Strong => "strong",
		AriaRole.Subscript => "subscript",
		AriaRole.Superscript => "superscript",
		AriaRole.Switch => "switch",
		AriaRole.Tab => "tab",
		AriaRole.Table => "table",
		AriaRole.Tablist => "tablist",
		AriaRole.Tabpanel => "tabpanel",
		AriaRole.Term => "term",
		AriaRole.Textbox => "textbox",
		AriaRole.Time => "time",
		AriaRole.Timer => "timer",
		AriaRole.Toolbar => "toolbar",
		AriaRole.Tooltip => "tooltip",
		AriaRole.Tree => "tree",
		AriaRole.Treegrid => "treegrid",
		AriaRole.Treeitem => "treeitem",
		_ => throw new ArgumentOutOfRangeException(nameof(role), role, $"Unknown AriaRole value: {role}"),
	};
}
