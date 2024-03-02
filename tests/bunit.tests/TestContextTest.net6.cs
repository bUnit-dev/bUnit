namespace Bunit;

public partial class TestContextTest : TestContext
{
	[Fact(DisplayName = "TestContext should provide a default IErrorBoundaryLogger")]
	public void Test001()
	{
		IErrorBoundaryLogger logger = Services.GetService<IErrorBoundaryLogger>();
		logger.ShouldNotBe(null);
	}
}
