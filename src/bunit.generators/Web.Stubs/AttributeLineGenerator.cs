using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Bunit.Web.Stubs;

internal static class AttributeLineGenerator
{
	private const string CascadingParameterAttributeQualifier = "Microsoft.AspNetCore.Components.CascadingParameterAttribute";
	private const string ParameterAttributeQualifier = "Microsoft.AspNetCore.Components.ParameterAttribute";

	public static string GetAttributeLine(ISymbol member)
	{
		var attribute = member.GetAttributes().First(attr =>
			attr.AttributeClass?.ToDisplayString() == ParameterAttributeQualifier ||
			attr.AttributeClass?.ToDisplayString() == CascadingParameterAttributeQualifier);

		var attributeLine = new StringBuilder("\t[");
		if (attribute.AttributeClass?.ToDisplayString() == ParameterAttributeQualifier)
		{
			attributeLine.Append($"global::{ParameterAttributeQualifier}");
			var captureUnmatchedValuesArg = attribute.NamedArguments
				.FirstOrDefault(arg => arg.Key == "CaptureUnmatchedValues").Value;
			if (captureUnmatchedValuesArg.Value is bool captureUnmatchedValues)
			{
				var captureString = captureUnmatchedValues ? "true" : "false";
				attributeLine.Append($"(CaptureUnmatchedValues = {captureString})");
			}
		}
		else if (attribute.AttributeClass?.ToDisplayString() == CascadingParameterAttributeQualifier)
		{
			attributeLine.Append($"global::{CascadingParameterAttributeQualifier}");
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
