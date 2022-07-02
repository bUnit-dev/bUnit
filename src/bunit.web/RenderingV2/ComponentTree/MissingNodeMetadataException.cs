namespace Bunit.RenderingV2.ComponentTree;

internal sealed class MissingNodeMetadataException : Exception
{
	public MissingNodeMetadataException() : base("There is no bUnit renderer metadata associated with this node.")
	{
	}

	public MissingNodeMetadataException(string message) : base(message)
	{

	}
}
