using AngleSharp.Dom;

namespace Bunit.RenderingPort;

public interface IRenderedComponent
{
	INodeList Nodes { get; }
}
