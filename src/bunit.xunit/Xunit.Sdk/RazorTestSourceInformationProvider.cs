using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using Bunit.RazorTesting;
using ReflectionHelpers;
using Xunit.Abstractions;

namespace Xunit.Sdk
{
	internal sealed class RazorTestSourceInformationProvider : IDisposable
	{
		private static readonly Type[] RazorTestTypes = GetRazorTestTypes().ToArray();

		private static IEnumerable<Type> GetRazorTestTypes()
		{
			var razorTestBaseType = typeof(RazorTestBase);
			foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
			{
				var name = a.FullName ?? string.Empty;
				if (!name.StartsWith("Bunit", StringComparison.Ordinal))
					continue;

				foreach (Type t in a.ExportedTypes)
				{
					if (razorTestBaseType.IsAssignableFrom(t) && !t.IsAbstract)
						yield return t;
				}
			}
		}

		private SourceFileFinder? sourceFileFinder;

		public IMessageSink DiagnosticMessageSink { get; set; }

		public RazorTestSourceInformationProvider(IMessageSink? diagnosticMessageSink = null)
		{
			DiagnosticMessageSink = diagnosticMessageSink ?? new NullMessageSink();
		}

		public void Dispose() => sourceFileFinder?.Dispose();

		[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
		public ISourceInformation? GetSourceInformation(Type testComponent, RazorTestBase test, int testNumber)
		{
			DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"{nameof(GetSourceInformation)}({testComponent.Name}): Attempting to find source file"));

			try
			{
				if (TryFindSourceFile(testComponent, out var razorFile))
				{
					var lineNumber = FindLineNumber(razorFile, test, testNumber);

					DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"{nameof(GetSourceInformation)}({testComponent.Name}): Source info found: File = {razorFile}, LineNumber = {lineNumber}"));

					return new SourceInformation { FileName = razorFile, LineNumber = lineNumber ?? 1 };
				}
			}
			catch (Exception ex)
			{
				DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"{nameof(GetSourceInformation)}({testComponent.Name}): Failed to find source information. Exception message: " +
					$"{ex.Message}{Environment.NewLine}{ex.StackTrace}"));
			}

			return null;
		}

		private bool TryFindSourceFile(Type testComponent, [NotNullWhen(true)] out string? razorFile)
		{
			var finder = GetSourceFileFinderForType(testComponent);

			razorFile = null;

			foreach (var file in finder.Find(testComponent))
			{
				DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"{nameof(GetSourceInformation)}({testComponent.Name}): Verifying file = {file}"));

				razorFile = file;

				if (IsTestComponentFile(testComponent, file))
					break;

				if (IsGeneratedTestComponentFile(testComponent, file) && TryGetRazorFileFromGeneratedFile(file, out razorFile))
					break;
			}

			return razorFile is not null;
		}

		private SourceFileFinder GetSourceFileFinderForType(Type testComponent)
		{
			if (sourceFileFinder is null || sourceFileFinder.SearchAssembly != testComponent.Assembly)
			{
				sourceFileFinder?.Dispose();
				sourceFileFinder = new SourceFileFinder(testComponent.Assembly);
			}

			return sourceFileFinder;
		}

		private static bool IsTestComponentFile(Type testComponent, string file)
		{
			const string RAZOR_FILE_EXTENSION = ".razor";
			return file.EndsWith(RAZOR_FILE_EXTENSION, StringComparison.OrdinalIgnoreCase) &&
				Path.GetFileNameWithoutExtension(file).Equals(testComponent.Name, StringComparison.Ordinal);
		}

		private static bool IsGeneratedTestComponentFile(Type testComponent, string file)
		{
			const string BLAZOR_GENERATED_FILE_EXTENSION = ".razor.g.cs";
			const string GENERATED_FILE_EXTENSION = ".g.cs";

			return file.EndsWith(BLAZOR_GENERATED_FILE_EXTENSION, StringComparison.OrdinalIgnoreCase)
				&& IsTestComponentFile(testComponent, file.Substring(0, file.Length - GENERATED_FILE_EXTENSION.Length));
		}

		[SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "Pretty sure this is an analyzer bug.")]
		private static bool TryGetRazorFileFromGeneratedFile(string file, [NotNullWhen(true)] out string? result)
		{
			// Pattern for first line in generated files: #pragma checksum "C:\...\bunit\src\bunit.xunit.tests\SampleComponents\ComponentWithTwoTests.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b0aa9328840c75d34f073c3300621046639ea9c7"
			const string GENERATED_FILE_REF_PREFIX = "#pragma checksum \"";

			result = null;

			var line = File.ReadLines(file).FirstOrDefault() ?? string.Empty;
			if (line.StartsWith(GENERATED_FILE_REF_PREFIX, StringComparison.Ordinal))
			{
				var refFileEndIndex = line.IndexOf('"', GENERATED_FILE_REF_PREFIX.Length);
				result = line[GENERATED_FILE_REF_PREFIX.Length..refFileEndIndex];
			}

			return result is not null;
		}

		private static int? FindLineNumber(string razorFile, RazorTestBase test, int testNumber)
		{
			var testCasesSeen = 0;
			var lineNumber = 0;
			var lastTestCaseName = string.Empty;
			var testCaseName = test.GetType().Name;

			foreach (var line in File.ReadLines(razorFile))
			{
				lineNumber++;

				if (!StartsWithTagStart(line))
					continue;

				if (TryFindRazorTestComponent(line, out var componentName))
				{
					testCasesSeen++;
					lastTestCaseName = componentName;
				}

				if (testNumber == testCasesSeen && lastTestCaseName.Equals(testCaseName, StringComparison.Ordinal))
					return lineNumber;

				if (testNumber < testCasesSeen)
					break;
			}

			return null;
		}

		private static bool StartsWithTagStart(string line) => line.StartsWith($"<", StringComparison.OrdinalIgnoreCase);

		private static bool TryFindRazorTestComponent(string line, [NotNullWhen(true)] out string? testComponentName)
		{
			testComponentName = default;

			for (int i = 0; i < RazorTestTypes.Length; i++)
			{
				if (StartsWithRazorTestComponent(line, RazorTestTypes[i].Name))
				{
					testComponentName = RazorTestTypes[i].Name;
					return true;
				}
			}

			return false;
		}

		private static bool StartsWithRazorTestComponent(string line, string testComponentName)
		{
			if (!line.StartsWith($"<{testComponentName}", StringComparison.Ordinal))
				return false;

			char? nextChar = line.Length > testComponentName.Length + 1
				? line[testComponentName.Length + 1]
				: null;

			return nextChar is null || nextChar == ' ' || nextChar == '>' || nextChar == '\n' || nextChar == '\r';
		}
	}
}
