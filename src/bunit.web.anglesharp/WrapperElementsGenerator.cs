using AngleSharp.Html.Dom;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Bunit.Web.AngleSharp;

[Generator]
public class WrapperElementsGenerator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		var assemblyReferences = context
			.CompilationProvider
			.Select((compilation, cancellationToken) => HasAngleSharpReference(compilation));

		context.RegisterSourceOutput(assemblyReferences, (context, hasAngleSharp) =>
		{
			if (!hasAngleSharp)
				return;

			var elementFactorySource = ReadEmbeddedResource("Bunit.Web.AngleSharp.IElementFactory.cs");
			var wrapperBase = ReadEmbeddedResource("Bunit.Web.AngleSharp.WrapperBase.cs");
			context.AddSource("IElementFactory.g.cs", elementFactorySource);
			context.AddSource("WrapperBase.g.cs", wrapperBase);
		});

		context.RegisterSourceOutput(context.CompilationProvider.Combine(assemblyReferences), (context, input) =>
		{
			var (compilation, hasAngleSharp) = input;

			if (!hasAngleSharp)
				return;

			var elementInterfacetypes = FindElementInterfaces(compilation);

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
		});
	}

	private static void GenerateWrapperFactory(StringBuilder source, IEnumerable<INamedTypeSymbol> elementInterfacetypes)
	{
		source.AppendLine("""namespace Bunit.Web.AngleSharp;""");
		source.AppendLine();
		source.AppendLine($"internal static class WrapperExtensions");
		source.AppendLine("{");
		source.AppendLine();
		source.AppendLine($"\tpublic static global::AngleSharp.Dom.IElement WrapUsing<TElementFactory>(this global::AngleSharp.Dom.IElement element, TElementFactory elementFactory) where TElementFactory : IElementFactory => element switch");
		source.AppendLine("\t{");

		foreach (var elm in elementInterfacetypes)
		{
			var wrapperName = $"{elm.Name.Substring(1)}Wrapper";
			source.AppendLine($"\t\t{elm.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)} e => new {wrapperName}(e, elementFactory),");
		}
		source.AppendLine($"\t\t_ => new ElementWrapper(element, elementFactory),");

		source.AppendLine("\t};");
		source.AppendLine("}");
	}

	private static IReadOnlyList<INamedTypeSymbol> FindElementInterfaces(Compilation compilation)
	{
		var meta = compilation.References.First(x => x.Display?.EndsWith("\\AngleSharp.dll") ?? false);

		var angleSharpAssemblySymbol = compilation.GetAssemblyOrModuleSymbol(meta) as IAssemblySymbol;
		var htmlDomNameSpace = angleSharpAssemblySymbol
			.GlobalNamespace
			.GetNamespaceMembers()
			.First(x => x.Name == "AngleSharp")
			.GetNamespaceMembers()
			.First(x => x.Name == "Html")
			.GetNamespaceMembers()
			.First(x => x.Name == "Dom");

		var elementInterfaceSymbol = angleSharpAssemblySymbol.GetTypeByMetadataName("AngleSharp.Dom.IElement");

		var result = htmlDomNameSpace
			.GetTypeMembers()
			.Where(typeSymbol => typeSymbol.TypeKind == TypeKind.Interface && typeSymbol.AllInterfaces.Contains(elementInterfaceSymbol))
			.ToList();

		result.Add(elementInterfaceSymbol);

		// Order
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

	private static bool HasAngleSharpReference(Compilation compilation)
	{
		foreach (var reference in compilation.ReferencedAssemblyNames)
		{
			if (reference.Name.StartsWith("AngleSharp", StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}

		return false;
	}
}
