using Xunit.Abstractions;

namespace Xunit.Sdk
{
	internal class RazorTest : XunitTest, ITest
	{
		public int TestIndex { get; }

		public RazorTest(RazorTestCase testCase, string displayName) : base(testCase, displayName)
		{
			TestIndex = testCase.TestIndex;
		}
	}
}
