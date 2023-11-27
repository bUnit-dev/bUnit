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
	/// <inheritdoc/>
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		context.RegisterPostInitializationOutput(
			ctx => ctx.AddSource("AddGeneratedStub.g.cs",
				@"namespace Bunit
{
    public static class ComponentFactoriesExtensions
	{
        /// <summary>
		/// Marks a component as a stub component so that a stub gets generated for it. The stub has the same name as the component, but with the suffix ""Stub"" added.
		/// </summary>
		/// <typeparam name=""TComponent"">The type of component to generate a stub for.</typeparam>
		/// <remarks>
		/// When <c>ComponentFactories.AddGeneratedStub&lt;MyButton&gt;()</c> is called, a stub component is generated for the component
		/// with the name <c>MyButtonStub</c>. The stub component is added to the <see cref=""ComponentFactoryCollection""/> and can be used.
		/// It can also be retrieved via `cut.FindComponent&lt;MyButtonStub&gt;()`.
		/// This call does the same as <c>ComponentFactories.Add&lt;MyButton, MyButtonStub&gt;()</c>.
		/// </remarks>
		public static ComponentFactoryCollection AddGeneratedStub<TComponent>(this ComponentFactoryCollection factories)
			where TComponent : Microsoft.AspNetCore.Components.IComponent
		{
			return factories.AddGeneratedStubInterceptor<TComponent>();
		}
	}
}"));
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
				var column = lineSpan.Span.Start.Character + context.Node.ToString().IndexOf("AddGeneratedStub", StringComparison.Ordinal);

				return new StubClassInfo
				{
					StubClassName = $"{symbol.Name}Stub",
					TargetTypeNamespace = symbol.ContainingNamespace.ToDisplayString(),
					TargetType = symbol,
					Path = path,
					Line = line,
					Column = column + 1,
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

			if (memberAccess.Name.Identifier.Text != "AddGeneratedStub" ||
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
		sourceBuilder.AppendLine($"internal partial class {classInfo.StubClassName} : Microsoft.AspNetCore.Components.ComponentBase");
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
				attr.AttributeClass?.ToDisplayString() == "Microsoft.AspNetCore.Components.ParameterAttribute" ||
				attr.AttributeClass?.ToDisplayString() == "Microsoft.AspNetCore.Components.CascadingParameterAttribute");

			var attributeLine = new StringBuilder("\t[");
			if (attribute.AttributeClass?.ToDisplayString() == "Microsoft.AspNetCore.Components.ParameterAttribute")
			{
				attributeLine.Append("global::Microsoft.AspNetCore.Components.Parameter");
				var captureUnmatchedValuesArg = attribute.NamedArguments
					.FirstOrDefault(arg => arg.Key == "CaptureUnmatchedValues").Value;
				if (captureUnmatchedValuesArg.Value is bool captureUnmatchedValues)
				{
					var captureString = captureUnmatchedValues ? "true" : "false";
					attributeLine.Append($"(CaptureUnmatchedValues = {captureString})");
				}
			}
			else if (attribute.AttributeClass?.ToDisplayString() ==
			         "Microsoft.AspNetCore.Components.CascadingParameterAttribute")
			{
				attributeLine.Append("global::Microsoft.AspNetCore.Components.CascadingParameter");
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
		interceptorSource.AppendLine($"\tstatic class Interceptor{stubbedComponentGroup.StubClassName}");
		interceptorSource.AppendLine("\t{");

		foreach (var hit in stubClassGrouped)
		{
			interceptorSource.AppendLine(
				$"\t\t[System.Runtime.CompilerServices.InterceptsLocationAttribute(\"{hit.Path}\", {hit.Line}, {hit.Column})]");
		}

		interceptorSource.AppendLine(
			"\t\tpublic static global::Bunit.ComponentFactoryCollection AddGeneratedStubInterceptor<TComponent>(this global::Bunit.ComponentFactoryCollection factories)");
		interceptorSource.AppendLine("\t\t\twhere TComponent : Microsoft.AspNetCore.Components.IComponent");
		interceptorSource.AppendLine("\t\t{");
		interceptorSource.AppendLine(
			$"\t\t\treturn factories.Add<global::{stubbedComponentGroup.TargetType.ToDisplayString()}, {stubbedComponentGroup.TargetTypeNamespace}.{stubbedComponentGroup.StubClassName}>();");
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
