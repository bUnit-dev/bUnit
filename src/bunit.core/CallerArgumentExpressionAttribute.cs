#if NETSTANDARD
namespace System.Runtime.CompilerServices;

sealed class CallerArgumentExpressionAttribute(string parameterName) :
	Attribute
{
	public string ParameterName { get; } = parameterName;
}
#endif
