using Shouldly;

namespace Bunit.JSInterop
{
	public static class JSRuntimeInvocationAssertionHelpers
	{
		public static void ShouldBe(this JSRuntimeInvocation actual, JSRuntimeInvocation expected)
		{
			actual.ShouldSatisfyAllConditions(
				x => x.Identifier.ShouldBe(expected.Identifier),
				x => x.Arguments.ShouldBe(expected.Arguments),
				x => x.CancellationToken.ShouldBe(expected.CancellationToken),
				x => x.InvocationMethodName.ShouldBe(expected.InvocationMethodName),
				x => x.IsVoidResultInvocation.ShouldBe(expected.IsVoidResultInvocation),
				x => x.ResultType.ShouldBe(expected.ResultType));
		}
	}
}
