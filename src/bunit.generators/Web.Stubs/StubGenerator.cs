#nullable enable
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bunit.Web.Stubs;

[Generator]
public class StubGenerator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		var classDeclarations = context.SyntaxProvider
			.CreateSyntaxProvider(
				predicate: static (s, _) => s is ClassDeclarationSyntax { AttributeLists.Count: > 0 },
				transform: static (ctx, _) => GetStubClassInfo(ctx))
			.Where(static m => m is not null);

		var compilationAndClasses = context.CompilationProvider.Combine(classDeclarations.Collect());

		context.RegisterSourceOutput(compilationAndClasses, static (spc, source) => Execute(source.Item2, spc));
	}

	private static StubClassInfo? GetStubClassInfo(GeneratorSyntaxContext context)
	{
		var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

		// Check if the class is partial
		if (!classDeclarationSyntax.Modifiers.Any(SyntaxKind.PartialKeyword))
		{
			return null;
		}

		// Find the StubAttribute on the class
		foreach (var attribute in classDeclarationSyntax.AttributeLists.SelectMany(a => a.Attributes))
		{
			var attributeSymbol =
				ModelExtensions.GetSymbolInfo(context.SemanticModel, attribute).Symbol as IMethodSymbol;
			if (attributeSymbol is null || !IsStubAttribute(attributeSymbol))
			{
				continue;
			}

			if (attribute.ArgumentList?.Arguments is not [{ Expression: TypeOfExpressionSyntax typeOfExpression }])
			{
				continue;
			}

			var typeSymbol = ModelExtensions.GetTypeInfo(context.SemanticModel, typeOfExpression.Type).Type;
			if (typeSymbol == null)
			{
				continue;
			}

			var namespaceSyntax = classDeclarationSyntax.Parent as NamespaceDeclarationSyntax;
			var namespaceName = namespaceSyntax?.Name.ToString();
			var className = classDeclarationSyntax.Identifier.ValueText;

			return new StubClassInfo { ClassName = className, Namespace = namespaceName, TargetType = typeSymbol };
		}

		return null;
	}

	private static bool IsStubAttribute(ISymbol attributeSymbol) => attributeSymbol.ContainingType.ToDisplayString() == "Bunit.StubAttribute";

	private static void Execute(ImmutableArray<StubClassInfo?> classes, SourceProductionContext context)
	{
		if (classes.IsDefaultOrEmpty)
		{
			return;
		}

		foreach (var classInfo in classes.Where(t => t?.TargetType is INamedTypeSymbol))
		{
			var targetTypeSymbol = (INamedTypeSymbol)classInfo!.TargetType;
			var sourceBuilder = new StringBuilder();

			sourceBuilder.AppendLine($"namespace {classInfo.Namespace};");
			// TODO: Use same modifier (not public) as the original class
			sourceBuilder.AppendLine($"public partial class {classInfo.ClassName}");
			sourceBuilder.AppendLine("{");

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
				var existingProperty = targetTypeSymbol?.GetMembers().OfType<IPropertySymbol>()
					.FirstOrDefault(p => p.Name == member.Name &&
					                     p.GetAttributes().Any(attr =>
						                     attr.AttributeClass?.ToDisplayString() == "Microsoft.AspNetCore.Components.ParameterAttribute" ||
						                     attr.AttributeClass?.ToDisplayString() == "Microsoft.AspNetCore.Components.CascadingParameterAttribute"));
				if (existingProperty is not null)
				{
					continue;
				}

				var propertyType = $"global::{member.Type.ToDisplayString()}";
				var propertyName = member.Name;

				var isParameterAttribute = member.GetAttributes().Any(attr =>
					attr.AttributeClass?.ToDisplayString() == "Microsoft.AspNetCore.Components.ParameterAttribute");
				var attributeLine = isParameterAttribute
					? "\t[global::Microsoft.AspNetCore.Components.Parameter]"
					: "\t[global::Microsoft.AspNetCore.Components.CascadingParameter]";

				sourceBuilder.AppendLine(attributeLine);
				sourceBuilder.AppendLine($"\tpublic {propertyType} {propertyName} {{ get; set; }}");
				sourceBuilder.AppendLine();
			}

			sourceBuilder.AppendLine("}");
			context.AddSource($"{classInfo.ClassName}Stub.g.cs", sourceBuilder.ToString());
		}
	}
}

internal sealed record StubClassInfo
{
	public required string ClassName { get; init; }
	public required string? Namespace { get; init; }
	public required ITypeSymbol TargetType { get; init; }
}
