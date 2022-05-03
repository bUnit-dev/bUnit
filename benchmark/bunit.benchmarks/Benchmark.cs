using BenchmarkDotNet.Attributes;
using bunit.benchmarks.assets;

namespace Bunit;

[MemoryDiagnoser]
public class Benchmark : TestContext
{
    [Benchmark]
    public IRenderedComponent<Counter> RenderCounter()
    {
        return RenderComponent<Counter>();
    }
}