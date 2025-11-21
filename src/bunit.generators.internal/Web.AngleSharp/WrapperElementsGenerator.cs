#nullable enable
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;

namespace Bunit.Web.AngleSharp;

[Generator]
public class WrapperElementsGenerator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		// Finds the AngleSharp assembly referenced by the target project
		// and collects element interface type names into cacheable records.
		var elementInterfaces = context
			.CompilationProvider
			.Select((compilation, cancellationToken) =>
			{
				var meta = compilation.References.FirstOrDefault(x => x.Display?.EndsWith($"{Path.DirectorySeparatorChar}AngleSharp.dll", StringComparison.Ordinal) ?? false);
				var assembly = compilation.GetAssemblyOrModuleSymbol(meta);
				
				if (assembly is not IAssemblySymbol angleSharpAssembly)
					return null;

				var elementInterfaceTypes = FindElementInterfaces(angleSharpAssembly);
				// Create cacheable records with just the essential info needed for generation
				// Store metadata names instead of symbols for cacheability
				return new ElementInterfacesData(
					elementInterfaceTypes.Select(t => new ElementTypeInfo(
						t.Name,
						t.ToDisplayString(GeneratorConfig.SymbolFormat),
						GetMetadataName(t)
					)).ToImmutableArray());
			});

		// Combine with compilation to retrieve symbols during execution
		var elementInterfacesWithCompilation = elementInterfaces.Combine(context.CompilationProvider);

		// Output the hardcoded source files
		context.RegisterSourceOutput(elementInterfaces, GenerateStaticContent);

		// Output the generated wrapper types
		context.RegisterSourceOutput(elementInterfacesWithCompilation, GenerateWrapperTypes);
	}

	private static void GenerateStaticContent(SourceProductionContext context, ElementInterfacesData? data)
	{
		if (data is null)
			return;

		context.AddSource("IElementWrapperFactory.g.cs", ReadEmbeddedResource("Bunit.Web.AngleSharp.IElementWrapperFactory.cs"));
		context.AddSource("IElementWrapper.g.cs", ReadEmbeddedResource("Bunit.Web.AngleSharp.IElementWrapper.cs"));
		context.AddSource("WrapperBase.g.cs", ReadEmbeddedResource("Bunit.Web.AngleSharp.WrapperBase.cs"));
	}

	private static void GenerateWrapperTypes(SourceProductionContext context, (ElementInterfacesData? data, Compilation compilation) input)
	{
		var (data, compilation) = input;
		if (data is null)
			return;

		// Find the AngleSharp assembly in the compilation
		var meta = compilation.References.FirstOrDefault(x => x.Display?.EndsWith($"{Path.DirectorySeparatorChar}AngleSharp.dll", StringComparison.Ordinal) ?? false);
		var assembly = compilation.GetAssemblyOrModuleSymbol(meta);
		
		if (assembly is not IAssemblySymbol angleSharpAssembly)
			return;

		// Retrieve the actual symbols from the assembly for code generation
		var elementSymbols = data.ElementTypes
			.Select(t => angleSharpAssembly.GetTypeByMetadataName(t.MetadataName))
			.Where(s => s is not null)
			.Cast<INamedTypeSymbol>()
			.ToList();

		var source = new StringBuilder();
		foreach (var elm in elementSymbols)
		{
			source.Clear();
			var name = WrapperElementGenerator.GenerateWrapperTypeSource(source, elm);
			context.AddSource($"{name}.g.cs", SourceText.From(source.ToString(), Encoding.UTF8));
		}

		source.Clear();
		GenerateWrapperFactory(source, data.ElementTypes);
		context.AddSource($"WrapperExtensions.g.cs", SourceText.From(source.ToString(), Encoding.UTF8));
	}

	private static void GenerateWrapperFactory(StringBuilder source, ImmutableArray<ElementTypeInfo> elementTypes)
	{
		source.AppendLine("""namespace Bunit.Web.AngleSharp;""");
		source.AppendLine();
		source.AppendLine("/// <summary>");
		source.AppendLine("/// Provide helpers for wrapped HTML elements.");
		source.AppendLine("/// </summary>");
		source.AppendLine("[System.CodeDom.Compiler.GeneratedCodeAttribute(\"Bunit.Web.AngleSharp\", \"1.0.0.0\")]");
		source.AppendLine("public static class WrapperExtensions");
		source.AppendLine("{");
		source.AppendLine();
		source.AppendLine("/// <summary>");
		source.AppendLine("/// Provide wrapper to be used when elements re-render.");
		source.AppendLine("/// </summary>");
		source.AppendLine($"\tpublic static global::AngleSharp.Dom.IElement WrapUsing<TElementFactory>(this global::AngleSharp.Dom.IElement element, TElementFactory elementFactory) where TElementFactory : Bunit.Web.AngleSharp.IElementWrapperFactory => element switch");
		source.AppendLine("\t{");

		foreach (var elm in elementTypes)
		{
			// Element interface names start with 'I' (e.g., IElement -> ElementWrapper)
			var wrapperName = elm.Name.Length > 1 && elm.Name.StartsWith("I", StringComparison.Ordinal)
				? $"{elm.Name[1..]}Wrapper"
				: $"{elm.Name}Wrapper";
			source.AppendLine($"\t\t{elm.FullyQualifiedName} e => new {wrapperName}(e, elementFactory),");
		}

		source.AppendLine($"\t\t_ => new ElementWrapper(element, elementFactory),");

		source.AppendLine("\t};");
		source.AppendLine("}");
	}

	private static string GetMetadataName(INamedTypeSymbol typeSymbol)
	{
		// Get the full metadata name that can be used with GetTypeByMetadataName
		// This is the fully qualified name without the "global::" prefix
		var containingNamespace = typeSymbol.ContainingNamespace;
		var namespacePrefix = containingNamespace?.IsGlobalNamespace == false 
			? containingNamespace.ToDisplayString() + "."
			: "";
		return namespacePrefix + typeSymbol.Name;
	}

	private static IReadOnlyList<INamedTypeSymbol> FindElementInterfaces(IAssemblySymbol angleSharpAssembly)
	{
		var htmlDomNamespace = angleSharpAssembly
			.GlobalNamespace
			.GetNamespaceMembers()
			.First(x => x.Name == "AngleSharp")
			.GetNamespaceMembers()
			.First(x => x.Name == "Html")
			.GetNamespaceMembers()
			.First(x => x.Name == "Dom");

		var elementInterfaceSymbol = angleSharpAssembly
			.GetTypeByMetadataName("AngleSharp.Dom.IElement");

		if (elementInterfaceSymbol is null)
			return Array.Empty<INamedTypeSymbol>();

		var result = htmlDomNamespace
			.GetTypeMembers()
			.Where(typeSymbol => typeSymbol.TypeKind == TypeKind.Interface && typeSymbol.AllInterfaces.Contains(elementInterfaceSymbol))
			.ToList();

		result.Add(elementInterfaceSymbol);

		// Order the interfaces such that interfaces that inherits
		// from other interfaces appears earlier in the result.
		// E.g. IHtmlElement appears before IElement.
		result.Sort(static (x, y) =>
		{
			if (x.AllInterfaces.Contains(y))
				return -1;
			else if (y.AllInterfaces.Contains(x))
				return 1;
			return 0;
		});

		return result;
	}

	private static string ReadEmbeddedResource(string resourceName)
	{
		using var stream = typeof(WrapperElementsGenerator).Assembly.GetManifestResourceStream(resourceName);

		if (stream is null)
		{
			throw new InvalidOperationException($"Could not find embedded resource: {resourceName}");
		}

		using var reader = new StreamReader(stream);
		return reader.ReadToEnd();
	}
}

// Cacheable data structure that stores minimal information about element interfaces
// This allows the incremental generator to cache and reuse results across builds
internal sealed record ElementInterfacesData(
	ImmutableArray<ElementTypeInfo> ElementTypes);

internal sealed record ElementTypeInfo(
	string Name,
	string FullyQualifiedName,
	string MetadataName);
