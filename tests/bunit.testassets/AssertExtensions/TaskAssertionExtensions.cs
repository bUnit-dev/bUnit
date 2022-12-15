namespace Bunit;

public static class TaskAssertionExtensions
{
	public static async Task ShouldCompleteWithin(this Task task, TimeSpan timeout)
	{
#if NET6_0_OR_GREATER
		await task.WaitAsync(timeout);
#else
		using var cts = new CancellationTokenSource();
        var delayTask = Task.Delay(timeout, cts.Token);
        if (task != await Task.WhenAny(task, delayTask))
            throw new TimeoutException();

        cts.Cancel();
#endif
	}
}
