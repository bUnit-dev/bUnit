#if NET5_0_OR_GREATER
namespace Bunit.JSInterop.InvocationHandlers.Implementation;

internal sealed class InputFileInvocationHandler : JSRuntimeInvocationHandler
{
	private const string Identifier = "Blazor._internal.InputFile.init";

	internal InputFileInvocationHandler()
		: base(inv => inv.Identifier.Equals(Identifier, StringComparison.Ordinal), isCatchAllHandler: true)
	{
		SetVoidResult();
	}
}
#endif