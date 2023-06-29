namespace Bunit.Rendering;

/// <summary>
/// Represents an render event from a <see cref="ITestRenderer"/>.
/// </summary>
public sealed class RenderEvent
{
	private readonly Dictionary<int, Status> statuses = new();

	internal IReadOnlyDictionary<int, Status> Statuses => statuses;

	/// <summary>
	/// Gets a collection of <see cref="ArrayRange{RenderTreeFrame}"/>, accessible via the ID
	/// of the component they are created by.
	/// </summary>
	public RenderTreeFrameDictionary Frames { get; } = new();

	/// <summary>
	/// Gets the render status for a <paramref name="renderedComponent"/>.
	/// </summary>
	/// <param name="renderedComponent">The <paramref name="renderedComponent"/> to get the status for.</param>
	/// <returns>A tuple of statuses indicating whether the rendered component rendered during the render cycle, if it changed or if it was disposed.</returns>
	public (bool Rendered, bool Changed, bool Disposed) GetRenderStatus(IRenderedFragmentBase renderedComponent)
	{
		if (renderedComponent is null)
			throw new ArgumentNullException(nameof(renderedComponent));

		return statuses.TryGetValue(renderedComponent.ComponentId, out var status)
			? (status.Rendered, status.Changed, status.Disposed)
			: (Rendered: false, Changed: false, Disposed: false);
	}

	internal Status GetStatus(int componentId)
	{
		if (!statuses.TryGetValue(componentId, out var status))
		{
			status = new();
			statuses[componentId] = status;
		}
		return status;
	}

	internal void SetDisposed(int componentId)
	{
		GetStatus(componentId).Disposed = true;
	}

	internal void SetUpdated(int componentId, bool hasChanges)
	{
		var status = GetStatus(componentId);
		status.Rendered = true;
		status.Changed = hasChanges;
	}

	internal void SetFramesLoaded(int componentId)
	{
		GetStatus(componentId).FramesLoaded = true;
	}

	internal void SetUpdatedApplied(int componentId)
	{
		GetStatus(componentId).UpdatesApplied = true;
	}

	internal void AddFrames(int componentId, ArrayRange<RenderTreeFrame> frames)
	{
		Frames.Add(componentId, frames);
		GetStatus(componentId).FramesLoaded = true;
	}

	internal record class Status
	{
		public bool Rendered { get; set; }

		public bool Changed { get; set; }

		public bool Disposed { get; set; }

		public bool UpdatesApplied { get; set; }

		public bool FramesLoaded { get; set; }

		public bool UpdateNeeded => Rendered || Changed;
	}
}

