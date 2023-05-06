using BenchmarkDotNet.Attributes;
using bunit.benchmarks.assets;

namespace Bunit;

[MemoryDiagnoser]
public class Benchmark : BenchmarkBase
{
	[Benchmark]
	public IRenderedComponent<Counter> RenderCounter()
	{
		return RenderComponent<Counter>();
	}
}