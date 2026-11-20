namespace Bunit;

/// <summary>
/// Represents ARIA roles that can be used to query elements.
/// </summary>
/// <remarks>
/// See https://www.w3.org/TR/wai-aria-1.2/#role_definitions for the full specification.
/// </remarks>
public enum AriaRole
{
	/// <summary>
	/// A message with important, and usually time-sensitive, information.
	/// </summary>
	Alert,

	/// <summary>
	/// A type of dialog that contains an alert message.
	/// </summary>
	AlertDialog,

	/// <summary>
	/// A structure containing one or more focusable elements requiring user input.
	/// </summary>
	Application,

	/// <summary>
	/// An independent section of the document, page, or site.
	/// </summary>
	Article,

	/// <summary>
	/// A section of content that is quoted from another source.
	/// </summary>
	Blockquote,

	/// <summary>
	/// An input that allows for user-triggered actions when clicked or pressed.
	/// </summary>
	Button,

	/// <summary>
	/// Visible content that names, and may also describe, a figure, table, grid, or treegrid.
	/// </summary>
	Caption,

	/// <summary>
	/// A checkable input that has three possible values: true, false, or mixed.
	/// </summary>
	Checkbox,

	/// <summary>
	/// A cell in a tabular container.
	/// </summary>
	Cell,

	/// <summary>
	/// A section whose content represents a fragment of computer code.
	/// </summary>
	Code,

	/// <summary>
	/// A cell containing header information for a column.
	/// </summary>
	ColumnHeader,

	/// <summary>
	/// A composite widget containing a single-line textbox and another element.
	/// </summary>
	Combobox,

	/// <summary>
	/// A form of widget that performs an action but does not receive input data. (Abstract)
	/// </summary>
	Command,

	/// <summary>
	/// A supporting section of the document, designed to be complementary to the main content.
	/// </summary>
	Complementary,

	/// <summary>
	/// A large perceivable region that contains information about the parent document.
	/// </summary>
	ContentInfo,

	/// <summary>
	/// A widget that may contain navigable descendants or owned children. (Abstract)
	/// </summary>
	Composite,

	/// <summary>
	/// A definition of a term or concept.
	/// </summary>
	Definition,

	/// <summary>
	/// A deletion contains content that is marked as removed or suggested for removal.
	/// </summary>
	Deletion,

	/// <summary>
	/// A dialog is a descendant window of the primary window of a web application.
	/// </summary>
	Dialog,

	/// <summary>
	/// A structure containing one or more focusable elements.
	/// </summary>
	Document,

	/// <summary>
	/// [Deprecated in ARIA 1.2] A list of references to members of a group.
	/// </summary>
	Directory,

	/// <summary>
	/// A scrollable list of articles.
	/// </summary>
	Feed,

	/// <summary>
	/// A figure is a perceivable section of content.
	/// </summary>
	Figure,

	/// <summary>
	/// A landmark region that contains a collection of items and objects.
	/// </summary>
	Form,

	/// <summary>
	/// A set of user interface objects that are not intended to be included in a page summary.
	/// </summary>
	Generic,

	/// <summary>
	/// A grid is an interactive control that contains cells of tabular data arranged in rows and columns.
	/// </summary>
	Grid,

	/// <summary>
	/// A cell in a grid or treegrid.
	/// </summary>
	GridCell,

	/// <summary>
	/// A set of user interface objects that are not intended to be included in a page summary or table of contents.
	/// </summary>
	Group,

	/// <summary>
	/// A heading for a section of the page.
	/// </summary>
	Heading,

	/// <summary>
	/// A container for a collection of elements that form an image.
	/// </summary>
	Img,

	/// <summary>
	/// A generic type of widget that allows user input. (Abstract)
	/// </summary>
	Input,

	/// <summary>
	/// An insertion contains content that is marked as added or suggested for addition.
	/// </summary>
	Insertion,

	/// <summary>
	/// A perceivable section for a specific purpose that users can navigate to. (Abstract)
	/// </summary>
	Landmark,

	/// <summary>
	/// An interactive reference to an internal or external resource.
	/// </summary>
	Link,

	/// <summary>
	/// A section containing listitem elements.
	/// </summary>
	List,

	/// <summary>
	/// A widget that allows the user to select one or more items from a list of choices.
	/// </summary>
	Listbox,

	/// <summary>
	/// A single item in a list or directory.
	/// </summary>
	Listitem,

	/// <summary>
	/// A type of live region where new information is added in meaningful order.
	/// </summary>
	Log,

	/// <summary>
	/// The main content of a document.
	/// </summary>
	Main,

	/// <summary>
	/// A type of live region where non-essential information changes frequently.
	/// </summary>
	Marquee,

	/// <summary>
	/// Content that represents a mathematical expression.
	/// </summary>
	Math,

	/// <summary>
	/// A type of widget that offers a list of choices to the user.
	/// </summary>
	Menu,

	/// <summary>
	/// A presentation of menu that usually remains visible and is usually presented horizontally.
	/// </summary>
	Menubar,

	/// <summary>
	/// An option in a set of choices contained by a menu or menubar.
	/// </summary>
	Menuitem,

	/// <summary>
	/// A menuitem with a checkable state.
	/// </summary>
	MenuitemCheckbox,

	/// <summary>
	/// A checkable menuitem in a set of elements with the same role.
	/// </summary>
	MenuitemRadio,

	/// <summary>
	/// A graphical display of values.
	/// </summary>
	Meter,

	/// <summary>
	/// A section of a page that represents navigation links.
	/// </summary>
	Navigation,

	/// <summary>
	/// A section whose content represents additional information.
	/// </summary>
	Note,

	/// <summary>
	/// An element whose implicit native role semantics will not be mapped to the accessibility API. Synonym of Presentation.
	/// </summary>
	None,

	/// <summary>
	/// A selectable item in a select list.
	/// </summary>
	Option,

	/// <summary>
	/// A paragraph of content.
	/// </summary>
	Paragraph,

	/// <summary>
	/// An element whose implicit native role semantics will not be mapped to the accessibility API.
	/// </summary>
	Presentation,

	/// <summary>
	/// An element that displays the progress status for tasks that take a long time.
	/// </summary>
	Progressbar,

	/// <summary>
	/// A checkable input in a group of elements with the same role.
	/// </summary>
	Radio,

	/// <summary>
	/// A group of radio buttons.
	/// </summary>
	RadioGroup,

	/// <summary>
	/// An element representing a range of values. (Abstract)
	/// </summary>
	Range,

	/// <summary>
	/// A perceivable section containing content that is relevant to a specific, author-specified purpose.
	/// </summary>
	Region,

	/// <summary>
	/// The base role from which all other roles inherit. (Abstract)
	/// </summary>
	RoleType,

	/// <summary>
	/// A row of cells in a tabular container.
	/// </summary>
	Row,

	/// <summary>
	/// A group containing one or more row elements in a grid.
	/// </summary>
	RowGroup,

	/// <summary>
	/// A cell containing header information for a row in a grid.
	/// </summary>
	RowHeader,

	/// <summary>
	/// A graphical object that controls the scrolling of content within a viewing area.
	/// </summary>
	Scrollbar,

	/// <summary>
	/// A landmark region that contains a collection of items and objects that, as a whole, combine to create a search facility.
	/// </summary>
	Search,

	/// <summary>
	/// A type of textbox intended for specifying search criteria.
	/// </summary>
	Searchbox,

	/// <summary>
	/// A divider that separates and distinguishes sections of content or groups of menuitems.
	/// </summary>
	Separator,

	/// <summary>
	/// An input where the user selects a value from within a given range.
	/// </summary>
	Slider,

	/// <summary>
	/// A form of range that expects the user to select from among discrete choices.
	/// </summary>
	Spinbutton,

	/// <summary>
	/// A type of live region whose content is advisory information for the user.
	/// </summary>
	Status,

	/// <summary>
	/// A type of checkbox that represents on/off values.
	/// </summary>
	Switch,

	/// <summary>
	/// A grouping label providing a mechanism for selecting the tab content that is to be rendered to the user.
	/// </summary>
	Tab,

	/// <summary>
	/// A section containing data arranged in rows and columns.
	/// </summary>
	Table,

	/// <summary>
	/// A list of tab elements, which are references to tabpanel elements.
	/// </summary>
	Tablist,

	/// <summary>
	/// A container for the resources associated with a tab.
	/// </summary>
	Tabpanel,

	/// <summary>
	/// An element that represents a specific value within a range.
	/// </summary>
	Term,

	/// <summary>
	/// A type of input that allows free-form text as its value.
	/// </summary>
	Textbox,

	/// <summary>
	/// An element that represents a specific point in time.
	/// </summary>
	Time,

	/// <summary>
	/// A type of live region containing a numerical counter.
	/// </summary>
	Timer,

	/// <summary>
	/// A collection of commonly used function buttons or controls represented in compact visual form.
	/// </summary>
	Toolbar,

	/// <summary>
	/// A contextual popup that displays a description for an element.
	/// </summary>
	Tooltip,

	/// <summary>
	/// A widget that allows the user to select one or more items from a hierarchically organized collection.
	/// </summary>
	Tree,

	/// <summary>
	/// A grid whose rows can be expanded and collapsed in the same manner as for a tree.
	/// </summary>
	Treegrid,

	/// <summary>
	/// An item in a tree.
	/// </summary>
	Treeitem,

	/// <summary>
	/// A document structural element. (Abstract)
	/// </summary>
	Structure,

	/// <summary>
	/// One or more subscripted characters.
	/// </summary>
	Subscript,

	/// <summary>
	/// One or more superscripted characters.
	/// </summary>
	Superscript,

	/// <summary>
	/// Content that is important, serious, or urgent.
	/// </summary>
	Strong,

	/// <summary>
	/// A renderable structural containment unit in a document or application. (Abstract)
	/// </summary>
	Section,

	/// <summary>
	/// A structure that labels or summarizes the topic of its related section. (Abstract)
	/// </summary>
	Sectionhead,

	/// <summary>
	/// A form widget that allows the user to make selections from a set of choices. (Abstract)
	/// </summary>
	Select,

	/// <summary>
	/// An interactive component of a graphical user interface (GUI). (Abstract)
	/// </summary>
	Widget,

	/// <summary>
	/// A browser or application window.
	/// </summary>
	Window,

	/// <summary>
	/// A landmark that contains mostly site-oriented content, rather than page-specific content.
	/// </summary>
	Banner,
}
