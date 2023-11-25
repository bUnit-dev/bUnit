using Microsoft.AspNetCore.Components;

namespace Bunit.Web.Stub;

public class StubTests
{
	[Fact]
	public void Test001()
	{}
	
	[Stub(typeof(CounterComponentStub))]
	public class CounterComponentStub : ComponentBase
	{
	}
}
