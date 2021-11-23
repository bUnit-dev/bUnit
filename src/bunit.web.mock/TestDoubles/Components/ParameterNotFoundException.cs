using System;
using System.Runtime.Serialization;

namespace Bunit.TestDoubles;

/// <summary>
/// Represents an exception which is thrown when the 
/// <see cref="CapturedParameterView{TComponent}.Get{TValue}(System.Linq.Expressions.Expression{Func{TComponent, TValue}})"/>
/// is used to get a parameter that was not passed to the doubled component.
/// </summary>
[Serializable]
public sealed class ParameterNotFoundException : Exception
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ParameterNotFoundException"/> class.
	/// </summary>
	/// <param name="parameterName"></param>
	/// <param name="componentName"></param>
	public ParameterNotFoundException(string parameterName, string componentName)
		: base($"The parameter '{parameterName}' was not passed to the component '{componentName}' when it was rendered.")
	{ }

	private ParameterNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}
