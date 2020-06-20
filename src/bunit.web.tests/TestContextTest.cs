using System;
using Bunit.TestAssets.SampleComponents;
using Bunit.TestDoubles.JSInterop;
using Shouldly;

using Xunit;

namespace Bunit
{
	public class TestContextTest : TestContext
	{
		[Fact(DisplayName = "The test service provider should register a placeholder IJSRuntime which throws exceptions")]
		public void Test021()
		{
			var ex = Should.Throw<AggregateException>(() => RenderComponent<SimpleWithJSRuntimeDep>());
			ex.InnerException.ShouldBeOfType<MissingMockJSRuntimeException>();
		}

		[Fact(DisplayName = "The placeholder IJSRuntime is overridden by a supplied mock and does not throw")]
		public void Test022()
		{
			Services.AddMockJSRuntime();

			RenderComponent<SimpleWithJSRuntimeDep>();
		}
	}
}
