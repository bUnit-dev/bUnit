using System.Runtime.CompilerServices;

namespace Bunit;

internal static class VerifyInitializer
{
	[ModuleInitializer]
	public static void Init() =>
		VerifySourceGenerators.Initialize();
}