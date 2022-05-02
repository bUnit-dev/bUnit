#if NET5_0_OR_GREATER
using Microsoft.AspNetCore.Components.Forms;

namespace Bunit;

public static class InputFileExtensions
{
    public static Task UploadFilesAsync(
        this IRenderedComponent<InputFile> cut,
        params InputFileContent[] files)
    {
        _ = cut ?? throw new ArgumentNullException(nameof(cut));

        var browserFiles = files.Select(f =>
        {
            var file = new BUnitBrowserFile
            {
                Name = f.Filename,
                ContentType = f.ContentType,
                LastModified = f.LastModified,
                Content = f.Content,
            };

            return file;
        });

        var args = new InputFileChangeEventArgs(browserFiles.ToArray());
		return cut.InvokeAsync(() => cut.Instance.OnChange.InvokeAsync(args));
	}
}
#endif