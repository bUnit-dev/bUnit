namespace Bunit;

internal class LabelQueryConstants
{
	internal static readonly List<string> HtmlElementsThatCanHaveALabel = new()
	{
		"input",
		"select",
		"textarea",
		"button",
		"meter",
		"output",
		"progress",
	};
}
