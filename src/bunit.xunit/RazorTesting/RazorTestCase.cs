using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bunit.RazorTesting;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Sdk
{

	internal class RazorTestCase : LongLivedMarshalByRefObject, IXunitTestCase, ITestCase, IXunitSerializable, ITest
	{
		private static readonly Dictionary<string, List<string>> EmptyTraits = new Dictionary<string, List<string>>(0);
		private string? _uniqueId;

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Called by the deserializer; should only be called by deriving classes for de-serialization purposes")]
		public RazorTestCase() { }

		public RazorTestCase(RazorTest test, int testNumber, int totalTests, ITestMethod testMethod)
		{
			TestMethod = testMethod;
			Method = testMethod.Method;
			DisplayName = test.Description ?? $"{test.GetType().Name} {testNumber}";
			Timeout = test.Timeout;
			SkipReason = test.Skip;
		}

		public Exception? InitializationException { get; private set; }

		/// <inheritdoc/>
		public IMethodInfo Method { get; private set; }

		/// <inheritdoc/>
		public int Timeout { get; private set; }

		/// <inheritdoc/>
		public string DisplayName { get; private set; }

		/// <inheritdoc/>
		public string? SkipReason { get; private set; }

		/// <inheritdoc/>
		public ISourceInformation? SourceInformation { get; set; }

		/// <inheritdoc/>
		public ITestMethod TestMethod { get; private set; }

		/// <inheritdoc/>
		public object[] TestMethodArguments { get; } = Array.Empty<object>();

		/// <inheritdoc/>
		public Dictionary<string, List<string>> Traits { get; } = EmptyTraits;

		public string UniqueID
		{
			get
			{
				if (_uniqueId is null)
					_uniqueId = GetUniqueID(TestMethod);
				return _uniqueId;
			}
		}

		public ITestCase TestCase => this;

		public Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink, IMessageBus messageBus, object[] constructorArguments, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
			=> throw new NotImplementedException();

		/// <inheritdoc/>
		public void Serialize(IXunitSerializationInfo data)
		{
			data.AddValue(nameof(TestMethod), TestMethod);
			data.AddValue(nameof(Timeout), Timeout);
			data.AddValue(nameof(DisplayName), DisplayName);
			data.AddValue(nameof(SkipReason), SkipReason);
		}

		/// <inheritdoc/>
		public void Deserialize(IXunitSerializationInfo data)
		{
			TestMethod = data.GetValue<ITestMethod>(nameof(TestMethod));
			Method = TestMethod.Method;
			Timeout = data.GetValue<int>(nameof(Timeout));
			DisplayName = data.GetValue<string>(nameof(DisplayName));
			SkipReason = data.GetValue<string?>(nameof(SkipReason));
		}

		/// <summary>
		/// Gets the unique ID for the test case.
		/// </summary>
		static string GetUniqueID(ITestMethod testMethod)
		{
			if (testMethod is null)
				throw new ArgumentNullException(nameof(testMethod));

			using (var stream = new MemoryStream())
			{
				var assemblyName = testMethod.TestClass.TestCollection.TestAssembly.Assembly.Name;

				//Get just the assembly name (without version info) when obtained by reflection
				if (testMethod.TestClass.TestCollection.TestAssembly.Assembly is IReflectionAssemblyInfo assembly)
					assemblyName = assembly.Assembly.GetName().Name;

				Write(stream, assemblyName);
				Write(stream, testMethod.TestClass.Class.Name);
				Write(stream, testMethod.Method.Name);

				stream.Position = 0;

				var hash = new byte[20];
				var data = stream.ToArray();

				var hasher = new Sha1Digest();
				hasher.BlockUpdate(data, 0, data.Length);
				hasher.DoFinal(hash, 0);

				return BytesToHexString(hash);
			}
		}

		static void Write(Stream stream, string value)
		{
			var bytes = Encoding.UTF8.GetBytes(value);
			stream.Write(bytes, 0, bytes.Length);
			stream.WriteByte(0);
		}

		/// <summary>Converts an array of bytes to its hexadecimal value as a string.</summary>
		/// <param name="bytes">The bytes.</param>
		/// <returns>A string containing the hexadecimal representation of the provided bytes.</returns>
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

		/// <summary>Gets a hexadecimal digit character from the 4-bit value.</summary>
		/// <param name="b">A value in the range [0, 15].</param>
		/// <returns>A character in the range ['0','9'] or ['a','f'].</returns>
		static char NibbleToHexChar(int b)
		{
			Debug.Assert(b < 16);
			return (char)(b < 10 ? b + '0' : b - 10 + 'a');
		}
	}
}
