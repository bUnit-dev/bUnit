namespace Bunit.TestDoubles;

internal class FakePersistentComponentStateStore : IPersistentComponentStateStore
{
	private readonly Dictionary<string, byte[]> state = new(StringComparer.Ordinal);

	public void Add(string key, byte[] value)
	{
		state[key] = value;
	}

	public Task<IDictionary<string, byte[]>> GetPersistedStateAsync()
		=> Task.FromResult<IDictionary<string, byte[]>>(state);

	public Task PersistStateAsync(IReadOnlyDictionary<string, byte[]> state)
	{
		foreach (var (key, value) in state)
		{
			Add(key, value);
		}

		return Task.CompletedTask;
	}
}
