using System.Text;

namespace Bunit;

/// <summary>
/// Exception use to indicate that an invocation was
/// received by the <see cref="BunitJSInterop"/> running in <see cref="JSRuntimeMode.Strict"/> mode,
/// which didn't contain a matching invocation handler.
/// </summary>
[Serializable]
public sealed class JSRuntimeUnhandledInvocationException : Exception
{
	/// <summary>
	/// Gets the unplanned invocation.
	/// </summary>
	public JSRuntimeInvocation Invocation { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="JSRuntimeUnhandledInvocationException"/> class
	/// with the provided <see cref="Invocation"/> attached.
	/// </summary>
	/// <param name="invocation">The unplanned invocation.</param>
	public JSRuntimeUnhandledInvocationException(JSRuntimeInvocation invocation)
		: base(CreateErrorMessage(invocation))
	{
		Invocation = invocation;
	}

	private JSRuntimeUnhandledInvocationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}

	[SuppressMessage("Minor Code Smell", "S6618:\"string.Create\" should be used instead of \"FormattableString\"", Justification = "string.Create not supported in all TFs")]
	private static string CreateErrorMessage(JSRuntimeInvocation invocation)
	{
		var sb = new StringBuilder();
		sb.AppendLine("bUnit's JSInterop has not been configured to handle the call:");
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
		sb.AppendLine("Configure bUnit's JSInterop to handle the call with following:");
		sb.AppendLine();

		if (IsImportModuleInvocation(invocation))
		{
			sb.AppendLine(FormattableString.Invariant($"    SetupModule({GetArguments(invocation, includeIdentifier: false)})"));
		}
		else
		{
			if (invocation.IsVoidResultInvocation)
			{
				sb.AppendLine(FormattableString.Invariant($"    SetupVoid({GetArguments(invocation)})"));
			}
			else
			{
				sb.AppendLine(FormattableString.Invariant($"    Setup<{GetReturnTypeName(invocation.ResultType)}>({GetArguments(invocation)})"));
			}

			if (invocation.Arguments.Any())
			{
				sb.AppendLine("or the following, to match any arguments:");
				if (invocation.IsVoidResultInvocation)
					sb.AppendLine(FormattableString.Invariant($"    SetupVoid(\"{invocation.Identifier}\", _ => true)"));
				else
					sb.AppendLine(FormattableString.Invariant($"    Setup<{GetReturnTypeName(invocation.ResultType)}>(\"{invocation.Identifier}\", _ => true)"));
			}
		}

		sb.AppendLine();
		sb.AppendLine("The setup methods are available on an instance of the BunitJSInterop or");
		sb.AppendLine("BunitJSModuleInterop type. The standard BunitJSInterop is available");
		sb.AppendLine("through the TestContext.JSInterop property, and a BunitJSModuleInterop");
		sb.AppendLine("instance is returned from calling SetupModule on a BunitJSInterop instance.");
		return sb.ToString();
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
			Type { FullName: "System.SByte" } => "sbyte",
			Type { FullName: "System.Single" } => "float",
			Type { FullName: "System.UInt16" } => "ushort",
			Type { FullName: "System.UInt32" } => "uint",
			Type { FullName: "System.UInt64" } => "ulong",
			Type x => x.Name
		};

	private static string GetGenericInvocationArguments(JSRuntimeInvocation invocation)
	{
		if (invocation.InvocationMethodName.Equals("InvokeUnmarshalled", StringComparison.Ordinal))
		{
			var genericArgs = invocation.Arguments
				.Select(x => x is not null ? GetReturnTypeName(x.GetType()) : "?")
				.Append(GetReturnTypeName(invocation.ResultType));

			return string.Join(", ", genericArgs);
		}
		else
		{
			return GetReturnTypeName(invocation.ResultType);
		}
	}

	private static bool IsImportModuleInvocation(JSRuntimeInvocation invocation)
	{
		const string DefaultImportIdentifier = "import";
		return string.Equals(invocation.Identifier, DefaultImportIdentifier, StringComparison.Ordinal)
			&& typeof(IJSObjectReference).IsAssignableFrom(invocation.ResultType);
	}

	private static string GetArguments(JSRuntimeInvocation invocation, bool includeIdentifier = true)
	{
		var args = invocation.Arguments
			.Select(x => x is string s ? $"\"{s}\"" : x?.ToString() ?? "null");
		if (includeIdentifier)
		{
			args = args.Prepend($"\"{invocation.Identifier}\"");
		}

		return string.Join(", ", args);
	}
}
