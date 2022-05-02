﻿#if NET5_0_OR_GREATER
using Microsoft.AspNetCore.Components.Forms;

namespace Bunit;

internal class BUnitBrowserFile : IBrowserFile
{
    public Stream OpenReadStream(long maxAllowedSize = 512000, CancellationToken cancellationToken = new())
    {
        return new MemoryStream(Content);
    }

    public string Name { get; set; }
    public DateTimeOffset LastModified { get; set; }
    public long Size { get; set; }
    public string ContentType { get; set; }
    public byte[] Content { get; set; }
}
#endif