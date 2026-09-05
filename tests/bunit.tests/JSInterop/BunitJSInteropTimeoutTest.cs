namespace Bunit.JSInterop;

public class BunitJSInteropTimeoutTest
{
	[Fact(DisplayName = "JSRuntime invocation times out when handler is not configured")]
	public async Task Test309()
	{
		const string identifier = "testFunction";

		var sut = new BunitJSInterop { Mode = JSRuntimeMode.Strict, DefaultWaitTimeout = TimeSpan.FromMilliseconds(100) };
		sut.Setup<int>(identifier);

		var invocationTask = sut.JSRuntime.InvokeAsync<int>(identifier);

		var exception = await Should.ThrowAsync<JSRuntimeInvocationNotSetException>(invocationTask.AsTask());
		exception.Invocation.Identifier.ShouldBe(identifier);
	}
}
