namespace Bunit.RenderingV2.ComponentTree;

internal record class NodeMetadata
{
	public int FrameId { get; set; }

	public int ComponentId { get; init; }
}
