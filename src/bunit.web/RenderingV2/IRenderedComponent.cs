using AngleSharp.Dom;

namespace Bunit.RenderingV2;

public interface IRenderedComponent
{
	int ComponentId { get; }
	int RenderCount { get; }
	INodeList Nodes { get; }
	IReadOnlyList<IRenderedComponent<IComponent>> Children { get; }

	internal void ApplyEdits(RenderTreeDiff updatedComponent, RenderBatch renderBatch, int renderBatchId);
}
