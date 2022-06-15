using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Infrastructure;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.DependencyInjection;

namespace Bunit.TestDoubles;

/// <summary>
/// Represents a fake <see cref="PersistentComponentState"/>, that can be used to the
/// real <see cref="PersistentComponentState"/> in the Blazor runtime.
/// </summary>
public sealed class FakePersistentComponentState
{
	private static readonly JsonSerializerOptions JsonSerializerOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		PropertyNameCaseInsensitive = true,
	};
	private readonly FakePersistentComponentStateStore store;
	private readonly Lazy<ComponentStatePersistenceManager> manager;
	private readonly Lazy<Renderer> renderer;

	/// <summary>
	/// Initializes a new instance of the <see cref="FakePersistentComponentState"/> class.
	/// </summary>
	/// <param name="services">The <see cref="IServiceProvider"/> to pull lazily dependencies from.</param>
	internal FakePersistentComponentState(IServiceProvider services)
	{
		store = new FakePersistentComponentStateStore();
		manager = new Lazy<ComponentStatePersistenceManager>(() => services.GetRequiredService<ComponentStatePersistenceManager>());
		renderer = new Lazy<Renderer>(() => services.GetRequiredService<Renderer>());
	}

	/// <summary>
	/// Triggers any listeners registered to the <see cref="PersistentComponentState.RegisterOnPersisting(Func{Task})"/> method.
	/// Use this method to emulate the Blazor runtime invoking the <c>OnPersisting</c> callbacks during
	/// component persistence.
	/// </summary>
	/// <remarks>
	/// Only call this method after all services has been registered with the <see cref="TestContextBase.Services"/>.
	/// Calling this method will lookup dependencies of the <see cref="FakePersistentComponentState"/>
	/// from the <see cref="TestServiceProvider"/>, which means no other services can be registered after this point.
	/// </remarks>
	public void TriggerOnPersisting()
	{
		manager.Value.PersistStateAsync(store, renderer.Value);
		manager.Value.RestoreStateAsync(store);
	}

	/// <summary>
	/// Persists <paramref name="instance"/> under the given <paramref name="key"/> in the store.
	/// </summary>
	/// <remarks>
	/// Only call this method after all services has been registered with the <see cref="TestContextBase.Services"/>.
	/// Calling this method will lookup dependencies of the <see cref="FakePersistentComponentState"/>
	/// from the <see cref="TestServiceProvider"/>, which means no other services can be registered after this point.
	/// </remarks>
	/// <typeparam name="TValue">The <paramref name="instance"/> type.</typeparam>
	/// <param name="key">The key to use to persist the state.</param>
	/// <param name="instance">The instance to persist.</param>
	public void Persist<TValue>(string key, TValue instance)
	{
		if (key is null)
			throw new ArgumentNullException(nameof(key));

		store.Add(key, JsonSerializer.SerializeToUtf8Bytes(instance, JsonSerializerOptions));
		manager.Value.RestoreStateAsync(store);
	}

	/// <summary>
	/// Tries to retrieve the persisted state with the given <paramref name="key"/>.
	/// When the key is present, the state is successfully returned via <paramref name="instance"/>.
	/// </summary>
	/// <remarks>
	/// Only call this method after all services has been registered with the <see cref="TestContextBase.Services"/>.
	/// Calling this method will lookup dependencies of the <see cref="FakePersistentComponentState"/>
	/// from the <see cref="TestServiceProvider"/>, which means no other services can be registered after this point.
	/// </remarks>
	/// <param name="key">The key used to persist the instance.</param>
	/// <param name="instance">The persisted instance.</param>
	/// <returns><c>true</c> if the state was found; <c>false</c> otherwise.</returns>
	public bool TryTake<TValue>(string key, [MaybeNullWhen(false)] out TValue? instance)
		=> manager.Value.State.TryTakeFromJson<TValue>(key, out instance);
}
