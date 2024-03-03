using System.Diagnostics;
using Bunit.Rendering;

namespace Bunit;

/// <summary>
/// Represents a rendered component.
/// </summary>
[DebuggerDisplay("Component={typeof(TComponent).Name,nq},RenderCount={RenderCount}")]
public sealed class RenderedComponent<TComponent> : RenderedFragment
	where TComponent : IComponent
{
	private TComponent? instance;

	/// <summary>
	/// Gets the component under test.
	/// </summary>
	public TComponent Instance
	{
		get
		{
			EnsureComponentNotDisposed();
			return instance ?? throw new InvalidOperationException("Component has not rendered yet...");
		}
	}

	internal RenderedComponent(int componentId, IServiceProvider services)
		: base(componentId, services) { }

	internal RenderedComponent(int componentId, TComponent instance, RenderTreeFrameDictionary componentFrames, IServiceProvider services)
		: base(componentId, services)
	{
		this.instance = instance;
		RenderCount++;
		UpdateMarkup(componentFrames);
	}

	/// <inheritdoc/>
	protected override void OnRenderInternal(RenderEvent renderEvent)
	{
		// checks if this is the first render, and if it is
		// tries to find the TComponent in the render event
		if (instance is null)
		{
			SetComponentAndID(renderEvent);
		}
	}

	private void SetComponentAndID(RenderEvent renderEvent)
	{
		if (TryFindComponent(renderEvent.Frames, ComponentId, out var id, out var component))
		{
			instance = component;
			ComponentId = id;
		}
		else
		{
			throw new InvalidOperationException("Component instance not found at expected position in render tree.");
		}
	}

	private static bool TryFindComponent(RenderTreeFrameDictionary framesCollection, int parentComponentId, out int componentId, out TComponent component)
	{
		componentId = -1;
		component = default!;

		var frames = framesCollection[parentComponentId];

		for (var i = 0; i < frames.Count; i++)
		{
			ref var frame = ref frames.Array[i];
			if (frame.FrameType == RenderTreeFrameType.Component)
			{
				if (frame.Component is TComponent c)
				{
					componentId = frame.ComponentId;
					component = c;
					return true;
				}

				if (TryFindComponent(framesCollection, frame.ComponentId, out componentId, out component))
				{
					return true;
				}
			}
		}

		return false;
	}
}
