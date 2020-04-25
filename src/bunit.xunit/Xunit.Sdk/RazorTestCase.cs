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

namespace Xunit.Sdk
{
	internal class RazorTestCase : LongLivedMarshalByRefObject, IXunitTestCase, ITestCase, ITest, IXunitSerializable
	{
		private string? _uniqueId;

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Called by the deserializer; should only be called by deriving classes for de-serialization purposes")]
		public RazorTestCase() { }

		public RazorTestCase(RazorTest test, int testIndex, ITestMethod testMethod)
		{
			TestMethod = testMethod;
			Method = testMethod.Method;
			DisplayName = test.Description ?? $"{test.GetType().Name} #{testIndex + 1}";
			Timeout = test.Timeout;
			SkipReason = test.Skip;
			TestIndex = testIndex;
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
		/// Gets the index of the
		/// </summary>
		public int TestIndex { get; private set; }

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
				if (_uniqueId is null)
					_uniqueId = GetUniqueID(TestMethod);
				return _uniqueId;
			}
		}

		/// <inheritdoc/>
		public ITestCase TestCase => this;

		/// <inheritdoc/>
		public Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink, IMessageBus messageBus, object[] constructorArguments, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
		{
			var razorTestInvoker = new RazorTestInvoker(this, diagnosticMessageSink, messageBus, constructorArguments, aggregator, cancellationTokenSource);
			return razorTestInvoker.RunTestCaseAsync();
		}

		/// <inheritdoc/>
		public void Serialize(IXunitSerializationInfo data)
		{
			data.AddValue(nameof(TestMethod), TestMethod);
			data.AddValue(nameof(Timeout), Timeout);
			data.AddValue(nameof(DisplayName), DisplayName);
			data.AddValue(nameof(SkipReason), SkipReason);
			data.AddValue(nameof(TestIndex), TestIndex);
		}

		/// <inheritdoc/>
		public void Deserialize(IXunitSerializationInfo data)
		{
			TestMethod = data.GetValue<ITestMethod>(nameof(TestMethod));
			Method = TestMethod.Method;
			Timeout = data.GetValue<int>(nameof(Timeout));
			DisplayName = data.GetValue<string>(nameof(DisplayName));
			SkipReason = data.GetValue<string?>(nameof(SkipReason));
			TestIndex = data.GetValue<int>(nameof(TestIndex));
		}

		/// <summary>
		/// Gets the unique ID for the test case.
		/// </summary>
		private string GetUniqueID(ITestMethod testMethod)
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
				Write(stream, DisplayName);
				Write(stream, TestIndex.ToString());

				stream.Position = 0;

				var hash = new byte[20];
				var data = stream.ToArray();

				var hasher = new Sha1Digest();
				hasher.BlockUpdate(data, 0, data.Length);
				hasher.DoFinal(hash, 0);

				return BytesToHexString(hash);
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
}
