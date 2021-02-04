using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Xunit.Sdk
{
	internal sealed class RazorTestCase : LongLivedMarshalByRefObject, IXunitTestCase
	{
		private string? uniqueId;

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Called by the deserializer; should only be called by deriving classes for de-serialization purposes")]
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
		public RazorTestCase() { }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

		public RazorTestCase(string displayName, TimeSpan? timeout, string? skipReason, int testNumber, ITestMethod testMethod, ISourceInformation? sourceInformation = null)
		{
			TestMethod = testMethod;
			Method = testMethod.Method;
			DisplayName = displayName;
			Timeout = timeout is null
				? 0
				: Convert.ToInt32(timeout.Value.TotalMilliseconds, CultureInfo.InvariantCulture);
			SkipReason = skipReason;
			TestNumber = testNumber;
			SourceInformation = sourceInformation;
		}

		/// <inheritdoc/>
		public Exception? InitializationException { get; }

		/// <inheritdoc/>
		public IMethodInfo Method { get; private set; }

		/// <inheritdoc/>
		public int Timeout { get; private set; }

		/// <inheritdoc/>
		public string DisplayName { get; private set; }

		/// <inheritdoc/>
		public string? SkipReason { get; private set; }

		/// <summary>
		/// Gets the index of the <see cref="RazorTest"/> in the test component.
		/// </summary>
		public int TestNumber { get; private set; }

		/// <inheritdoc/>
		public ISourceInformation? SourceInformation { get; set; }

		/// <inheritdoc/>
		public ITestMethod TestMethod { get; private set; }

		/// <inheritdoc/>
		public object[] TestMethodArguments { get; } = Array.Empty<object>();

		/// <inheritdoc/>
		public Dictionary<string, List<string>>? Traits { get; }

		/// <inheritdoc/>
		public string UniqueID
		{
			get
			{
				if (uniqueId is null)
					uniqueId = GetUniqueID(TestMethod);
				return uniqueId;
			}
		}

		/// <inheritdoc/>
		public Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink, IMessageBus messageBus, object[] constructorArguments, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
		{
			var runner = new RazorTestCaseRunner(this, DisplayName, SkipReason, constructorArguments, TestMethodArguments, messageBus, aggregator, cancellationTokenSource);
			return runner.RunAsync();
		}

		/// <inheritdoc/>
		public void Serialize(IXunitSerializationInfo info)
		{
			info.AddValue(nameof(TestMethod), TestMethod);
			info.AddValue(nameof(Timeout), Timeout);
			info.AddValue(nameof(DisplayName), DisplayName);
			info.AddValue(nameof(SkipReason), SkipReason);
			info.AddValue(nameof(TestNumber), TestNumber);
		}

		/// <inheritdoc/>
		public void Deserialize(IXunitSerializationInfo info)
		{
			TestMethod = info.GetValue<ITestMethod>(nameof(TestMethod));
			Method = TestMethod.Method;
			Timeout = info.GetValue<int>(nameof(Timeout));
			DisplayName = info.GetValue<string>(nameof(DisplayName));
			SkipReason = info.GetValue<string?>(nameof(SkipReason));
			TestNumber = info.GetValue<int>(nameof(TestNumber));
		}

		/// <summary>
		/// Gets the unique ID for the test case.
		/// </summary>
		private string GetUniqueID(ITestMethod testMethod)
		{
			if (testMethod is null)
				throw new ArgumentNullException(nameof(testMethod));

			using var stream = new MemoryStream();
			var assemblyName = testMethod.TestClass.TestCollection.TestAssembly.Assembly.Name;

			// Get just the assembly name (without version info) when obtained by reflection
			if (testMethod.TestClass.TestCollection.TestAssembly.Assembly is IReflectionAssemblyInfo assembly)
				assemblyName = assembly.Assembly.GetName().Name ?? string.Empty;

			Write(stream, assemblyName);
			Write(stream, testMethod.TestClass.Class.Name);
			Write(stream, TestNumber.ToString(CultureInfo.InvariantCulture));

			stream.Position = 0;

			var hash = new byte[20];
			var data = stream.ToArray();

			var hasher = new Sha1Digest();
			hasher.BlockUpdate(data, 0, data.Length);
			hasher.DoFinal(hash, 0);

			return BytesToHexString(hash);

			static void Write(Stream stream, string value)
			{
				var bytes = Encoding.UTF8.GetBytes(value);
				stream.Write(bytes, 0, bytes.Length);
				stream.WriteByte(0);
			}

			static string BytesToHexString(byte[] bytes)
			{
				var chars = new char[bytes.Length * 2];
				var i = 0;

				foreach (var b in bytes)
				{
					chars[i++] = NibbleToHexChar(b >> 4);
					chars[i++] = NibbleToHexChar(b & 0xF);
				}

				return new string(chars);
			}

			static char NibbleToHexChar(int b)
				=> (char)(b < 10 ? b + '0' : b - 10 + 'a');
		}
	}
}
