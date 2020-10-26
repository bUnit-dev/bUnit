using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Bunit.TestDoubles
{
	/// <summary>
	/// This JSRuntime is used to provide users with helpful exceptions if they fail to provide a mock when required. 
	/// </summary>
	internal class PlaceholderJSRuntime : IJSRuntime
	{
		public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
		{
			throw new MissingMockJSRuntimeException(identifier, args);
		}

		public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
		{
			throw new MissingMockJSRuntimeException(identifier, args);
		}
	}
}
