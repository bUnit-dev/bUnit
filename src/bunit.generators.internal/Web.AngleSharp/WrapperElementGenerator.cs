using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Bunit.Web.AngleSharp;

internal static class WrapperElementGenerator
{
	internal static string GenerateWrapperTypeSource(StringBuilder source, INamedTypeSymbol elm)
	{
		var name = $"{elm.Name.Substring(1)}Wrapper";
		var wrappedTypeName = elm.ToDisplayString(GeneratorConfig.SymbolFormat);

		source.AppendLine("#nullable enable");
		source.AppendLine("using System.Runtime.CompilerServices;");
		source.AppendLine();
		source.AppendLine("""namespace Bunit.Web.AngleSharp;""");
		source.AppendLine();
		source.AppendLine("/// <inheritdoc/>");
		source.AppendLine("[System.Diagnostics.DebuggerDisplay(\"{OuterHtml,nq}\")]");
		source.AppendLine("[System.Diagnostics.DebuggerNonUserCode]");
		source.AppendLine("[System.CodeDom.Compiler.GeneratedCodeAttribute(\"Bunit.Web.AngleSharp\", \"1.0.0.0\")]");
		source.AppendLine($"internal sealed class {name} : WrapperBase<{wrappedTypeName}>, {wrappedTypeName}");
		source.AppendLine("{");
		source.AppendLine();

		source.AppendLine($$"""
					internal {{name}}({{wrappedTypeName}} element, Bunit.Web.AngleSharp.IElementWrapperFactory elementFactory) : base(element, elementFactory) { }
				""");

		var generatedProperties = new HashSet<string>();
		foreach (var symbol in elm.GetAllMembers().OrderBy(x => x.Kind).ThenBy(x => x.Name))
		{
			switch (symbol)
			{
				case IMethodSymbol method when !IsSpecialMethod(method.Name):
					GenerateOrdinaryMethod(source, method);
					break;
				case IEventSymbol evt:
					if (!generatedProperties.Contains(symbol.Name))
					{
						generatedProperties.Add(symbol.Name);
						GenerateEventProperty(source, evt);
					}
					break;
				case IPropertySymbol property when property.IsIndexer:
					GenerateIndexerProperty(source, property);
					break;
				case IPropertySymbol property when !property.IsIndexer:
					GenerateRegularProperty(source, property);
					break;
			}
		}

		source.AppendLine("}");
		source.AppendLine("#nullable restore");
		return name;
	}

	private static void GenerateOrdinaryMethod(StringBuilder source, IMethodSymbol method)
	{
		var methodParts = method.ToDisplayParts(GeneratorConfig.SymbolFormat);

		// It seems that the ToDisplayParts will return ...
		// 
		//    public global::AngleSharp.Dom.IShadowRoot AttachShadow(global::AngleSharp.Dom.ShadowRootMode mode = 0)
		//
		// when called on a method with a default enum parameters specified.
		// However, the C# compiler changes this afterword to ...
		//
		//    public global::AngleSharp.Dom.IShadowRoot AttachShadow(global::AngleSharp.Dom.ShadowRootMode mode = Open)
		//
		// which doesn't compile.
		// So this entire IF is there to fix this, and instead generate ...
		//
		//    public global::AngleSharp.Dom.IShadowRoot AttachShadow(global::AngleSharp.Dom.ShadowRootMode mode = global::AngleSharp.Dom.ShadowRootMode.Open)
		if (method.Parameters.SingleOrDefault(x => x.HasExplicitDefaultValue && x.Type.BaseType?.ToString() == "System.Enum") is IParameterSymbol parameter)
		{
			var defaultValue = parameter.Type
				.GetMembers()
				.OfType<IFieldSymbol>()
				.Single(x => x.ConstantValue == parameter.ExplicitDefaultValue);

			methodParts = methodParts[0..^2]
				.AddRange(methodParts[0..2])
				.AddRange(defaultValue.ToDisplayParts())
				.Add(methodParts[^1]);
		}

		source.AppendLine();
		source.AppendInheritDoc();
		source.AppendDefaultAttributes("\t");
		source.Append("\tpublic ").AppendLine(methodParts.ToDisplayString());
		source.Append($"\t\t=> WrappedElement.{method.Name}(")
			.AppendCallParameters(method.Parameters)
			.AppendLine(");");
	}

	private static void GenerateRegularProperty(StringBuilder source, IPropertySymbol property)
	{
		source.AppendLine();
		source.AppendInheritDoc();
		source.Append("\tpublic ").AppendLine(property.ToDisplayString(GeneratorConfig.SymbolFormat));
		source.AppendLine("\t{");

		if (property.GetMethod is IMethodSymbol)
		{
			source.AppendDefaultAttributes();
			source.AppendLine($"\t\tget => WrappedElement.{property.Name};");
		}

		if (property.SetMethod is IMethodSymbol)
		{
			source.AppendDefaultAttributes();
			source.AppendLine($"\t\tset => WrappedElement.{property.Name} = value;");
		}

		source.AppendLine("\t}");
	}

	private static void GenerateIndexerProperty(StringBuilder source, IPropertySymbol property)
	{
		source.AppendLine();
		source.AppendInheritDoc();
		source.Append("\tpublic ").AppendLine(property.ToDisplayString(GeneratorConfig.SymbolFormat));
		source.AppendLine("\t{");

		if (property.GetMethod is IMethodSymbol)
		{
			source.AppendDefaultAttributes();
			source.Append("\t\tget => WrappedElement[");
			source.AppendCallParameters(property.Parameters);
			source.AppendLine("];");
		}

		if (property.SetMethod is IMethodSymbol)
		{
			source.AppendDefaultAttributes();
			source.Append("\t\tset => WrappedElement[");
			source.AppendCallParameters(property.Parameters);
			source.AppendLine("] = value;");
		}
		source.AppendLine("\t}");
	}

	private static void GenerateEventProperty(StringBuilder source, IEventSymbol eventSymbol)
	{
		source.AppendLine();
		source.AppendInheritDoc();
		source.AppendLine($"\tpublic event {eventSymbol.Type.ToDisplayString(GeneratorConfig.SymbolFormat)} {eventSymbol.Name}");
		source.AppendLine("\t{");
		source.AppendLine("\t\t[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]");
		source.AppendLine("\t\t[System.Diagnostics.DebuggerHidden]");
		source.AppendLine($"\t\tadd => Unsafe.As<{eventSymbol.ContainingSymbol.ToDisplayString(GeneratorConfig.SymbolFormat)}>(WrappedElement).{eventSymbol.Name} += value;");
		source.AppendLine("\t\t[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]");
		source.AppendLine("\t\t[System.Diagnostics.DebuggerHidden]");
		source.AppendLine($"\t\tremove => Unsafe.As<{eventSymbol.ContainingSymbol.ToDisplayString(GeneratorConfig.SymbolFormat)}>(WrappedElement).{eventSymbol.Name} -= value;");
		source.AppendLine("\t}");
	}

	private static IEnumerable<ISymbol> GetAllMembers(this INamedTypeSymbol symbol)
	{
		foreach (var item in symbol.GetMembers())
		{
			yield return item;
		}

		foreach (var item in symbol.AllInterfaces.SelectMany(x => x.GetMembers()))
		{
			yield return item;
		}
	}

	private static StringBuilder AppendInputArguments(this StringBuilder source, ImmutableArray<IParameterSymbol> parameters)
	{
		for (int i = 0; i < parameters.Length; i++)
		{
			if (i > 0)
				source.Append(", ");

			source.Append($"{parameters[i].Type.ToDisplayString(GeneratorConfig.SymbolFormat)} {parameters[i].Name}");
		}
		return source;
	}

	private static StringBuilder AppendCallParameters(this StringBuilder source, ImmutableArray<IParameterSymbol> parameters)
	{
		for (int i = 0; i < parameters.Length; i++)
		{
			if (i > 0)
				source.Append(", ");

			source.Append(parameters[i].Name);
		}
		return source;
	}

	private static StringBuilder AppendInheritDoc(this StringBuilder source)
	{
		source.AppendLine("\t/// <inheritdoc/>");
		return source;
	}

	private static StringBuilder AppendDefaultAttributes(this StringBuilder source, string tabs = "\t\t")
	{
		source.Append(tabs).AppendLine("[System.Diagnostics.DebuggerHidden]");
		source.Append(tabs).AppendLine("[System.Diagnostics.DebuggerStepThrough]");
		source.Append(tabs).AppendLine("[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]");
		return source;
	}

	private static bool IsSpecialMethod(string methodName)
	{
		return methodName.StartsWith("add_") || methodName.StartsWith("remove_") ||
			   methodName.StartsWith("get_") || methodName.StartsWith("set_");
	}
}
