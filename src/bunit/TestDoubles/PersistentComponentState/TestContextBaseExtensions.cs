using Microsoft.AspNetCore.Components.Infrastructure;

namespace Bunit.TestDoubles;

/// <summary>
/// Extensions related to <see cref="BunitPersistentComponentState"/>.
/// </summary>
public static class TestContextBaseExtensions
{
	/// <summary>
	/// Adds and returns a <see cref="BunitPersistentComponentState"/> to the services of the <paramref name="testContext"/>.
	/// </summary>
	/// <param name="testContext">The test context to add the <see cref="BunitPersistentComponentState"/> to.</param>
	/// <returns>The added <see cref="BunitPersistentComponentState"/>.</returns>
	public static BunitPersistentComponentState AddBunitPersistentComponentState(this TestContextBase testContext)
	{
		ArgumentNullException.ThrowIfNull(testContext);

		testContext.Services.AddSingleton<ComponentStatePersistenceManager>();
		testContext.Services.AddSingleton<PersistentComponentState>(s => s.GetRequiredService<ComponentStatePersistenceManager>().State);
		return new BunitPersistentComponentState(testContext.Services);
	}
}
