using System.Runtime.CompilerServices;

[assembly: System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]

namespace Bunit;

internal static class ThreadPoolSetup
{
	// Many tests block a pool thread synchronously (WaitFor*, Click) while xunit runs 2x processor count in parallel,
	// so the default pool floor starves the continuations those tests wait on, causing sporadic WaitFor* timeouts on CI.
	[ModuleInitializer]
	internal static void RaiseMinThreads()
	{
		ThreadPool.GetMinThreads(out var workers, out var io);
		ThreadPool.SetMinThreads(Math.Max(workers, Environment.ProcessorCount * 4), io);
	}
}
