#if NET6_0_OR_GREATER
using AutoFixture.Xunit2;
using Bunit.TestAssets.SampleComponents;
using Microsoft.AspNetCore.Components.Web;
using Shouldly;
using Xunit;

namespace Bunit.TestDoubles.ErrorBoundary
{
	public partial class TestContextTest : TestContext
	{
		[Fact(DisplayName = "TestContextBase should provide a default IErrorBoundaryLogger")]
		public void Test001()
		{
			IErrorBoundaryLogger logger = Services.GetService<IErrorBoundaryLogger>();
			logger.ShouldNotBe(null);
		}
	}
}
#endif
