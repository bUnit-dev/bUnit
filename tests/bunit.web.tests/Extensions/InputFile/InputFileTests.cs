﻿#if NET5_0_OR_GREATER
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Forms;

namespace Bunit.Extensions.InputFile;

using InputFile = Microsoft.AspNetCore.Components.Forms.InputFile;

public class InputFileTests : TestContext
{
    [Fact(DisplayName = "InputFile can upload a single string file")]
    public void Test001()
    {
        var cut = RenderComponent<InputFileComponent>();
        var lastModified = new DateTime(1991, 5, 17);
        var file = InputFileContent.CreateFromText("Hello World", "Hey.txt", lastModified);
        
        cut.FindComponent<InputFile>().UploadFiles(file);
        
        cut.Find("#filename").TextContent.ShouldBe("Hey.txt");
        cut.Find("#content").TextContent.ShouldBe("Hello World");
        cut.Find("#changed").TextContent.ShouldBe("17/05/1991");
        cut.Find("#size").TextContent.ShouldBe("11");
    }
    
    [Fact(DisplayName = "InputFile can upload a single byte file")]
    public void Test002()
    {
        var cut = RenderComponent<InputFileComponent>();
        var file = InputFileContent.CreateFromBinary(Encoding.Default.GetBytes("Hello World"));
        
        cut.FindComponent<InputFile>().UploadFiles(file);
        
        cut.Find("#content").TextContent.ShouldBe("Hello World");
    }

    [Fact(DisplayName = "InputFile can upload multiple files")]
    public void Test003()
    {
        var cut = RenderComponent<MultipleInputFileComponent>();
        var lastModified = new DateTime(1991, 5, 17);
        var file1 = InputFileContent.CreateFromText("Hello World", "Hey.txt", lastModified, "test");
        var file2 = InputFileContent.CreateFromText("World Hey", "Test.txt", lastModified, "unit");
        
        cut.FindComponent<InputFile>().UploadFiles(file1, file2);
        
        cut.Find("#item-0-filename").TextContent.ShouldBe("Hey.txt");
        cut.Find("#item-0-content").TextContent.ShouldBe("Hello World");
        cut.Find("#item-0-lastchanged").TextContent.ShouldBe("17/05/1991");
        cut.Find("#item-0-size").TextContent.ShouldBe("11");
        cut.Find("#item-0-type").TextContent.ShouldBe("test");
        cut.Find("#item-1-filename").TextContent.ShouldBe("Test.txt");
        cut.Find("#item-1-content").TextContent.ShouldBe("World Hey");
        cut.Find("#item-1-lastchanged").TextContent.ShouldBe("17/05/1991");
        cut.Find("#item-1-size").TextContent.ShouldBe("9");
        cut.Find("#item-1-type").TextContent.ShouldBe("unit");
    }
    
    private class InputFileComponent : ComponentBase
    {
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "p");
            builder.AddAttribute(1, "id", "filename");
            builder.AddContent(2, filename);
            builder.CloseElement();
            builder.AddMarkupContent(3, "\r\n");
            builder.OpenElement(4, "p");
            builder.AddAttribute(5, "id", "content");
            builder.AddContent(6, content);
            
            builder.CloseElement();
            builder.AddMarkupContent(7, "\r\n");
            builder.OpenElement(8, "p");
            builder.AddAttribute(9, "id", "changed");
            builder.AddContent(10, lastChanged);
            
            builder.CloseElement();
            builder.AddMarkupContent(11, "\r\n");
            builder.OpenElement(12, "p");
            builder.AddAttribute(13, "id", "size");
            builder.AddContent(14, size);

            builder.CloseElement();
            builder.AddMarkupContent(15, "\r\n");
            builder.OpenComponent<InputFile>(16);
            builder.AddAttribute(17, "OnChange", RuntimeHelpers.TypeCheck(
                EventCallback.Factory.Create<InputFileChangeEventArgs>(
                    this,
                    OnChange
                )));
            builder.CloseComponent();
        }

        private string filename = string.Empty;
        private string content = string.Empty;
        private string lastChanged = string.Empty;
        private string size = string.Empty;
        private void OnChange(InputFileChangeEventArgs args)
        {
            var file = args.File;
            filename = file.Name;
            lastChanged = file.LastModified.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            size = file.Size.ToString(CultureInfo.InvariantCulture);
            using var stream = new StreamReader(file.OpenReadStream());
            content = stream.ReadToEnd();
        }
    }
    
    private class MultipleInputFileComponent : ComponentBase
    {
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            for (var i = 0; i < files.Count; i++)
            {
                builder.OpenElement(0, "p");
                builder.AddAttribute(1, "id", "item-" + i + "-filename");
                builder.AddContent(2, files[i].Filename);

                builder.CloseElement();
                builder.AddMarkupContent(3, "\r\n    ");
                builder.OpenElement(4, "p");
                builder.AddAttribute(5, "id", "item-" + i + "-content");
                builder.AddContent(6, files[i].FileContent);

                builder.CloseElement();
                builder.AddMarkupContent(7, "\r\n    ");
                builder.OpenElement(8, "p");
                builder.AddAttribute(9, "id", "item-" + i + "-lastchanged");
                builder.AddContent(10, files[i].LastChanged);

                builder.CloseElement();
                builder.AddMarkupContent(11, "\r\n    ");
                builder.OpenElement(12, "p");
                builder.AddAttribute(13, "id", "item-" + i + "-size");
                builder.AddContent(14, files[i].Size);

                builder.CloseElement();
                builder.AddMarkupContent(15, "\r\n    ");
                builder.OpenElement(16, "p");
                builder.AddAttribute(17, "id", "item-" + i + "-type");
                builder.AddContent(18, files[i].Type);

                builder.CloseElement();
            }

            builder.OpenComponent<InputFile>(19);
            builder.AddAttribute(20, "OnChange", RuntimeHelpers.TypeCheck(
                EventCallback.Factory.Create<InputFileChangeEventArgs>(this,
                    OnChange
                )));
            builder.CloseComponent();
        }

        private readonly List<File> files = new();

        private void OnChange(InputFileChangeEventArgs args)
        {
            foreach (var file in args.GetMultipleFiles())
            {
                using var stream = new StreamReader(file.OpenReadStream());
                var content = stream.ReadToEnd();

                var lastChanged = file.LastModified.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                var fileSize = file.Size.ToString(CultureInfo.InvariantCulture);
                var newFile = new File(file.Name, content, lastChanged, fileSize, file.ContentType);
                files.Add(newFile);
            }
        }

        private record File(string Filename, string FileContent, string LastChanged, string Size, string Type);
    }
}
#endif