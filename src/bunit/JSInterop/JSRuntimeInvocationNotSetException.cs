using System.Text;

namespace Bunit;

/// <summary>
/// Exception used to indicate that an invocation was received by a JSRuntime invocation handler,
/// but the handler was not configured with a result (via SetResult, SetVoidResult, SetCanceled, or SetException).
/// This causes the invocation to hang indefinitely.
/// </summary>
public sealed class JSRuntimeInvocationNotSetException : Exception
{
	/// <summary>
	/// Gets the invocation that was not handled with a result.
	/// </summary>
	public JSRuntimeInvocation Invocation { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="JSRuntimeInvocationNotSetException"/> class
	/// with the provided <see cref="Invocation"/> attached.
	/// </summary>
	/// <param name="invocation">The invocation that was not provided with a result.</param>
	public JSRuntimeInvocationNotSetException(JSRuntimeInvocation invocation)
		: base(CreateErrorMessage(invocation))
	{
		Invocation = invocation;
	}

	[SuppressMessage("Minor Code Smell", "S6618:\"string.Create\" should be used instead of \"FormattableString\"", Justification = "string.Create not supported in all TFs")]
	private static string CreateErrorMessage(JSRuntimeInvocation invocation)
	{
		var sb = new StringBuilder();
		sb.AppendLine("bUnit's JSInterop invocation handler was setup to handle the call:");
		sb.AppendLine();

		if (invocation.IsVoidResultInvocation)
		{
			sb.AppendLine(FormattableString.Invariant($"    {invocation.InvocationMethodName}({GetArguments(invocation)})"));
		}
		else
		{
			sb.AppendLine(FormattableString.Invariant($"    {invocation.InvocationMethodName}<{GetGenericInvocationArguments(invocation)}>({GetArguments(invocation)})"));
		}

		sb.AppendLine();
		sb.AppendLine("However, the invocation handler was not configured to return a result,");
		sb.AppendLine("causing the invocation to hang indefinitely.");
		sb.AppendLine();
		sb.AppendLine("To fix this, configure the handler to return a result using one of the following methods:");
		sb.AppendLine();

		if (invocation.IsVoidResultInvocation)
		{
			sb.AppendLine("    handler.SetVoidResult();");
		}
		else
		{
			sb.AppendLine(FormattableString.Invariant($"    handler.SetResult({GetExampleResult(invocation.ResultType)});"));
		}

		sb.AppendLine("    handler.SetCanceled();");
		sb.AppendLine("    handler.SetException(new Exception(\"error message\"));");
		return sb.ToString();
	}

	private static string GetArguments(JSRuntimeInvocation invocation)
	{
		if (!invocation.Arguments.Any())
			return $"\"{invocation.Identifier}\"";

		var argStrings = invocation.Arguments.Select(FormatArgument).Prepend($"\"{invocation.Identifier}\"");
		return string.Join(", ", argStrings);
	}

	private static string GetGenericInvocationArguments(JSRuntimeInvocation invocation)
	{
		return GetReturnTypeName(invocation.ResultType);
	}

	private static string FormatArgument(object? arg)
	{
		return arg switch
		{
			null => "null",
			string str => $"\"{str}\"",
			char c => $"'{c}'",
			bool b => b.ToString().ToUpperInvariant(),
			_ => arg.ToString() ?? "null"
		};
	}

	private static string GetReturnTypeName(Type resultType)
		=> resultType switch
		{
			Type { FullName: "System.Boolean" } => "bool",
			Type { FullName: "System.Byte" } => "byte",
			Type { FullName: "System.Char" } => "char",
			Type { FullName: "System.Double" } => "double",
			Type { FullName: "System.Int16" } => "short",
			Type { FullName: "System.Int32" } => "int",
			Type { FullName: "System.Int64" } => "long",
			Type { FullName: "System.Single" } => "float",
			Type { FullName: "System.String" } => "string",
			Type { FullName: "System.Decimal" } => "decimal",
			Type { FullName: "System.Guid" } => "Guid",
			Type { FullName: "System.DateTime" } => "DateTime",
			Type { FullName: "System.DateTimeOffset" } => "DateTimeOffset",
			Type { FullName: "System.TimeSpan" } => "TimeSpan",
			Type { FullName: "System.Object" } => "object",
			_ => resultType.Name
		};

	private static string GetExampleResult(Type resultType)
		=> resultType switch
		{
			Type { FullName: "System.Boolean" } => "true",
			Type { FullName: "System.Byte" } => "1",
			Type { FullName: "System.Char" } => "'a'",
			Type { FullName: "System.Double" } => "1.0",
			Type { FullName: "System.Int16" } => "1",
			Type { FullName: "System.Int32" } => "1",
			Type { FullName: "System.Int64" } => "1L",
			Type { FullName: "System.Single" } => "1.0f",
			Type { FullName: "System.String" } => "\"result\"",
			Type { FullName: "System.Decimal" } => "1.0m",
			Type { FullName: "System.Guid" } => "Guid.NewGuid()",
			Type { FullName: "System.DateTime" } => "DateTime.Now",
			Type { FullName: "System.DateTimeOffset" } => "DateTimeOffset.Now",
			Type { FullName: "System.TimeSpan" } => "TimeSpan.FromSeconds(1)",
			Type { FullName: "System.Object" } => "new object()",
			_ => $"new {resultType.Name}()"
		};
}
