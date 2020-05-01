using Xunit.Abstractions;

namespace Xunit.Sdk
{
	internal class RazorTest : XunitTest, ITest
	{
		public int TestNumber { get; }

		public RazorTest(RazorTestCase testCase, string displayName) : base(testCase, displayName)
		{
			TestNumber = testCase.TestNumber;
		}
	}
}
