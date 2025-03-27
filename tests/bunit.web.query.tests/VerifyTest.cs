namespace Bunit;

public class VerifyTest
{
	[Fact]
	public Task Run() =>
		VerifyChecks.Run();
}
