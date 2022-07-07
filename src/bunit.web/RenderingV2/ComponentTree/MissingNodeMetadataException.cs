namespace Bunit.RenderingV2.ComponentTree;

#pragma warning disable S3871 // Exception types should be "public"
internal sealed class MissingNodeMetadataException : Exception
#pragma warning restore S3871 // Exception types should be "public"
{
	public MissingNodeMetadataException() : base("There is no bUnit renderer metadata associated with this node.")
	{
	}

	public MissingNodeMetadataException(string message) : base(message)
	{

	}
}
