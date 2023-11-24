using Microsoft.CodeAnalysis;

namespace Bunit.Web.AngleSharp;

internal static class GeneratorConfig
{
	internal static readonly SymbolDisplayFormat SymbolFormat = new SymbolDisplayFormat(
		globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Included,
		typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
		genericsOptions:
			SymbolDisplayGenericsOptions.IncludeTypeParameters |
			SymbolDisplayGenericsOptions.IncludeTypeConstraints |
			SymbolDisplayGenericsOptions.IncludeVariance,
		memberOptions:
			SymbolDisplayMemberOptions.IncludeType |
			SymbolDisplayMemberOptions.IncludeModifiers |
			SymbolDisplayMemberOptions.IncludeAccessibility |
			SymbolDisplayMemberOptions.IncludeParameters |
			SymbolDisplayMemberOptions.IncludeConstantValue |
			SymbolDisplayMemberOptions.IncludeRef,
		kindOptions: SymbolDisplayKindOptions.IncludeMemberKeyword,
		parameterOptions:
			SymbolDisplayParameterOptions.IncludeName |
			SymbolDisplayParameterOptions.IncludeType |
			SymbolDisplayParameterOptions.IncludeParamsRefOut |
			SymbolDisplayParameterOptions.IncludeDefaultValue |
			SymbolDisplayParameterOptions.IncludeModifiers,
		localOptions: SymbolDisplayLocalOptions.IncludeType,
		miscellaneousOptions:
			SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers |
			SymbolDisplayMiscellaneousOptions.UseSpecialTypes |
			SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier |
			SymbolDisplayMiscellaneousOptions.AllowDefaultLiteral);

	internal static readonly SymbolDisplayFormat SymbolFormatDefaultValue = new SymbolDisplayFormat(
		globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Included,
		typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
		memberOptions: SymbolDisplayMemberOptions.IncludeType,
		parameterOptions: SymbolDisplayParameterOptions.IncludeType);
}
