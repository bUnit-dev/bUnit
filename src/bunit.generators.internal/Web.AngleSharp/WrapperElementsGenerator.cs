using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
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
		// This should prevent the source generator from running unless a
		// new symbol is returned.
		var angleSharpAssemblyReference = context
			.CompilationProvider
			.Select((compilation, cancellationToken) =>
			{
				var meta = compilation.References.FirstOrDefault(x => x.Display?.EndsWith($"{Path.DirectorySeparatorChar}AngleSharp.dll", StringComparison.Ordinal) ?? false);
				return compilation.GetAssemblyOrModuleSymbol(meta);
			});

		// Output the hardcoded source files
		context.RegisterSourceOutput(angleSharpAssemblyReference, GenerateStaticContent);

		// Output the generated wrapper types
		context.RegisterSourceOutput(angleSharpAssemblyReference, GenerateWrapperTypes);
	}

	private static void GenerateStaticContent(SourceProductionContext context, ISymbol assembly)
	{
		if (assembly is not IAssemblySymbol)
			return;

		context.AddSource("IElementWrapperFactory.g.cs", ReadEmbeddedResource("Bunit.Web.AngleSharp.IElementWrapperFactory.cs"));
		context.AddSource("IElementWrapper.g.cs", ReadEmbeddedResource("Bunit.Web.AngleSharp.IElementWrapper.cs"));
		context.AddSource("WrapperBase.g.cs", ReadEmbeddedResource("Bunit.Web.AngleSharp.WrapperBase.cs"));
	}

	private static void GenerateWrapperTypes(SourceProductionContext context, ISymbol assembly)
	{
		if (assembly is not IAssemblySymbol angleSharpAssembly)
			return;

		var elementInterfacetypes = FindElementInterfaces(angleSharpAssembly);

		var source = new StringBuilder();
		foreach (var elm in elementInterfacetypes)
		{
			source.Clear();
			var name = WrapperElementGenerator.GenerateWrapperTypeSource(source, elm);
			context.AddSource($"{name}.g.cs", SourceText.From(source.ToString(), Encoding.UTF8));
		}

		source.Clear();
		GenerateWrapperFactory(source, elementInterfacetypes);
		context.AddSource($"WrapperExtensions.g.cs", SourceText.From(source.ToString(), Encoding.UTF8));
	}

	private static void GenerateWrapperFactory(StringBuilder source, IEnumerable<INamedTypeSymbol> elementInterfacetypes)
	{
		source.AppendLine("""namespace Bunit.Web.AngleSharp;""");
		source.AppendLine();
		source.AppendLine("[System.CodeDom.Compiler.GeneratedCodeAttribute(\"Bunit.Web.AngleSharp\", \"1.0.0.0\")]");
		source.AppendLine($"internal static class WrapperExtensions");
		source.AppendLine("{");
		source.AppendLine();
		source.AppendLine($"\tpublic static global::AngleSharp.Dom.IElement WrapUsing<TElementFactory>(this global::AngleSharp.Dom.IElement element, TElementFactory elementFactory) where TElementFactory : Bunit.Web.AngleSharp.IElementWrapperFactory => element switch");
		source.AppendLine("\t{");

		foreach (var elm in elementInterfacetypes)
		{
			var wrapperName = $"{elm.Name.Substring(1)}Wrapper";
			source.AppendLine($"\t\t{elm.ToDisplayString(GeneratorConfig.SymbolFormat)} e => new {wrapperName}(e, elementFactory),");
		}

		source.AppendLine($"\t\t_ => new ElementWrapper(element, elementFactory),");

		source.AppendLine("\t};");
		source.AppendLine("}");
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
