using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit.Mocking.JSInterop;
using Bunit.TestAssets.SampleComponents;
using Shouldly;
using Xunit;

namespace Bunit
{
	public class TestContextTest : TestContext
	{
		[Fact(DisplayName = "The test service provider should register a placeholder IJSRuntime which throws exceptions")]
		public void Test021()
		{
			var ex = Should.Throw<AggregateException>(() => RenderComponent<SimpleWithJsRuntimeDep>());
			ex.InnerException.ShouldBeOfType<MissingMockJsRuntimeException>();
		}

		[Fact(DisplayName = "The placeholder IJSRuntime is overridden by a supplied mock and does not throw")]
		public void Test022()
		{
			Services.AddMockJsRuntime();

			RenderComponent<SimpleWithJsRuntimeDep>();
		}
	}
}
