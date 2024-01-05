using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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
		foreach (var attribute in context.TargetSymbol.GetAttributes())
		{
			if (context.TargetSymbol is not ITypeSymbol stubbedType)
			{
				continue;
			}

			var namespaceName = stubbedType.ContainingNamespace.ToDisplayString();
			var className = context.TargetSymbol.Name;
			var visibility = context.TargetSymbol.DeclaredAccessibility.ToString().ToLower();
			var isPartial = ((ClassDeclarationSyntax)context.TargetNode).Modifiers.Any(SyntaxKind.PartialKeyword);

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
				Visibility = visibility,
				IsNestedClass = context.TargetSymbol.ContainingType is not null,
				IsPartial = isPartial,
			};
		}

		return null;
	}

	private static void Execute(StubClassInfo classInfo, SourceProductionContext context)
	{
		if (CheckDiagnostics(classInfo, context))
		{
			return;
		}

		var hasSomethingToStub = false;
		var targetTypeSymbol = (INamedTypeSymbol)classInfo!.TargetType;
		var sourceBuilder = new StringBuilder(1000);

		sourceBuilder.AppendLine(HeaderProvider.Header);
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

			var attributeLine = GetAttributeLineForMember(member);

			sourceBuilder.AppendLine(attributeLine);
			sourceBuilder.AppendLine($"\tpublic {propertyType} {propertyName} {{ get; set; }} = default!;");
		}

		sourceBuilder.AppendLine("}");

		if (hasSomethingToStub)
		{
			context.AddSource($"{classInfo.ClassName}.g.cs", sourceBuilder.ToString());
		}

		static string GetAttributeLineForMember(ISymbol member)
		{
			var isParameterAttribute = member.GetAttributes().Any(attr =>
				attr.AttributeClass?.ToDisplayString() == "Microsoft.AspNetCore.Components.ParameterAttribute");
			var attributeLine = isParameterAttribute
				? "\t[global::Microsoft.AspNetCore.Components.Parameter]"
				: "\t[global::Microsoft.AspNetCore.Components.CascadingParameter]";
			return attributeLine;
		}
	}

	private static bool CheckDiagnostics(StubClassInfo classInfo, SourceProductionContext context)
	{
		const string helpUrl = "https://bunit.dev/docs/extensions/bunit-generators.html";

		if (classInfo.IsNestedClass)
		{
			context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(
					"BUNIT0001",
					"Stubbing nested classes is not supported",
					"Stubbing nested classes ({0}) is not supported.",
					"Bunit", DiagnosticSeverity.Warning,
					isEnabledByDefault: true,
					helpLinkUri: helpUrl),
				Location.None,
				classInfo.TargetType.ToDisplayString()));
			return true;
		}

		if (!classInfo.IsPartial)
		{
			context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(
					"BUNIT0002",
					"Stubbing non-partial classes is not supported",
					"Class ({0}) is not partial.",
					"Bunit", DiagnosticSeverity.Warning,
					isEnabledByDefault: true,
					helpLinkUri: helpUrl),
				Location.None,
				classInfo.TargetType.ToDisplayString()));
			return true;
		}

		return false;
	}
}

internal sealed class StubClassInfo
{
	public string ClassName { get; set; }
	public string Namespace { get; set; }
	public ITypeSymbol TargetType { get; set; }
	public string Visibility { get; set; }
	public bool IsNestedClass { get; set; }
	public bool IsPartial { get; set; }
}
