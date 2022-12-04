namespace Bunit;

public static class TaskAssertionExtensions
{
	public static Task ShouldCompleteWithin(this Task task, TimeSpan timeout)
	{
		return task.WaitAsync(timeout);
	}
}
