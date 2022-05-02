#if NET5_0_OR_GREATER
namespace Bunit.TestDoubles;

public static class InputFileExtensions
{
    private const string InputFileJsIdentifier = "Blazor._internal.InputFile.init";

    public static void AddInputFileSupport(this TestContext context)
    {
        if (context is null)
            throw new ArgumentNullException(nameof(context));
        
        context.JSInterop
            .SetupVoid(invocation => string.Equals(invocation.Identifier, InputFileJsIdentifier, StringComparison.Ordinal))
            .SetVoidResult();
    }
}
#endif