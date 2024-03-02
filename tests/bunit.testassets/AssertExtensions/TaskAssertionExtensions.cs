namespace Bunit;

public static class TaskAssertionExtensions
{
	public static async Task ShouldCompleteWithin(this Task task, TimeSpan timeout)
	{
		await task.WaitAsync(timeout);
	}
}
