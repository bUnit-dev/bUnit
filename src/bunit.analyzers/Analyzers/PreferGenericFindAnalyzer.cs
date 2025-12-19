using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Bunit.Analyzers;

/// <summary>
/// Analyzer that detects cast expressions from Find() and suggests using Find{T}() instead.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class PreferGenericFindAnalyzer : DiagnosticAnalyzer
{
	/// <inheritdoc/>
	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(DiagnosticDescriptors.PreferGenericFind);

	/// <inheritdoc/>
	public override void Initialize(AnalysisContext context)
	{
		if (context is null)
		{
			throw new System.ArgumentNullException(nameof(context));
		}

		context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
		context.EnableConcurrentExecution();

		context.RegisterSyntaxNodeAction(AnalyzeCastExpression, SyntaxKind.CastExpression);
	}

	private static void AnalyzeCastExpression(SyntaxNodeAnalysisContext context)
	{
		var castExpression = (CastExpressionSyntax)context.Node;

		// Check if the cast is on a Find() invocation
		if (castExpression.Expression is not InvocationExpressionSyntax invocation)
		{
			return;
		}

		// Check if it's a member access expression (e.g., cut.Find(...))
		if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
		{
			return;
		}

		// Check if the method name is "Find"
		if (memberAccess.Name.Identifier.ValueText != "Find")
		{
			return;
		}

		// Get the method symbol to verify it's the bUnit Find method
		var methodSymbol = context.SemanticModel.GetSymbolInfo(memberAccess, context.CancellationToken).Symbol as IMethodSymbol;
		if (methodSymbol is null)
		{
			return;
		}

		// Check if the method is from IRenderedFragment or related types
		var containingType = methodSymbol.ContainingType;
		if (containingType is null || !IsRenderedFragmentType(containingType))
		{
			return;
		}

		// Get the selector argument if present
		var selector = invocation.ArgumentList.Arguments.FirstOrDefault()?.Expression.ToString() ?? "selector";

		// Get the cast type
		var castType = castExpression.Type.ToString();

		var diagnostic = Diagnostic.Create(
			DiagnosticDescriptors.PreferGenericFind,
			castExpression.GetLocation(),
			castType,
			selector);

		context.ReportDiagnostic(diagnostic);
	}

	private static bool IsRenderedFragmentType(INamedTypeSymbol type)
	{
		// Check if the type or any of its interfaces is IRenderedFragment
		var typeName = type.Name;
		if (typeName is "IRenderedFragment" or "IRenderedComponent" or "RenderedFragment" or "RenderedComponent")
		{
			return true;
		}

		foreach (var @interface in type.AllInterfaces)
		{
			if (@interface.Name is "IRenderedFragment" or "IRenderedComponent")
			{
				return true;
			}
		}

		return false;
	}
}
