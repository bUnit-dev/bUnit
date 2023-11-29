using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bunit.Web.Stubs;

/// <summary>
/// Generator that creates a stub that mimics the public surface of a Component.
/// </summary>
[Generator]
public class StubGenerator : IIncrementalGenerator
{
	private const string CascadingParameterAttributeQualifier = "Microsoft.AspNetCore.Components.CascadingParameterAttribute";
	private const string ParameterAttributeQualifier = "Microsoft.AspNetCore.Components.ParameterAttribute";

	/// <inheritdoc/>
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		var classesToStub = context.SyntaxProvider
			.CreateSyntaxProvider(
				predicate: static (s, _) => s is InvocationExpressionSyntax,
				transform: static (ctx, _) => GetStubClassInfo(ctx))
			.Where(static m => m is not null)
			.Collect();

		context.RegisterSourceOutput(
			classesToStub,
			static (spc, source) => Execute(source, spc));
	}

	private static StubClassInfo GetStubClassInfo(GeneratorSyntaxContext context)
	{
		var invocation = context.Node as InvocationExpressionSyntax;
		if (!IsComponentFactoryStubMethod(invocation, context.SemanticModel))
		{
			return null;
		}

		if (invocation?.Expression is MemberAccessExpressionSyntax { Name: GenericNameSyntax { TypeArgumentList.Arguments.Count: 1 } genericName })
		{
			var typeArgument = genericName.TypeArgumentList.Arguments[0];
			if (context.SemanticModel.GetSymbolInfo(typeArgument).Symbol is ITypeSymbol symbol)
			{
				var path = GetInterceptorFilePath(context.Node.SyntaxTree, context.SemanticModel.Compilation);
				var lineSpan = context.SemanticModel.SyntaxTree.GetLineSpan(context.Node.Span);
				var line = lineSpan.StartLinePosition.Line + 1;
				var column = lineSpan.Span.Start.Character + context.Node.ToString().IndexOf("AddStub", StringComparison.Ordinal) + 1;

				return new StubClassInfo
				{
					StubClassName = $"{symbol.Name}Stub",
					TargetTypeNamespace = symbol.ContainingNamespace.ToDisplayString(),
					TargetType = symbol,
					Path = path,
					Line = line,
					Column = column,
				};
			}
		}

		return null;

		static bool IsComponentFactoryStubMethod(InvocationExpressionSyntax invocation, SemanticModel semanticModel)
		{
			if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
			{
				return false;
			}

			if (memberAccess.Name.Identifier.Text != "AddStub" ||
			    invocation.ArgumentList.Arguments.Count != 0)
			{
				return false;
			}

			return semanticModel.GetSymbolInfo(invocation).Symbol is IMethodSymbol { IsExtensionMethod: true, ReceiverType.Name: "ComponentFactoryCollection" };
		}

		static string GetInterceptorFilePath(SyntaxTree tree, Compilation compilation)
		{
			return compilation.Options.SourceReferenceResolver?.NormalizePath(tree.FilePath, baseFilePath: null) ?? tree.FilePath;
		}
	}

	private static void Execute(ImmutableArray<StubClassInfo> classInfos, SourceProductionContext context)
	{
		foreach (var stubClassGrouped in classInfos.GroupBy(c => c.UniqueQualifier))
		{
			var stubbedComponentGroup = stubClassGrouped.First();
			var didStubComponent = GenerateStubComponent(stubbedComponentGroup, context);
			if (didStubComponent is false)
			{
				return;
			}

			GenerateInterceptorCode(stubbedComponentGroup, stubClassGrouped, context);
		}
	}

	private static bool GenerateStubComponent(StubClassInfo classInfo, SourceProductionContext context)
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

	private static void GenerateInterceptorCode(StubClassInfo stubbedComponentGroup, IEnumerable<StubClassInfo> stubClassGrouped, SourceProductionContext context)
	{
		// Generate the attribute
		const string attribute = @"namespace System.Runtime.CompilerServices
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	sealed file class InterceptsLocationAttribute : Attribute
	{
		public InterceptsLocationAttribute(string filePath, int line, int column)
		{
			_ = filePath;
			_ = line;
			_ = column;
		}
	}
}";

		// Generate the interceptor
		var interceptorSource = new StringBuilder();
		interceptorSource.AppendLine(attribute);
		interceptorSource.AppendLine();
		interceptorSource.AppendLine("namespace Bunit");
		interceptorSource.AppendLine("{");
		interceptorSource.AppendLine($"\tinternal static class Interceptor{stubbedComponentGroup.StubClassName}");
		interceptorSource.AppendLine("\t{");

		foreach (var hit in stubClassGrouped)
		{
			interceptorSource.AppendLine(
				$"\t\t[global::System.Runtime.CompilerServices.InterceptsLocationAttribute(\"{hit.Path}\", {hit.Line}, {hit.Column})]");
		}

		interceptorSource.AppendLine(
			"\t\tpublic static global::Bunit.ComponentFactoryCollection AddStubInterceptor<TComponent>(this global::Bunit.ComponentFactoryCollection factories)");
		interceptorSource.AppendLine("\t\t\twhere TComponent : global::Microsoft.AspNetCore.Components.IComponent");
		interceptorSource.AppendLine("\t\t{");
		interceptorSource.AppendLine(
			$"\t\t\treturn factories.Add<global::{stubbedComponentGroup.TargetType.ToDisplayString()}, global::{stubbedComponentGroup.TargetTypeNamespace}.{stubbedComponentGroup.StubClassName}>();");
		interceptorSource.AppendLine("\t\t}");
		interceptorSource.AppendLine("\t}");
		interceptorSource.AppendLine("}");

		context.AddSource($"Interceptor{stubbedComponentGroup.StubClassName}.g.cs", interceptorSource.ToString());
	}
}

internal sealed class StubClassInfo
{
	public string StubClassName { get; set; }
	public string TargetTypeNamespace { get; set; }
	public string UniqueQualifier => $"{TargetTypeNamespace}.{StubClassName}";
	public ITypeSymbol TargetType { get; set; }
	public string Path { get; set; }
	public int Line { get; set; }
	public int Column { get; set; }
}
