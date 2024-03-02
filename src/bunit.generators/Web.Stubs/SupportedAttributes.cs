using Microsoft.CodeAnalysis;

namespace Bunit.Web.Stubs;

internal static class SupportedAttributes
{
	public const string CascadingParameterAttributeQualifier = "Microsoft.AspNetCore.Components.CascadingParameterAttribute";
	public const string ParameterAttributeQualifier = "Microsoft.AspNetCore.Components.ParameterAttribute";
	public const string SupplyParameterFromQueryAttributeQualifier = "Microsoft.AspNetCore.Components.SupplyParameterFromQueryAttribute";
	public const string SupplyParameterFromFormAttributeQualifier = "Microsoft.AspNetCore.Components.SupplyParameterFromFormAttribute";

	public static bool IsSupportedAttribute(AttributeData attribute)
	{
		var displayString = attribute.AttributeClass?.ToDisplayString();
		return displayString is ParameterAttributeQualifier or CascadingParameterAttributeQualifier or SupplyParameterFromQueryAttributeQualifier or SupplyParameterFromFormAttributeQualifier;
	}
}
