using Microsoft.CodeAnalysis;
using System.Collections.Generic;
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
		source.AppendLine("""[System.Diagnostics.DebuggerDisplay("{OuterHtml,nq}")]""");
		source.AppendLine("[System.Diagnostics.DebuggerNonUserCode]");
		source.AppendLine("[System.CodeDom.Compiler.GeneratedCodeAttribute(\"Bunit.Web.AngleSharp\", \"1.0.0.0\")]");
		source.AppendLine($"internal sealed class {name} : WrapperBase<{wrappedTypeName}>, {wrappedTypeName}");
		source.AppendLine("{");
		source.AppendLine();

		source.AppendLine($$"""
					internal {{name}}({{wrappedTypeName}} element, Bunit.Web.AngleSharp.IElementFactory elementFactory) : base(element, elementFactory) { }
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
		// Determine the return type of the method
		string returnType = method.ReturnType.ToDisplayString(GeneratorConfig.SymbolFormat);

		// Start building the method signature
		source.AppendLine();
		source.AppendLine("\t/// <inheritdoc/>");
		source.AppendLine("\t[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]");
		source.AppendLine("\t[System.Diagnostics.DebuggerHidden]");
		source.AppendLine("\t[System.Diagnostics.DebuggerStepThrough]");
		source.Append($"\tpublic {returnType} {method.Name}(");

		// Append parameters directly to the StringBuilder using a for loop
		var parameters = method.Parameters;
		for (int i = 0; i < parameters.Length; i++)
		{
			if (i > 0)
				source.Append(", ");

			source.Append($"{parameters[i].Type.ToDisplayString(GeneratorConfig.SymbolFormat)} {parameters[i].Name}");
		}

		// Complete the method signature and start the method body
		source.Append($") => WrappedElement.{method.Name}(");

		// Append method invocation parameters using a for loop
		for (int i = 0; i < parameters.Length; i++)
		{
			if (i > 0)
				source.Append(", ");

			source.Append(parameters[i].Name);
		}

		// Close the method invocation
		source.AppendLine(");");
	}

	private static void GenerateRegularProperty(StringBuilder source, IPropertySymbol property)
	{
		source.AppendLine();
		source.AppendLine("\t/// <inheritdoc/>");
		source.Append($"\tpublic {property.Type.ToDisplayString(GeneratorConfig.SymbolFormat)} {property.Name}");
		source.AppendLine();
		source.AppendLine("\t{");

		if (property.GetMethod is IMethodSymbol)
		{
			source.AppendLine("\t\t[System.Diagnostics.DebuggerHidden]");
			source.AppendLine("\t\t[System.Diagnostics.DebuggerStepThrough]");
			source.AppendLine("\t\t[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]");
			source.AppendLine($"\t\tget => WrappedElement.{property.Name};");
		}

		if (property.SetMethod is IMethodSymbol)
		{
			source.AppendLine("\t\t[System.Diagnostics.DebuggerHidden]");
			source.AppendLine("\t\t[System.Diagnostics.DebuggerStepThrough]");
			source.AppendLine("\t\t[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]");
			source.AppendLine($"\t\tset => WrappedElement.{property.Name} = value;");
		}

		source.AppendLine("\t}");
	}

	private static void GenerateIndexerProperty(StringBuilder source, IPropertySymbol property)
	{
		source.AppendLine();
		source.AppendLine("\t/// <inheritdoc/>");
		source.Append($"\tpublic {property.Type.ToDisplayString(GeneratorConfig.SymbolFormat)} this[");

		foreach (var p in property.Parameters)
		{
			source.Append($"{p.Type.ToDisplayString(GeneratorConfig.SymbolFormat)} {p.Name}");
		}

		source.Append("]");
		source.AppendLine();
		source.AppendLine("\t{");

		if (property.GetMethod is IMethodSymbol)
		{
			source.AppendLine("\t\t[System.Diagnostics.DebuggerHidden]");
			source.AppendLine("\t\t[System.Diagnostics.DebuggerStepThrough]");
			source.AppendLine("\t\t[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]");
			source.Append("\t\tget => WrappedElement[");
			PrintCallParameters();
			source.AppendLine("];");
		}

		if (property.SetMethod is IMethodSymbol)
		{
			source.AppendLine("\t\t[System.Diagnostics.DebuggerHidden]");
			source.AppendLine("\t\t[System.Diagnostics.DebuggerStepThrough]");
			source.AppendLine("\t\t[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]");
			source.Append("\t\tset => WrappedElement[");
			PrintCallParameters();
			source.AppendLine("] = value;");
		}
		source.AppendLine("\t}");

		void PrintCallParameters()
		{
			for (int i = 0; i < property.Parameters.Length; i++)
			{
				if (i > 0)
					source.Append(", ");

				source.Append(property.Parameters[i].Name);
			}
		}
	}

	private static void GenerateEventProperty(StringBuilder source, IEventSymbol eventSymbol)
	{
		source.AppendLine();
		source.AppendLine("\t/// <inheritdoc/>");
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

	private static bool IsSpecialMethod(string methodName)
	{
		return methodName.StartsWith("add_") || methodName.StartsWith("remove_") ||
			   methodName.StartsWith("get_") || methodName.StartsWith("set_");
	}
}
