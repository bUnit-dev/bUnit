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
		var cut = Render<InputFileComponent>();
		var lastModified = new DateTime(1991, 5, 17);
		var file = InputFileContent.CreateFromText("Hello World", "Hey.txt", lastModified);

		cut.FindComponent<InputFile>().UploadFiles(file);

		cut.Instance.Content.ShouldBe("Hello World");
		cut.Instance.Filename.ShouldBe("Hey.txt");
		cut.Instance.Size.ShouldBe(11);
		cut.Instance.LastChanged.ShouldBe(lastModified);
	}

	[Fact(DisplayName = "InputFile can upload a single byte file")]
	public void Test002()
	{
		var cut = Render<InputFileComponent>();
		var file = InputFileContent.CreateFromBinary(Encoding.Default.GetBytes("Hello World"));

		cut.FindComponent<InputFile>().UploadFiles(file);

		cut.Instance.Content.ShouldBe("Hello World");
	}

	[Fact(DisplayName = "InputFile can upload multiple files")]
	public void Test003()
	{
		var cut = Render<MultipleInputFileComponent>();
		var lastModified = new DateTime(1991, 5, 17);
		var file1 = InputFileContent.CreateFromText("Hello World", "Hey.txt", lastModified, "test");
		var file2 = InputFileContent.CreateFromText("World Hey", "Test.txt", lastModified, "unit");

		cut.FindComponent<InputFile>().UploadFiles(file1, file2);

		cut.Instance.Files.Count.ShouldBe(2);
		cut.Instance.Files[0].FileContent.ShouldBe("Hello World");
		cut.Instance.Files[0].Filename.ShouldBe("Hey.txt");
		cut.Instance.Files[0].LastChanged.ShouldBe(lastModified);
		cut.Instance.Files[0].Size.ShouldBe(11);
		cut.Instance.Files[0].Type.ShouldBe("test");
		cut.Instance.Files[1].FileContent.ShouldBe("World Hey");
		cut.Instance.Files[1].Filename.ShouldBe("Test.txt");
		cut.Instance.Files[1].LastChanged.ShouldBe(lastModified);
		cut.Instance.Files[1].Size.ShouldBe(9);
		cut.Instance.Files[1].Type.ShouldBe("unit");
	}

	[Fact(DisplayName = "UploadFile throws exception when InputFile is null")]
	public void Test004()
	{
		Action action = () => ((IRenderedComponent<InputFile>)null).UploadFiles();

		action.ShouldThrow<ArgumentNullException>();
	}

	[Fact(DisplayName = "Creating InputFileContent with null text throws exception")]
	public void Test005()
	{
		Action action = () => InputFileContent.CreateFromText(null);

		action.ShouldThrow<ArgumentNullException>();
	}

	[Fact(DisplayName = "Creating InputFileContent with null binary throws exception")]
	public void Test006()
	{
		Action action = () => InputFileContent.CreateFromBinary(null);

		action.ShouldThrow<ArgumentNullException>();
	}

	[Fact(DisplayName = "Upload no files will result in Exception")]
	public void Test007()
	{
		var cut = Render<InputFileComponent>();

		Action act = () => cut.FindComponent<InputFile>().UploadFiles();

		act.ShouldThrow<ArgumentException>();
	}

	[Fact(DisplayName = "Setting up InputFile will not overwrite bUnit default")]
	public void Test008()
	{
		JSInterop.SetupVoid("Blazor._internal.InputFile.init").SetException(new Exception());
		var cut = Render<InputFileComponent>();

		Action act = () => cut.FindComponent<InputFile>().UploadFiles(InputFileContent.CreateFromText("Hello"));

		act.ShouldNotThrow();
	}

	private sealed class InputFileComponent : ComponentBase
	{
		public string? Filename { get; private set; }
		public string? Content { get; private set; }
		public DateTimeOffset? LastChanged { get; private set; }
		public long Size { get; private set; }

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenComponent<InputFile>(0);
			builder.AddAttribute(1, "OnChange", RuntimeHelpers.TypeCheck(
				EventCallback.Factory.Create<InputFileChangeEventArgs>(
					this,
					OnChange
				)));
			builder.CloseComponent();
		}

		private void OnChange(InputFileChangeEventArgs args)
		{
			var file = args.File;
			Filename = file.Name;
			LastChanged = file.LastModified;
			Size = file.Size;
			using var stream = new StreamReader(file.OpenReadStream());
			Content = stream.ReadToEnd();
		}
	}

	private sealed class MultipleInputFileComponent : ComponentBase
	{
		public readonly List<File> Files = new();

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenComponent<InputFile>(0);
			builder.AddAttribute(1, "OnChange", RuntimeHelpers.TypeCheck(
				EventCallback.Factory.Create<InputFileChangeEventArgs>(this,
					OnChange
				)));
			builder.CloseComponent();
		}

		private void OnChange(InputFileChangeEventArgs args)
		{
			foreach (var file in args.GetMultipleFiles())
			{
				using var stream = new StreamReader(file.OpenReadStream());
				var content = stream.ReadToEnd();

				var newFile = new File(file.Name, content, file.LastModified, file.Size, file.ContentType);
				Files.Add(newFile);
			}
		}

		public sealed record File(string Filename, string FileContent, DateTimeOffset LastChanged, long Size, string Type);
	}
}
