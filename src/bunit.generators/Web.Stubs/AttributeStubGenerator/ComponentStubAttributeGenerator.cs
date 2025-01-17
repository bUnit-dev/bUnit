using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
			static (spc, source) => Execute(source!, spc));
	}

	private static bool IsClassWithComponentStubAttribute(SyntaxNode s) =>
		s is ClassDeclarationSyntax c && c.AttributeLists.SelectMany(a => a.Attributes)
			.Any(at => at.Name.ToString().Contains("ComponentStub"));

	[SuppressMessage("Bug", "CA1308: Normalize strings to uppercase", Justification = "On purpose")]
	private static StubClassInfo? GetStubClassInfo(GeneratorAttributeSyntaxContext context)
	{
		foreach (var attribute in context.TargetSymbol.GetAttributes())
		{
			if (context.TargetSymbol is not ITypeSymbol stubbedType)
			{
				continue;
			}

			var namespaceName = stubbedType.ContainingNamespace.ToDisplayString();
			var className = context.TargetSymbol.Name;
			var visibility = context.TargetSymbol.DeclaredAccessibility.ToString().ToLowerInvariant();
			var isPartial = ((ClassDeclarationSyntax)context.TargetNode).Modifiers.Any(SyntaxKind.PartialKeyword);

			var originalTypeToStub = attribute.AttributeClass?.TypeArguments.FirstOrDefault();
			if (originalTypeToStub is null)
			{
				continue;
			}

			var parameter = attribute.AttributeClass!.TypeArguments
				.SelectMany(s => s.GetAllMembersRecursively())
				.OfType<IPropertySymbol>()
				.Where(IsParameterOrCascadingParameter)
				.Select(CreateFromProperty)
				.ToImmutableArray();

			return new StubClassInfo
			{
				ClassName = className,
				Namespace = namespaceName,
				Visibility = visibility,
				IsNestedClass = context.TargetSymbol.ContainingType is not null,
				IsPartial = isPartial,
				Properties = parameter,
			};
		}

		return null;

		static bool IsParameterOrCascadingParameter(ISymbol member)
		{
			return member.GetAttributes().Any(SupportedAttributes.IsSupportedAttribute);
		}

		static StubPropertyInfo CreateFromProperty(IPropertySymbol member)
		{
			return new StubPropertyInfo
			{
				Name = member.Name,
				Type = member.Type.ToDisplayString(),
				AttributeLine = AttributeLineGenerator.GetAttributeLine(member),
			};
		}
	}

	private static void Execute(StubClassInfo classInfo, SourceProductionContext context)
	{
		if (CheckDiagnostics(classInfo, context))
		{
			return;
		}

		var hasSomethingToStub = false;
		var sourceBuilder = new StringBuilder(1000);

		sourceBuilder.AppendLine(HeaderProvider.Header);
		sourceBuilder.AppendLine($"namespace {classInfo.Namespace};");

		sourceBuilder.AppendLine(
			$"{classInfo.Visibility} partial class {classInfo.ClassName} : global::Microsoft.AspNetCore.Components.ComponentBase");
		sourceBuilder.Append('{');

		foreach (var member in classInfo.Properties)
		{
			sourceBuilder.AppendLine();

			hasSomethingToStub = true;
			var propertyType = member.Type;
			var propertyName = member.Name;

			sourceBuilder.AppendLine(member.AttributeLine);
			sourceBuilder.AppendLine($"\tpublic {propertyType} {propertyName} {{ get; set; }} = default!;");
		}

		sourceBuilder.AppendLine("}");

		if (hasSomethingToStub)
		{
			context.AddSource($"{classInfo.ClassName}.g.cs", sourceBuilder.ToString());
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
				classInfo.ClassName));
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
				classInfo.ClassName));
			return true;
		}

		return false;
	}
}

internal sealed record StubClassInfo
{
	public required string ClassName { get; set; }
	public required string Namespace { get; set; }
	public ImmutableArray<StubPropertyInfo> Properties { get; set; } = ImmutableArray<StubPropertyInfo>.Empty;
	public required string Visibility { get; set; }
	public bool IsNestedClass { get; set; }
	public bool IsPartial { get; set; }
}

internal sealed record StubPropertyInfo
{
	public required string Name { get; set; }
	public required string Type { get; set; }
	public required string AttributeLine { get; set; }
}

