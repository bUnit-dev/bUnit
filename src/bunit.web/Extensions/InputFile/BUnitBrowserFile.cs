#if NET5_0_OR_GREATER
using Microsoft.AspNetCore.Components.Forms;

namespace Bunit;

internal class BUnitBrowserFile : IBrowserFile
{
    public BUnitBrowserFile(
        string name,
        DateTimeOffset lastModified,
        long size,
        string contentType,
        byte[] content)
    {
        Name = name;
        LastModified = lastModified;
        Size = size;
        ContentType = contentType;
        Content = content;
    }

    public string Name { get; set; }
    public DateTimeOffset LastModified { get; set; }
    public long Size { get; set; }
    public string ContentType { get; set; }
    public byte[] Content { get; set; }

    public Stream OpenReadStream(long maxAllowedSize = 512000, CancellationToken cancellationToken = default)
    {
        return new MemoryStream(Content);
    }
}
#endif