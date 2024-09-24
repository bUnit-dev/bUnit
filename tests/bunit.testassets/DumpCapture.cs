using System.Diagnostics;
using System.Runtime.CompilerServices;
using Xunit.Abstractions;

namespace Bunit.TestAssets;

/// <summary>
/// Wrap a test action or function in a try-catch block that captures a dump file if the test fails.
/// </summary>
/// <remarks>
/// This requires the <c>dotnet-dump</c> tool to be installed as a local dotnet tool.
/// </remarks>
public static class DumpCapture
{

	public static async Task OnFailureAsync(
		Action testAction,
		ITestOutputHelper outputHelper,
		[CallerMemberName] string testName = "",
		[CallerFilePath] string testFilePath = "")
	{
		try
		{
			testAction();
		}
		catch
		{
			await CaptureDump(testName, testFilePath, outputHelper);
			throw;
		}
	}

	public static async Task OnFailureAsync(
		Func<Task> testAction,
		ITestOutputHelper outputHelper,
		[CallerMemberName] string testName = "",
		[CallerFilePath] string testFilePath = "")
	{
		try
		{
			await testAction();
		}
		catch
		{
			await CaptureDump(testName, testFilePath, outputHelper);
			throw;
		}
	}

	private static async Task CaptureDump(string testName, string testFilePath, ITestOutputHelper outputHelper)
	{
#if NETSTANDARD2_1
		var processId = Process.GetCurrentProcess().Id;
#else
		var processId = Environment.ProcessId;
#endif
		var dumpFilePath = Path.Combine(Directory.GetCurrentDirectory(), $"{Path.GetFileNameWithoutExtension(testFilePath)}-{testName}-wait-failed-{Guid.NewGuid()}.dmp");
		// Attempt to start the dotnet-dump process
		var startInfo = new ProcessStartInfo
		{
			FileName = "dotnet",
			Arguments = $"dotnet-dump collect -p {processId} -o {dumpFilePath}",
			RedirectStandardOutput = true,
			RedirectStandardError = true,
			UseShellExecute = false,
			CreateNoWindow = true
		};
		using var process = Process.Start(startInfo);
		if (process is null)
		{
			outputHelper.WriteLine(" Failed to start dotnet-dump process.");
			return;
		}

#if NETSTANDARD2_1
		process.WaitForExit();
#else
		await process.WaitForExitAsync();
#endif
		var output = await process.StandardOutput.ReadToEndAsync();
		var error = await process.StandardError.ReadToEndAsync();
		outputHelper.WriteLine($"Dump status: {{process.ExitCode}}. Dump file: {dumpFilePath}");
		if (!string.IsNullOrWhiteSpace(output))
		{
			outputHelper.WriteLine($"Dump output: {output}");
		}
		if (!string.IsNullOrWhiteSpace(error))
		{
			outputHelper.WriteLine($"Dump error: {error}");
		}
	}
}
