namespace Bunit;

public static class TaskAssertionExtensions
{
	public static async Task ShouldCompleteWithin(this Task task, TimeSpan timeout)
	{
		if (task != await Task.WhenAny(task, Task.Delay(timeout)))
			throw new TimeoutException();
	}
}
