using System.Threading.Tasks;

namespace Bunit.TestAssets.SampleComponents.Data
{
	public interface IAsyncTestDep
	{
		Task<string> GetDataAsync();
	}
}
