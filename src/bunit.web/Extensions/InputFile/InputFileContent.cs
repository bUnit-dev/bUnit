using System.Text;

#if NET5_0_OR_GREATER
namespace Bunit;

/// <summary>
/// Represents a file which can be uploaded.
/// </summary>
public class InputFileContent
{
    private InputFileContent(byte[] content, string? filename, DateTimeOffset? lastModified, string? contentType)
    {
        Content = content;
        Filename = filename;
        LastModified = lastModified;
        ContentType = contentType;
    }
    
    internal byte[] Content { get; set; }

    internal string? Filename { get; set; }
    
    internal DateTimeOffset? LastModified { get; set; }

    internal long Size => Content.Length;
    
    internal string? ContentType { get; set; }

    /// <summary>
    /// Creates an <see cref="InputFileContent"/> instance which has string content.
    /// </summary>
    /// <param name="fileContent">The string content.</param>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="lastChanged">The last modified date of the file.</param>
    /// <param name="contentType">The mime type of the file.</param>
    public static InputFileContent CreateFromText(
        string fileContent,
        string? fileName = null,
        DateTimeOffset? lastChanged = null,
        string? contentType = null)
    {
        if (fileContent == null)
            throw new ArgumentNullException(nameof(fileContent));
        
        return new InputFileContent(Encoding.Default.GetBytes(fileContent), fileName, lastChanged, contentType);
    }

    /// <summary>
    /// Creates an <see cref="InputFileContent"/> instance which has binary content.
    /// </summary>
    /// <param name="fileContent">The binary content.</param>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="lastChanged">The last modified date of the file.</param>
    /// <param name="contentType">The mime type of the file.</param>
    public static InputFileContent CreateFromBinary(
        byte[] fileContent,
        string? fileName = null,
        DateTimeOffset? lastChanged = null,
        string? contentType = null)
    {
        if (fileContent == null)
            throw new ArgumentNullException(nameof(fileContent));
        
        return new InputFileContent(fileContent, fileName, lastChanged, contentType);
    }
}
#endif