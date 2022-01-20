using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Bunit.TestDoubles;

/// <summary>
/// Represents a fake <see cref="SignOutSessionStateManager"/> that captures calls to
/// </summary>
public class FakeSignOutSessionStateManager : SignOutSessionStateManager
{
	/// <summary>
	/// Returns true when <see cref="SetSignOutState"/> was called, otherwise false
	/// </summary>
	public bool IsSignedOut { get; set; }

	/// <summary>
	/// Initializes a new instance of <see cref="FakeSignOutSessionStateManager"/>
	/// </summary>
	public FakeSignOutSessionStateManager(IJSRuntime jsRuntime) : base(jsRuntime)
	{
	}

	/// <inheritdoc />
	public override ValueTask SetSignOutState()
	{
		IsSignedOut = true;
		return new ValueTask();
	}

	/// <inheritdoc />
	public override Task<bool> ValidateSignOutState() => Task.FromResult(IsSignedOut);
}
