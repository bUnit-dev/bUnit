using System.Diagnostics;

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

		await WithDefaultWaitTimeout(TimeSpan.FromMilliseconds(100), async () =>
		{
			var sut = new BunitJSInterop { Mode = JSRuntimeMode.Strict };
			sut.Setup<int>(identifier);

			var invocationTask = sut.JSRuntime.InvokeAsync<int>(identifier);

			var exception = await Should.ThrowAsync<JSRuntimeInvocationNotSetException>(invocationTask.AsTask());
			exception.Invocation.Identifier.ShouldBe(identifier);
		});
	}

	[Fact(DisplayName = "Each pending invocation times out with its own invocation")]
	public async Task Test310()
	{
		await WithDefaultWaitTimeout(TimeSpan.FromMilliseconds(100), async () =>
		{
			var sut = new BunitJSInterop { Mode = JSRuntimeMode.Strict };
			sut.Setup<int>(_ => true);

			var first = sut.JSRuntime.InvokeAsync<int>("first").AsTask();
			var second = sut.JSRuntime.InvokeAsync<int>("second").AsTask();

			(await Should.ThrowAsync<JSRuntimeInvocationNotSetException>(first))
				.Invocation.Identifier.ShouldBe("first");
			(await Should.ThrowAsync<JSRuntimeInvocationNotSetException>(second))
				.Invocation.Identifier.ShouldBe("second");
		});
	}

	[Fact(DisplayName = "A timed out invocation does not affect later invocations")]
	public async Task Test311()
	{
		const string identifier = "testFunction";

		await WithDefaultWaitTimeout(TimeSpan.FromMilliseconds(100), async () =>
		{
			var sut = new BunitJSInterop { Mode = JSRuntimeMode.Strict };
			var handler = sut.Setup<int>(identifier);

			await Should.ThrowAsync<JSRuntimeInvocationNotSetException>(
				sut.JSRuntime.InvokeAsync<int>(identifier).AsTask());

			handler.SetResult(42);

			(await sut.JSRuntime.InvokeAsync<int>(identifier)).ShouldBe(42);
		});
	}

	[Fact(DisplayName = "Setting a result while the timeout elapses does not crash the test host")]
	public async Task Test312()
	{
		const string identifier = "testFunction";
		var timeout = TimeSpan.FromMilliseconds(2);

		await WithDefaultWaitTimeout(timeout, async () =>
		{
			var workers = Enumerable
				.Range(0, Math.Max(4, Environment.ProcessorCount))
				.Select(_ => Task.Run(() => RaceResultAgainstTimeout(identifier, timeout, iterations: 250)));

			await Task.WhenAll(workers);
		});
	}

	private static async Task RaceResultAgainstTimeout(string identifier, TimeSpan timeout, int iterations)
	{
		for (var i = 0; i < iterations; i++)
		{
			var sut = new BunitJSInterop { Mode = JSRuntimeMode.Strict };
			var handler = sut.Setup<int>(identifier);

			var invocationTask = sut.JSRuntime.InvokeAsync<int>(identifier).AsTask();

			// Spin until the timer is due so that setting the result races the elapsing timeout.
			var spin = Stopwatch.StartNew();
			while (spin.Elapsed < timeout)
				Thread.SpinWait(1);

			handler.SetResult(i);

			// Either the result or the timeout may win the race, but the invocation must
			// always complete and never surface anything but the timeout exception.
			var completed = await Task.WhenAny(invocationTask, Task.Delay(TimeSpan.FromSeconds(10)));
			completed.ShouldBe(invocationTask);

			if (invocationTask.Exception is { } exception)
				exception.InnerException.ShouldBeOfType<JSRuntimeInvocationNotSetException>();
		}
	}

	private static async Task WithDefaultWaitTimeout(TimeSpan timeout, Func<Task> test)
	{
		var originalTimeout = BunitContext.DefaultWaitTimeout;
		BunitContext.DefaultWaitTimeout = timeout;
		try
		{
			await test();
		}
		finally
		{
			BunitContext.DefaultWaitTimeout = originalTimeout;
		}
	}
}
