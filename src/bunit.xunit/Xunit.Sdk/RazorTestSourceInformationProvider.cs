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
	internal class RazorTestSourceInformationProvider : IDisposable
	{
		private static readonly Type[] RazorTestTypes = GetRazorTestTypes().ToArray();

		private static IEnumerable<Type> GetRazorTestTypes()
		{
			var razorTestBaseType = typeof(RazorTestBase);
			foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (!a.FullName.StartsWith("Bunit", StringComparison.Ordinal))
					continue;

				foreach (Type t in a.ExportedTypes)
				{
					if (razorTestBaseType.IsAssignableFrom(t) && !t.IsAbstract)
						yield return t;
				}
			}
		}

		private SourceFileFinder? _sourceFileFinder;

		public IMessageSink DiagnosticMessageSink { get; set; }

		public RazorTestSourceInformationProvider(IMessageSink? diagnosticMessageSink = null)
		{
			DiagnosticMessageSink = diagnosticMessageSink ?? new NullMessageSink();
		}

		public void Dispose() => _sourceFileFinder?.Dispose();

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

		private bool TryFindSourceFile(Type testComponent, [NotNullWhen(true)]out string? razorFile)
		{
			var sourceFileFinder = GetSourceFileFinderForType(testComponent);

			razorFile = null;

			foreach (var file in sourceFileFinder.Find(testComponent))
			{
				DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"{nameof(GetSourceInformation)}({testComponent.Name}): Verifying file = {file}"));

				razorFile = file;


				if (IsTestComponentFile(testComponent, file))
					break;

				if (IsGeneratedTestComponentFile(testComponent, file) && TryGetRazorFileFromGeneratedFile(file, out razorFile))
					break;
			}

			return razorFile is { };
		}

		private static bool IsTestComponentFile(Type testComponent, string file)
		{
			const string RAZOR_FILE_EXTENSION = ".razor";
			return file.EndsWith(RAZOR_FILE_EXTENSION, StringComparison.OrdinalIgnoreCase) &&
				Path.GetFileNameWithoutExtension(file).Equals(testComponent.Name, StringComparison.Ordinal);
		}

		private bool IsGeneratedTestComponentFile(Type testComponent, string file)
		{
			const string BLAZOR_GENERATED_FILE_EXTENSION = ".razor.g.cs";
			const string GENERATED_FILE_EXTENSION = ".g.cs";

			return file.EndsWith(BLAZOR_GENERATED_FILE_EXTENSION, StringComparison.OrdinalIgnoreCase)
				&& IsTestComponentFile(testComponent, file.Substring(0, file.Length - GENERATED_FILE_EXTENSION.Length));
		}

		private bool TryGetRazorFileFromGeneratedFile(string file, [NotNullWhen(true)]out string? result)
		{
			// Pattern for first line in generated files: #pragma checksum "C:\...\bunit\src\bunit.xunit.tests\SampleComponents\ComponentWithTwoTests.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b0aa9328840c75d34f073c3300621046639ea9c7"
			const string GENERATED_FILE_REF_PREFIX = "#pragma checksum \"";

			result = null;

			var line = File.ReadLines(file).FirstOrDefault() ?? string.Empty;
			if (line.StartsWith(GENERATED_FILE_REF_PREFIX, StringComparison.Ordinal))
			{
				var refFileEndIndex = line.IndexOf('"', GENERATED_FILE_REF_PREFIX.Length);
				result = line.Substring(GENERATED_FILE_REF_PREFIX.Length, refFileEndIndex - GENERATED_FILE_REF_PREFIX.Length);
			}

			return result is { };
		}

		private int? FindLineNumber(string razorFile, RazorTestBase test, int testNumber)
		{
			var testCasesSeen = 0;
			var lineNumber = 0;
			var lastTestCaseName = string.Empty;
			var testCaseName = test.GetType().Name;
			foreach (var line in File.ReadLines(razorFile))
			{
				lineNumber++;

				if (!line.StartsWith($"<", StringComparison.OrdinalIgnoreCase))
					continue;

				for (int i = 0; i < RazorTestTypes.Length; i++)
				{
					if (line.StartsWith($"<{RazorTestTypes[i].Name}", StringComparison.Ordinal))
					{
						char? nextChar = null;
						if (line.Length > RazorTestTypes[i].Name.Length + 1)
							nextChar = line[RazorTestTypes[i].Name.Length + 1];

						if (nextChar is null || nextChar == ' ' || nextChar == '>' || nextChar == '\n' || nextChar == '\r')
						{
							testCasesSeen++;
							lastTestCaseName = RazorTestTypes[i].Name;
							break;
						}
					}
				}

				if (testNumber == testCasesSeen && lastTestCaseName.Equals(testCaseName, StringComparison.Ordinal))
					return lineNumber;
				else if (testNumber < testCasesSeen)
					break;
			}
			return null;
		}

		private SourceFileFinder GetSourceFileFinderForType(Type testComponent)
		{
			if (_sourceFileFinder is null || _sourceFileFinder.SearchAssembly != testComponent.Assembly)
			{
				_sourceFileFinder?.Dispose();
				_sourceFileFinder = new SourceFileFinder(testComponent.Assembly);
			}
			return _sourceFileFinder;
		}
	}
}
