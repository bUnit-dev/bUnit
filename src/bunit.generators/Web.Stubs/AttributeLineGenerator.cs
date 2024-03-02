using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Bunit.Web.Stubs;

internal static class AttributeLineGenerator
{
	public static string GetAttributeLine(ISymbol member)
	{
		var attribute = member.GetAttributes().First(SupportedAttributes.IsSupportedAttribute);

		var attributeLine = new StringBuilder("\t[");
		if (attribute.AttributeClass?.ToDisplayString() == SupportedAttributes.ParameterAttributeQualifier)
		{
			attributeLine.Append($"global::{SupportedAttributes.ParameterAttributeQualifier}");
			var captureUnmatchedValuesArg = attribute.NamedArguments
				.FirstOrDefault(arg => arg.Key == "CaptureUnmatchedValues").Value;
			if (captureUnmatchedValuesArg.Value is bool captureUnmatchedValues)
			{
				var captureString = captureUnmatchedValues ? "true" : "false";
				attributeLine.Append($"(CaptureUnmatchedValues = {captureString})");
			}
		}
		else if (attribute.AttributeClass?.ToDisplayString() == SupportedAttributes.CascadingParameterAttributeQualifier)
		{
			attributeLine.Append($"global::{SupportedAttributes.CascadingParameterAttributeQualifier}");
			var nameArg = attribute.NamedArguments.FirstOrDefault(arg => arg.Key == "Name").Value;
			if (!nameArg.IsNull)
			{
				attributeLine.Append($"(Name = \"{nameArg.Value}\")");
			}
		}
		else if (attribute.AttributeClass?.ToDisplayString() == SupportedAttributes.SupplyParameterFromQueryAttributeQualifier)
		{
			attributeLine.Append($"global::{SupportedAttributes.SupplyParameterFromQueryAttributeQualifier}");
			var nameArg = attribute.NamedArguments.FirstOrDefault(arg => arg.Key == "Name").Value;
			if (!nameArg.IsNull)
			{
				attributeLine.Append($"(Name = \"{nameArg.Value}\")");
			}
		}
		else if (attribute.AttributeClass?.ToDisplayString() == SupportedAttributes.SupplyParameterFromFormAttributeQualifier)
		{
			attributeLine.Append($"global::{SupportedAttributes.SupplyParameterFromFormAttributeQualifier}");
			var nameArg = attribute.NamedArguments.FirstOrDefault(arg => arg.Key == "Name").Value;
			if (!nameArg.IsNull)
			{
				attributeLine.Append($"(Name = \"{nameArg.Value}\")");
			}
			var formNameArg = attribute.NamedArguments.FirstOrDefault(arg => arg.Key == "FormName").Value;
			if (!formNameArg.IsNull)
			{
				attributeLine.Append($", (FormName = \"{formNameArg.Value}\")");
			}
		}
		else if (attribute.AttributeClass?.ToDisplayString() == SupportedAttributes.SupplyParameterFromQueryAttributeQualifier)
		{
			attributeLine.Append($"global::{SupportedAttributes.SupplyParameterFromQueryAttributeQualifier}");
			var nameArg = attribute.NamedArguments.FirstOrDefault(arg => arg.Key == "Name").Value;
			if (!nameArg.IsNull)
			{
				attributeLine.Append($"(Name = \"{nameArg.Value}\")");
			}
		}

		attributeLine.Append("]");
		return attributeLine.ToString();
	}
}
