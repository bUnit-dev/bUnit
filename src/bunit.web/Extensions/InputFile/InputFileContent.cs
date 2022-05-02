#if NET5_0_OR_GREATER
namespace Bunit;

public class InputFileContent
{
    public byte[] Content { get; set; }

    public string Filename { get; set; }
    
    public DateTimeOffset LastModified { get; set; }

    public long Size => Content?.Length ?? 0;
    
    public string ContentType { get; set; }
}
#endif