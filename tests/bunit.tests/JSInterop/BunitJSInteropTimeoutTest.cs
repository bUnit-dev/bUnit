namespace Bunit.JSInterop;

[CollectionDefinition(nameof(DefaultWaitTimeoutTestGroup), DisableParallelization = true)]
public sealed class DefaultWaitTimeoutTestGroup
{
}

[Collection(nameof(DefaultWaitTimeoutTestGroup))]
public class BunitJSInteropTimeoutTest
{
	[Fact(DisplayName = "JSRuntime invocation times out when handler is not configured")]
	public async Task Test309()
	{
		const string identifier = "testFunction";
		var originalTimeout = BunitContext.DefaultWaitTimeout;

		try
		{
			BunitContext.DefaultWaitTimeout = TimeSpan.FromMilliseconds(100);

			var sut = new BunitJSInterop { Mode = JSRuntimeMode.Strict };
			sut.Setup<int>(identifier);

			var invocationTask = sut.JSRuntime.InvokeAsync<int>(identifier);

			var exception = await Should.ThrowAsync<JSRuntimeInvocationNotSetException>(invocationTask.AsTask());
			exception.Invocation.Identifier.ShouldBe(identifier);
		}
		finally
		{
			BunitContext.DefaultWaitTimeout = originalTimeout;
		}
	}
}
