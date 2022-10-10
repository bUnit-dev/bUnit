#if NET6_0_OR_GREATER
using System.Runtime.CompilerServices;

namespace Bunit;

internal static class ModuleInitializer
{
	[ModuleInitializer]

	public static void Init()
	{
		ThreadPool.SetMinThreads(100, 100);
	}
}
#endif
