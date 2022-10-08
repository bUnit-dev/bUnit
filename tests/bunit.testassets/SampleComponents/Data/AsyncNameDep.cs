namespace Bunit.TestAssets.SampleComponents.Data;

public class AsyncNameDep : IAsyncTestDep
{
	private TaskCompletionSource<string> completionSource = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

	public Task<string> GetDataAsync() => completionSource.Task;

	public void SetResult(string name)
	{
		var prevSource = completionSource;
		completionSource = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
		prevSource.SetResult(name);
	}
}
