﻿using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Bunit.TestDoubles;

/// <summary>
/// Represents bUnit's own <see cref="SignOutSessionStateManager"/> that captures calls to <see cref="SetSignOutState"/>
/// that will help later to assert if the user was logged out
/// </summary>
#pragma warning disable CS0618
public class BunitSignOutSessionStateManager : SignOutSessionStateManager
#pragma warning restore CS0618
{
	/// <summary>
	/// Returns true when <see cref="SetSignOutState"/> was called, otherwise false
	/// </summary>
	public bool IsSignedOut { get; set; }

	/// <summary>
	/// Initializes a new instance of <see cref="BunitSignOutSessionStateManager"/>
	/// </summary>
	public BunitSignOutSessionStateManager(IJSRuntime jsRuntime) : base(jsRuntime)
	{
	}

	/// <inheritdoc />
	public override ValueTask SetSignOutState()
	{
		IsSignedOut = true;
		return new ValueTask();
	}

	/// <inheritdoc />
	public override Task<bool> ValidateSignOutState()
	{
		var wasSignedOut = IsSignedOut;
		IsSignedOut = false;
		return Task.FromResult(wasSignedOut);
	}
}
