#if NET5_0_OR_GREATER
using Microsoft.AspNetCore.Components.Forms;

namespace Bunit;

internal class BUnitBrowserFile : IBrowserFile
{
    public string Name { get; set; } = default!;
    public DateTimeOffset LastModified { get; set; }
    public long Size { get; set; }
    public string ContentType { get; set; } = default!;
    public byte[] Content { get; set; } = default!;

    public Stream OpenReadStream(long maxAllowedSize = 512000, CancellationToken cancellationToken = default)
    {
        return new MemoryStream(Content);
    }
}
#endif