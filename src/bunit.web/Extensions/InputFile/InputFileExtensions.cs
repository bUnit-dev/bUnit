#if NET5_0_OR_GREATER
using System.Runtime.ExceptionServices;
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
		this IRenderedComponent<InputFile> inputFileComponent,
		params InputFileContent[] files)
	{
		if (inputFileComponent == null)
			throw new ArgumentNullException(nameof(inputFileComponent));

		if (!files.Any())
			throw new ArgumentException("No files were provided to be uploaded.", nameof(files));

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

		if (uploadTask.Exception is { InnerException: not null } e)
		{
			ExceptionDispatchInfo.Capture(e.InnerException).Throw();
		}
	}
}
#endif
