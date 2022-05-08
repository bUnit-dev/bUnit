---
uid: input-file
title: Uploading a file via InputFile
---

# Uploading a file via InputFile

bUnit comes with integrated support for the [`InputFile`](https://docs.microsoft.com/en-us/aspnet/core/blazor/file-uploads?view=aspnetcore-6.0&pivots=server) component and makes it easy to simulate uploading files.

To upload a file, first find the `InputFile` component in the component under test. Afterwards call the extension method `UploadFile`:
```csharp
using var ctx = new TestContext();
var cut = ctx.RenderComponent<ComponentUnderTest>();
var fileToUpload = InputFileContent.CreateFromText("Text content", "Filename.txt");

cut.FindComponent<InputFile>().UploadFile(fileToUpload);
```

Also binary content is supported via `InputFileContent.CreateFromBinary(...);`. For the full overview of supported properties head over to this [page](xref:Bunit.InputFileContent).