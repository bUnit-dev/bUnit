using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Bunit.TestDoubles;

/// <summary>
/// Represents bUnit's own <see cref="IWebAssemblyHostEnvironment"/> that makes the <see cref="Environment"/> and <see cref="BaseAddress"/> settable.
/// </summary>
public class BunitWebAssemblyHostEnvironment : IWebAssemblyHostEnvironment
{
	/// <summary>
	/// Gets or sets the name of the environment. Default is <c>Production</c>.
	/// </summary>
	public string Environment { get; set; } = "Production";

	/// <summary>
	/// Gets or sets the base address. Default is <c>/</c>.
	/// </summary>
	public string BaseAddress { get; set; } = "/";

	/// <summary>
	/// Sets the <see cref="Environment"/> property to <c>Development</c>.
	/// </summary>
	public void SetEnvironmentToDevelopment()
	{
		Environment = "Development";
	}

	/// <summary>
	/// Sets the <see cref="Environment"/> property to <c>Staging</c>.
	/// </summary>
	public void SetEnvironmentToStaging()
	{
		Environment = "Staging";
	}

	/// <summary>
	/// Sets the <see cref="Environment"/> property to <c>Production</c>.
	/// </summary>
	public void SetEnvironmentToProduction()
	{
		Environment = "Production";
	}
}
