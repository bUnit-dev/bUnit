using Microsoft.CodeAnalysis;

namespace Bunit.Analyzers;

/// <summary>
/// Diagnostic descriptors for bUnit analyzers.
/// </summary>
public static class DiagnosticDescriptors
{
	private const string Category = "Usage";

	/// <summary>
	/// BUNIT0001: Razor test files should inherit from BunitContext when using local variables in component parameters.
	/// </summary>
	public static readonly DiagnosticDescriptor MissingInheritsInRazorFile = new(
		id: "BUNIT0001",
		title: "Razor test files should inherit from BunitContext",
		messageFormat: "Razor test file should inherit from BunitContext using @inherits BunitContext to avoid render handle errors",
		category: Category,
		defaultSeverity: DiagnosticSeverity.Warning,
		isEnabledByDefault: true,
		description: "When writing tests in Razor files that use variables or event callbacks from the test code, the file must inherit from BunitContext. Otherwise, you may encounter the error: The render handle is not yet assigned.",
		helpLinkUri: "https://bunit.dev/docs/analyzers/bunit0001.html");

	/// <summary>
	/// BUNIT0002: Prefer Find{T} over casting.
	/// </summary>
	public static readonly DiagnosticDescriptor PreferGenericFind = new(
		id: "BUNIT0002",
		title: "Prefer Find<T> over casting",
		messageFormat: "Use Find<{0}>(\"{1}\") instead of casting",
		category: Category,
		defaultSeverity: DiagnosticSeverity.Info,
		isEnabledByDefault: true,
		description: "When finding elements with a specific type, use Find<T>(selector) instead of casting the result of Find(selector).",
		helpLinkUri: "https://bunit.dev/docs/analyzers/bunit0002.html");
}
