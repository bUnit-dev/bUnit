#if NET5_0_OR_GREATER
using System.Text;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Forms;

namespace Bunit.Extensions.InputFile;

using InputFile = Microsoft.AspNetCore.Components.Forms.InputFile;

public class InputFileASyncTests : TestContext
{
	[Fact(DisplayName = "InputFile can upload a single string file")]
	[Trait("Category", "async")]
	public async Task Test001()
	{
		var cut = RenderComponent<InputFileComponent>();
		var lastModified = new DateTime(1991, 5, 17);
		var file = InputFileContent.CreateFromText("Hello World", "Hey.txt", lastModified);

		await cut.FindComponent<InputFile>().UploadFilesAsync(file);

		cut.Instance.Content.ShouldBe("Hello World");
		cut.Instance.Filename.ShouldBe("Hey.txt");
		cut.Instance.Size.ShouldBe(11);
		cut.Instance.LastChanged.ShouldBe(lastModified);
	}

	[Fact(DisplayName = "InputFile can upload a single byte file")]
	[Trait("Category", "async")]
	public async Task Test002()
	{
		var cut = RenderComponent<InputFileComponent>();
		var file = InputFileContent.CreateFromBinary(Encoding.Default.GetBytes("Hello World"));

		await cut.FindComponent<InputFile>().UploadFilesAsync(file);

		cut.Instance.Content.ShouldBe("Hello World");
	}

	[Fact(DisplayName = "InputFile can upload multiple files")]
	[Trait("Category", "async")]
	public async Task Test003()
	{
		var cut = RenderComponent<MultipleInputFileComponent>();
		var lastModified = new DateTime(1991, 5, 17);
		var file1 = InputFileContent.CreateFromText("Hello World", "Hey.txt", lastModified, "test");
		var file2 = InputFileContent.CreateFromText("World Hey", "Test.txt", lastModified, "unit");

		await cut.FindComponent<InputFile>().UploadFilesAsync(file1, file2);

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
	[Trait("Category", "async")]
	public async Task Test004()
	{
		Func<Task> action = async () => await ((IRenderedComponent<InputFile>)null).UploadFilesAsync();

		await action.ShouldThrowAsync<ArgumentNullException>();
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
	[Trait("Category", "async")]
	public async Task Test007()
	{
		var cut = RenderComponent<InputFileComponent>();

		Func<Task> act = async () => await cut.FindComponent<InputFile>().UploadFilesAsync();

		await act.ShouldThrowAsync<ArgumentException>();
	}

	[Fact(DisplayName = "Setting up InputFile will not overwrite bUnit default")]
	[Trait("Category", "async")]
	public async Task Test008()
	{
		JSInterop.SetupVoid("Blazor._internal.InputFile.init").SetException(new Exception());
		var cut = RenderComponent<InputFileComponent>();

		await cut.FindComponent<InputFile>().UploadFilesAsync(InputFileContent.CreateFromText("Hello"));

		cut.Instance.Content.ShouldNotBeNull();
	}

	private class InputFileComponent : ComponentBase
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

	private class MultipleInputFileComponent : ComponentBase
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

		public record File(string Filename, string FileContent, DateTimeOffset LastChanged, long Size, string Type);
	}
}
#endif
