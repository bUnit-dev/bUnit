using AngleSharp.Dom;

namespace Bunit.RenderingV2;

public interface IRenderedComponent
{
	int ComponentId { get; }
	INodeList Nodes { get; }
}
