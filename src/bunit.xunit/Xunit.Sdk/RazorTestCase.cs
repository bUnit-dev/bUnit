using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Bunit.RazorTesting;

using Xunit.Abstractions;

namespace Xunit.Sdk
{
	public class RazorTestCase : LongLivedMarshalByRefObject, IXunitTestCase, ITestCase, ITest, IXunitSerializable
	{
		private static readonly Dictionary<string, List<string>> EmptyTraits = new Dictionary<string, List<string>>(0);

		private string? _uniqueId;

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Called by the deserializer; should only be called by deriving classes for de-serialization purposes")]
		public RazorTestCase() { }

		public RazorTestCase(RazorTest test, int testIndex, ITestMethod testMethod)
		{
			TestMethod = testMethod;
			Method = testMethod.Method;
			DisplayName = test.Description ?? $"{test.GetType().Name} {testIndex}";
			Timeout = test.Timeout;
			SkipReason = test.Skip;
			try
			{
				test.Validate();
			}
			catch (Exception ex)
			{
				InitializationException = ex;
			}
		}

		/// <inheritdoc/>
		public Exception? InitializationException { get; protected set; }

		/// <inheritdoc/>
		public IMethodInfo Method { get; protected set; }

		/// <inheritdoc/>
		public int Timeout { get; protected set; }

		/// <inheritdoc/>
		public string DisplayName { get; protected set; }

		/// <inheritdoc/>
		public string? SkipReason { get; protected set; }

		public int TestIndex { get; protected set; }

		/// <inheritdoc/>
		public ISourceInformation? SourceInformation { get; set; }

		/// <inheritdoc/>
		public ITestMethod TestMethod { get; protected set; }

		/// <inheritdoc/>
		public object[]? TestMethodArguments { get; }

		/// <inheritdoc/>
		public Dictionary<string, List<string>> Traits { get; } = EmptyTraits;

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
		public async Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink, IMessageBus messageBus, object[] constructorArguments, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
		{
			var summary = new RunSummary();
			return summary; // TEMPOARAY

			if (!messageBus.QueueMessage(new TestCaseStarting(TestCase)))
				cancellationTokenSource.Cancel();
			else
			{
				try
				{
					//await AfterTestCaseStartingAsync();
					summary = await RunTestAsync(messageBus, aggregator, constructorArguments, cancellationTokenSource);

					aggregator.Clear();
					//await BeforeTestCaseFinishedAsync();

					if (aggregator.HasExceptions)
						if (!messageBus.QueueMessage(new TestCaseCleanupFailure(TestCase, aggregator.ToException())))
							cancellationTokenSource.Cancel();
				}
				finally
				{
					if (!messageBus.QueueMessage(new TestCaseFinished(TestCase, summary.Time, summary.Total, summary.Failed, summary.Skipped)))
						cancellationTokenSource.Cancel();
				}
			}

			return summary;
		}

		private async Task<RunSummary> RunTestAsync(IMessageBus messageBus, ExceptionAggregator parentAggregator, object[] constructorArguments, CancellationTokenSource cancellationTokenSource)
		{
			var runSummary = new RunSummary { Total = 1 };
			var output = string.Empty;

			if (!messageBus.QueueMessage(new TestStarting(this)))
				cancellationTokenSource.Cancel();
			else
			{
				//AfterTestStarting();

				if (!string.IsNullOrEmpty(SkipReason))
				{
					runSummary.Skipped++;

					if (!messageBus.QueueMessage(new TestSkipped(this, SkipReason)))
						cancellationTokenSource.Cancel();
				}
				else
				{
					var aggregator = new ExceptionAggregator(parentAggregator);

					if (!aggregator.HasExceptions)
					{
						var tuple = await aggregator.RunAsync(() => InvokeTestAsync(messageBus, aggregator, constructorArguments, cancellationTokenSource));
						if (tuple != null)
						{
							runSummary.Time = tuple.Item1;
							output = tuple.Item2;
						}
					}

					var exception = aggregator.ToException();
					TestResultMessage testResult;

					if (exception == null)
						testResult = new TestPassed(this, runSummary.Time, output);
					else
					{
						testResult = new TestFailed(this, runSummary.Time, output, exception);
						runSummary.Failed++;
					}

					if (!cancellationTokenSource.IsCancellationRequested)
						if (!messageBus.QueueMessage(testResult))
							cancellationTokenSource.Cancel();
				}

				parentAggregator.Clear();
				//BeforeTestFinished();

				if (parentAggregator.HasExceptions)
					if (!messageBus.QueueMessage(new TestCleanupFailure(this, parentAggregator.ToException())))
						cancellationTokenSource.Cancel();

				if (!messageBus.QueueMessage(new TestFinished(this, runSummary.Time, output)))
					cancellationTokenSource.Cancel();
			}

			return runSummary;
		}

		protected async Task<Tuple<decimal, string>> InvokeTestAsync(IMessageBus messageBus, ExceptionAggregator aggregator, object[] constructorArguments, CancellationTokenSource cancellationTokenSource)
		{
			var output = string.Empty;

			TestOutputHelper? testOutputHelper = null;
			foreach (object obj in constructorArguments)
			{
				testOutputHelper = obj as TestOutputHelper;
				if (testOutputHelper != null)
					break;
			}

			if (testOutputHelper != null)
				testOutputHelper.Initialize(messageBus, this);

			var executionTime = await TestInvokerRunAsync(aggregator, cancellationTokenSource);

			if (testOutputHelper != null)
			{
				output = testOutputHelper.Output;
				testOutputHelper.Uninitialize();
			}

			return Tuple.Create(executionTime, output);
		}

		public Task<decimal> TestInvokerRunAsync(ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
		{
			return aggregator.RunAsync(async () =>
			{
				var timer = new ExecutionTimer();
				if (!cancellationTokenSource.IsCancellationRequested)
				{
					//var testClassInstance = CreateTestClass();
					// TODO: add ITestOutputHelper to services collection?
					// TODO: render razor test
					try
					{
						if (!cancellationTokenSource.IsCancellationRequested)
						{
							//TODO: await razorTest.RunTest();
						}
					}
					finally
					{
						// TODO: dispose razortest.
					}
				}

				return timer.Total;
			});
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
		}

		private static void Write(Stream stream, string value)
		{
			var bytes = Encoding.UTF8.GetBytes(value);
			stream.Write(bytes, 0, bytes.Length);
			stream.WriteByte(0);
		}

		/// <summary>Converts an array of bytes to its hexadecimal value as a string.</summary>
		/// <param name="bytes">The bytes.</param>
		/// <returns>A string containing the hexadecimal representation of the provided bytes.</returns>
		private static string BytesToHexString(byte[] bytes)
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
		private static char NibbleToHexChar(int b)
		{
			Debug.Assert(b < 16);
			return (char)(b < 10 ? b + '0' : b - 10 + 'a');
		}
	}
}
