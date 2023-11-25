using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Bunit.Web.Stubs;

/// <summary>
/// Generator that creates a stub that mimics the public surface of a Component.
/// </summary>
[Generator]
public class StubGenerator : IIncrementalGenerator
{
	private const string AttributeFullQualifiedName = "Bunit.StubAttribute";

	/// <inheritdoc/>
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
			"StubAttribute.g.cs",
			SourceText.From(StubAttributeGenerator.StubAttribute, Encoding.UTF8)));

		var classesToStub = context.SyntaxProvider
			.ForAttributeWithMetadataName(
				AttributeFullQualifiedName,
				predicate: static (s, _) => s is ClassDeclarationSyntax,
				transform: static (ctx, _) => GetStubClassInfo(ctx))
			.Where(static m => m is not null);


		context.RegisterSourceOutput(
			classesToStub,
			static (spc, source) => Execute(source, spc));
	}

	private static StubClassInfo GetStubClassInfo(GeneratorAttributeSyntaxContext context)
	{
		foreach (var attribute in context.TargetSymbol.GetAttributes())
		{
			if (context.TargetSymbol is not ITypeSymbol stubbedType ||
			    !ImplementsInterface(stubbedType, "Microsoft.AspNetCore.Components.IComponent"))
			{
				continue;
			}

			var namespaceName = stubbedType.ContainingNamespace.ToDisplayString();
			var className = context.TargetSymbol.Name;

			// TODO: Check for the name not the first
			var originalTypeToStub = attribute.ConstructorArguments.FirstOrDefault().Value;
			if (originalTypeToStub is not ITypeSymbol originalType)
			{
				continue;
			}

			return new StubClassInfo { ClassName = className, Namespace = namespaceName, TargetType = originalType };
		}

		return null;

		static bool ImplementsInterface(ITypeSymbol typeSymbol, string interfaceName)
		{
			return typeSymbol.AllInterfaces.Any(i => i.ToDisplayString() == interfaceName);
		}
	}

	private static void Execute(StubClassInfo classInfo, SourceProductionContext context)
	{
		var hasSomethingToStub = false;
		var targetTypeSymbol = (INamedTypeSymbol)classInfo!.TargetType;
		var sourceBuilder = new StringBuilder();

		// TODO: Shall we dictate file-scoped namespaces here?
		sourceBuilder.AppendLine($"namespace {classInfo.Namespace};");

		// TODO: If the class is a nested one, that approach does not work
		sourceBuilder.AppendLine($"public partial class {classInfo.ClassName}");
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
			context.AddSource($"{classInfo.ClassName}Stub.g.cs", sourceBuilder.ToString());
		}
	}
}

internal sealed class StubClassInfo
{
	public string ClassName { get; set; }
	public string Namespace { get; set; }
	public ITypeSymbol TargetType { get; set; }
}
