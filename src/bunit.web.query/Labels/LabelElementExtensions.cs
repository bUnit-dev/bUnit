using AngleSharp.Dom;

namespace Bunit;

internal static class LabelElementExtensions
{
	internal static bool IsHtmlElementThatCanHaveALabel(this IElement element) => element.NodeName switch
	{
		"INPUT" => true,
		"SELECT" => true,
		"TEXTAREA" => true,
		"BUTTON" => true,
		"METER" => true,
		"OUTPUT" => true,
		"PROGRESS" => true,
		_ => false
	};
}
