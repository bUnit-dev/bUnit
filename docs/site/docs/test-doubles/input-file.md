---
uid: input-file
title: Uploading files to the InputFile component
---

# Uploading files to the `InputFile` component

bUnit comes with integrated support for the [`InputFile`](https://docs.microsoft.com/en-us/aspnet/core/blazor/file-uploads?view=aspnetcore-6.0&pivots=server) component and makes it easy to simulate uploading files.

To upload a file, first find the `InputFile` component in the component under test. Afterward, call the method `UploadFile`:

```csharp
// Create an InputFileContent with string content
InputFileContent fileToUpload = InputFileContent.CreateFromText("Text content", "Filename.txt");

// Render the component under test which contains the InputFile component as a child component
IRenderedComponent<ComponentUnderTest> cut = RenderComponent<ComponentUnderTest>();

// Find the InputFile component
IRenderedComponent<InputFile> inputFile = cut.FindComponent<InputFile>();

// Upload the file to upload to the InputFile component
inputFile.UploadFile(fileToUpload);

// Assertions...
```

To upload binary content, create an `InputFileContent` with the `InputFileContent.CreateFromBinary()` method.

## Known limitations
bUnit's support for the `InputFile` component is limited when uploading and resizing images (using the provided stream).

```razor
<InputFile OnChange="Upload" />
<img src="@imageBase64">

@code {
  private string imageBase64 = string.Empty;

  private async Task Upload(InputFileChangeEventArgs args)
  {
    var file = args.File;
    var preview = await file.RequestImageFileAsync("image/png", 100, 100);
    await using var stream = preview.OpenReadStream();
    var buffer = new byte[stream.Length];
    await using var memoryStream = new MemoryStream(buffer);
    await stream.CopyToAsync(memoryStream);
    var base64 = Convert.ToBase64String(buffer);
    imageBase64 = $"data:image/png;base64,{base64}";
  }
}
```

When using the `RequestImageFileAsync` method, the `UploadFiles` method will not be able to upload the file inside a test. Blazor has some internal checks, bUnit can not overcome easily. So the following test will fail:

```csharp
[Fact]
public void UploadFileTest()
{
  var cut = Render<ComponentThatUsesInputFile>();
    
  cut.FindComponent<InputFile>().UploadFiles(InputFileContent.CreateFromBinary([1,2], "test.png"));

  cut.Find("img").GetAttribute("src").Should().NotBeNullOrEmpty(); // Will fail
  Renderer.UnhandledException.Should().BeNull(); // Will fail
}
```

To work around this limitation, refactoring the logic into a service that is injected into the component and then mocking the service in the test is a possible solution.