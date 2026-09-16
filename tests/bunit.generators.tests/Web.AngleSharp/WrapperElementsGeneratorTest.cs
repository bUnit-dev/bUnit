using AngleSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Bunit.Web.AngleSharp;

public class WrapperElementsGeneratorTest
{
	private const string SnapshotPrefix = "WrapperElementsGeneratorTest.Generator#";
	private const string SnapshotSuffix = ".verified.cs";

	[Fact]
	public void Generator()
	{
		var inputCompilation = CreateCompilation();
		var generator = new WrapperElementsGenerator();

		GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
		driver = driver.RunGenerators(inputCompilation, Xunit.TestContext.Current.CancellationToken);

		var result = driver.GetRunResult();
		Assert.Empty(result.Diagnostics);
		var run = Assert.Single(result.Results);
		Assert.Null(run.Exception);

		var changed = UpdateSnapshots(run.GeneratedSources);
		Assert.True(changed.Count == 0,
			$"Snapshots were updated, review and commit the changes:{Environment.NewLine}{string.Join(Environment.NewLine, changed)}");
	}

	private static List<string> UpdateSnapshots(IEnumerable<GeneratedSourceResult> sources, [CallerFilePath] string testFilePath = "")
	{
		using var mutex = new Mutex(initiallyOwned: false, @"Global\bunit.generators.tests.WrapperElementsSnapshots");
		try
		{
			if (!mutex.WaitOne(TimeSpan.FromMinutes(1)))
				throw new TimeoutException("Timed out waiting for the snapshot lock.");
		}
		catch (AbandonedMutexException)
		{
		}

		try
		{
			return CompareAndUpdateSnapshots(sources, Path.GetDirectoryName(testFilePath)!);
		}
		finally
		{
			mutex.ReleaseMutex();
		}
	}

	private static List<string> CompareAndUpdateSnapshots(IEnumerable<GeneratedSourceResult> sources, string directory)
	{
		var changed = new List<string>();
		var expectedFiles = new HashSet<string>(StringComparer.Ordinal);

		foreach (var source in sources)
		{
			var fileName = SnapshotPrefix + Path.GetFileNameWithoutExtension(source.HintName) + SnapshotSuffix;
			var path = Path.Combine(directory, fileName);
			expectedFiles.Add(fileName);

			var actual = Normalize($"//HintName: {source.HintName}\n{source.SourceText}");
			var existing = File.Exists(path) ? Normalize(File.ReadAllText(path)) : null;
			if (existing == actual)
				continue;

			File.WriteAllText(path, actual, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
			changed.Add(fileName);
		}

		foreach (var stale in Directory.EnumerateFiles(directory, SnapshotPrefix + "*" + SnapshotSuffix)
			.Select(Path.GetFileName)
			.Where(fileName => !expectedFiles.Contains(fileName!)))
		{
			File.Delete(Path.Combine(directory, stale!));
			changed.Add(stale!);
		}

		return changed;
	}

	private static string Normalize(string text) => text.Replace("\r\n", "\n", StringComparison.Ordinal);

	private static Compilation CreateCompilation()
	{
		return CSharpCompilation.Create(
			assemblyName: "compilation",
			syntaxTrees: null,
			references: new[]
			{
				MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location),
				MetadataReference.CreateFromFile(typeof(BrowsingContext).Assembly.Location),
			},
			options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
	}
}
