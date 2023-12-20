using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Bunit.Web.Stubs.AddStubMethodStubGenerator;

internal static class StubComponentBuilder
{
	private const string CascadingParameterAttributeQualifier = "Microsoft.AspNetCore.Components.CascadingParameterAttribute";
	private const string ParameterAttributeQualifier = "Microsoft.AspNetCore.Components.ParameterAttribute";

	public static bool GenerateStubComponent(AddStubClassInfo classInfo, SourceProductionContext context)
	{
		var hasSomethingToStub = false;
		var targetTypeSymbol = (INamedTypeSymbol)classInfo!.TargetType;
		var sourceBuilder = new StringBuilder();

		sourceBuilder.AppendLine($"namespace {classInfo.TargetTypeNamespace};");
		sourceBuilder.AppendLine();
		sourceBuilder.AppendLine($"internal partial class {classInfo.StubClassName} : global::Microsoft.AspNetCore.Components.ComponentBase");
		sourceBuilder.Append("{");

		foreach (var member in targetTypeSymbol
			         .GetMembers()
			         .OfType<IPropertySymbol>()
			         .Where(p => p.GetAttributes()
				         .Any(attr =>
					         attr.AttributeClass?.ToDisplayString() ==
					         ParameterAttributeQualifier ||
					         attr.AttributeClass?.ToDisplayString() ==
					         CascadingParameterAttributeQualifier)))
		{
			sourceBuilder.AppendLine();

			hasSomethingToStub = true;
			var propertyType = member.Type.ToDisplayString();
			var propertyName = member.Name;

			var attributeLine = GetAttributeLine(member);
			sourceBuilder.AppendLine(attributeLine);

			sourceBuilder.AppendLine($"\tpublic {propertyType} {propertyName} {{ get; set; }}");
		}

		sourceBuilder.AppendLine("}");

		if (hasSomethingToStub)
		{
			context.AddSource($"{classInfo.StubClassName}.g.cs", sourceBuilder.ToString());
		}

		return hasSomethingToStub;

		string GetAttributeLine(ISymbol member)
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
}
