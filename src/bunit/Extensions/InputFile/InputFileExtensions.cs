using Microsoft.AspNetCore.Components.Forms;

namespace Bunit;

/// <summary>
/// Extensions for the <see cref="InputFile"/> component.
/// </summary>
public static class InputFileExtensions
{
	/// <summary>
	/// Uploads multiple files and invokes the OnChange event.
	/// </summary>
	/// <param name="inputFileComponent">The <see cref="InputFile"/> component which will upload the files.</param>
	/// <param name="files">Files to upload.</param>
	public static void UploadFiles(
		this RenderedComponent<InputFile> inputFileComponent,
		params InputFileContent[] files)
	{
		ArgumentNullException.ThrowIfNull(inputFileComponent);
		ArgumentNullException.ThrowIfNull(files);
		ArgumentOutOfRangeException.ThrowIfZero(files.Length);

		var browserFiles = files.Select(file => new BUnitBrowserFile(
			file.Filename ?? string.Empty,
			file.LastModified ?? default,
			file.Size,
			file.ContentType ?? string.Empty,
			file.Content));

		var args = new InputFileChangeEventArgs(browserFiles.ToArray());
		var uploadTask = inputFileComponent.InvokeAsync(() => inputFileComponent.Instance.OnChange.InvokeAsync(args));
		if (!uploadTask.IsCompleted)
		{
			uploadTask.GetAwaiter().GetResult();
		}
	}
}
