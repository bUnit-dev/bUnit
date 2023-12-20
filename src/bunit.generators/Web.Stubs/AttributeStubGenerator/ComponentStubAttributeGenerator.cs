using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Bunit.Web.Stubs.AttributeStubGenerator;

/// <summary>
/// Generator that creates a stub of a marked component.
/// </summary>
[Generator]
public class ComponentStubAttributeGenerator : IIncrementalGenerator
{
	private const string AttributeFullQualifiedName = "Bunit.ComponentStubAttribute`1";

	/// <inheritdoc />
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
			"ComponentStubAttribute.g.cs",
			SourceText.From(ComponentStubAttribute.ComponentStubAttributeSource, Encoding.UTF8)));

		var classesToStub = context.SyntaxProvider
			.ForAttributeWithMetadataName(
				AttributeFullQualifiedName,
				predicate: static (s, _) => IsClassWithComponentStubAttribute(s),
				transform: static (ctx, _) => GetStubClassInfo(ctx))
			.Where(static m => m is not null);

		context.RegisterSourceOutput(
			classesToStub,
			static (spc, source) => Execute(source, spc));
	}

	private static bool IsClassWithComponentStubAttribute(SyntaxNode s) =>
		s is ClassDeclarationSyntax c && c.AttributeLists.SelectMany(a => a.Attributes)
			.Any(at => at.Name.ToString().Contains("ComponentStub"));

	private static StubClassInfo GetStubClassInfo(GeneratorAttributeSyntaxContext context)
	{
#pragma warning disable RS1035
		foreach (var attribute in context.TargetSymbol.GetAttributes())
		{
			if (context.TargetSymbol is not ITypeSymbol stubbedType)
			{
				continue;
			}

			var namespaceName = stubbedType.ContainingNamespace.ToDisplayString();
			var className = context.TargetSymbol.Name;
			var visibility = context.TargetSymbol.DeclaredAccessibility.ToString().ToLower();

			var originalTypeToStub = attribute.AttributeClass?.TypeArguments.FirstOrDefault();
			if (originalTypeToStub is null)
			{
				continue;
			}

			return new StubClassInfo
			{
				ClassName = className,
				Namespace = namespaceName,
				TargetType = originalTypeToStub,
				Visibility = visibility
			};
		}

		return null;
	}

	private static void Execute(StubClassInfo classInfo, SourceProductionContext context)
	{
		var hasSomethingToStub = false;
		var targetTypeSymbol = (INamedTypeSymbol)classInfo!.TargetType;
		var sourceBuilder = new StringBuilder();

		sourceBuilder.AppendLine($"namespace {classInfo.Namespace};");

		sourceBuilder.AppendLine(
			$"{classInfo.Visibility} partial class {classInfo.ClassName} : global::Microsoft.AspNetCore.Components.ComponentBase");
		sourceBuilder.Append("{");

		foreach (var member in targetTypeSymbol
			         .GetMembers()
			         .OfType<IPropertySymbol>()
			         .Where(p => p.GetAttributes()
				         .Any(attr =>
					         attr.AttributeClass?.ToDisplayString() ==
					         "Microsoft.AspNetCore.Components.ParameterAttribute" ||
					         attr.AttributeClass?.ToDisplayString() ==
					         "Microsoft.AspNetCore.Components.CascadingParameterAttribute")))
		{
			sourceBuilder.AppendLine();

			hasSomethingToStub = true;
			var propertyType = member.Type.ToDisplayString();
			var propertyName = member.Name;

			var isParameterAttribute = member.GetAttributes().Any(attr =>
				attr.AttributeClass?.ToDisplayString() == "Microsoft.AspNetCore.Components.ParameterAttribute");
			var attributeLine = isParameterAttribute
				? "\t[global::Microsoft.AspNetCore.Components.Parameter]"
				: "\t[global::Microsoft.AspNetCore.Components.CascadingParameter]";

			sourceBuilder.AppendLine(attributeLine);
			sourceBuilder.AppendLine($"\tpublic {propertyType} {propertyName} {{ get; set; }}");
		}

		sourceBuilder.AppendLine("}");

		if (hasSomethingToStub)
		{
			context.AddSource($"{classInfo.ClassName}.g.cs", sourceBuilder.ToString());
		}
	}
}

internal sealed class StubClassInfo
{
	public string ClassName { get; set; }
	public string Namespace { get; set; }
	public ITypeSymbol TargetType { get; set; }
	public string Visibility { get; set; }
}
