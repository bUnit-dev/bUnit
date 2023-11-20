using Microsoft.CodeAnalysis;

namespace Bunit.Web.AngleSharp;

internal static class GeneratorConfig
{
	internal static readonly SymbolDisplayFormat SymbolFormat = new SymbolDisplayFormat(
		globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Included,
		typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
		genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
		memberOptions:
			SymbolDisplayMemberOptions.IncludeParameters |
			SymbolDisplayMemberOptions.IncludeType |
			SymbolDisplayMemberOptions.IncludeRef |
			SymbolDisplayMemberOptions.IncludeContainingType,
		kindOptions:
			SymbolDisplayKindOptions.IncludeMemberKeyword,
		parameterOptions:
			SymbolDisplayParameterOptions.IncludeName |
			SymbolDisplayParameterOptions.IncludeType |
			SymbolDisplayParameterOptions.IncludeParamsRefOut |
			SymbolDisplayParameterOptions.IncludeDefaultValue,
		localOptions: SymbolDisplayLocalOptions.IncludeType,
		miscellaneousOptions:
			SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers |
			SymbolDisplayMiscellaneousOptions.UseSpecialTypes |
			SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier);
}
