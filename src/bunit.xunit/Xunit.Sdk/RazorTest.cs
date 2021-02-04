namespace Xunit.Sdk
{
	internal sealed class RazorTest : XunitTest
	{
		public int TestNumber { get; }

		public RazorTest(RazorTestCase testCase, string displayName)
			: base(testCase, displayName)
		{
			TestNumber = testCase.TestNumber;
		}
	}
}
