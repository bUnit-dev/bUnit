using System.Threading.Tasks;

namespace Bunit.TestAssets.SampleComponents.Data
{
	public class AsyncNameDep : IAsyncTestDep
	{
		private TaskCompletionSource<string> completionSource = new TaskCompletionSource<string>();

		public Task<string> GetDataAsync() => completionSource.Task;

		public void SetResult(string name)
		{
			var prevSource = completionSource;
			completionSource = new TaskCompletionSource<string>();
			prevSource.SetResult(name);
		}
	}
}
