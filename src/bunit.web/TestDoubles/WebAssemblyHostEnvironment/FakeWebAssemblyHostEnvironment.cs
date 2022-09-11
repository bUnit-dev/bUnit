using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Bunit.TestDoubles;

/// <summary>
/// Represents a fake <see cref="IWebAssemblyHostEnvironment"/> that makes the <see cref="Environment"/> and <see cref="BaseAddress"/> settable.
/// </summary>
public class FakeWebAssemblyHostEnvironment : IWebAssemblyHostEnvironment
{
	/// <summary>
	/// Gets the name of the environment. This is configured to use the environment of
	/// the application hosting the Blazor WebAssembly application. Configured to "Production"
	/// when not specified by the host.
	/// In the <see cref="FakeWebAssemblyHostEnvironment"/> it can also be set.
	/// </summary>
	public string Environment { get; set; } = "Production";

	/// <summary>
	/// Gets the base address for the application. This is typically derived from the
	/// >base href&lt; value in the host page.
	/// In the <see cref="FakeWebAssemblyHostEnvironment"/> it can also be set.
	/// </summary>
	public string BaseAddress { get; set; } = "/";

	/// <summary>
	/// Sets the <see cref="Environment"/> property to "Development".
	/// </summary>
	public void SetEnvironmentToDevelopment()
	{
		Environment = "Development";
	}

	/// <summary>
	/// Sets the <see cref="Environment"/> property to "Staging".
	/// </summary>
	public void SetEnvironmentToStaging()
	{
		Environment = "Staging";
	}

	/// <summary>
	/// Sets the <see cref="Environment"/> property to "Production".
	/// </summary>
	public void SetEnvironmentToProduction()
	{
		Environment = "Production";
	}

}
