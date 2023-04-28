using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Bunit.TestDoubles;

/// <summary>
/// Extensions related to <see cref="FakePersistentComponentState"/>.
/// </summary>
public static class TestContextBaseExtensions
{
	/// <summary>
	/// Adds and returns a <see cref="FakePersistentComponentState"/> to the services of the <paramref name="testContext"/>.
	/// </summary>
	/// <param name="testContext">The test context to add the <see cref="FakePersistentComponentState"/> to.</param>
	/// <returns>The added <see cref="FakePersistentComponentState"/>.</returns>
	public static FakePersistentComponentState AddFakePersistentComponentState(this TestContextBase testContext)
	{
		ArgumentNullException.ThrowIfNull(testContext);

		testContext.Services.AddSingleton<ComponentStatePersistenceManager>();
		testContext.Services.AddSingleton<PersistentComponentState>(s => s.GetRequiredService<ComponentStatePersistenceManager>().State);
		return new FakePersistentComponentState(testContext.Services);
	}
}
