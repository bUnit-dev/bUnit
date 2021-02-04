using System;
using System.Runtime.Serialization;

namespace Bunit
{
	/// <summary>
	/// Represents a number of unexpected invocation to a <see cref="BunitJSInterop"/>.
	/// </summary>
	[Serializable]
	public sealed class JSInvokeCountExpectedException : Exception
	{
		/// <summary>
		/// Gets the expected invocation count.
		/// </summary>
		public int ExpectedInvocationCount { get; }

		/// <summary>
		/// Gets the actual invocation count.
		/// </summary>
		public int ActualInvocationCount { get; }

		/// <summary>
		/// Gets the identifier.
		/// </summary>
		public string Identifier { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="JSInvokeCountExpectedException"/> class.
		/// </summary>
		public JSInvokeCountExpectedException(string identifier, int expectedCount, int actualCount, string assertMethod, string? userMessage = null)
			: base(CreateMessage(identifier, expectedCount, actualCount, assertMethod, userMessage))
		{
			ExpectedInvocationCount = expectedCount;
			ActualInvocationCount = actualCount;
			Identifier = identifier;
		}

		private JSInvokeCountExpectedException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
			if (serializationInfo is null) throw new ArgumentNullException(nameof(serializationInfo));
			ExpectedInvocationCount = serializationInfo.GetInt32(nameof(ExpectedInvocationCount));
			ActualInvocationCount = serializationInfo.GetInt32(nameof(ActualInvocationCount));
			Identifier = serializationInfo.GetString(nameof(Identifier)) ?? string.Empty;
		}

		/// <inheritdoc/>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info is null) throw new ArgumentNullException(nameof(info));

			info.AddValue(nameof(ExpectedInvocationCount), ExpectedInvocationCount);
			info.AddValue(nameof(ActualInvocationCount), ActualInvocationCount);
			info.AddValue(nameof(Identifier), Identifier);
			base.GetObjectData(info, context);
		}

		private static string CreateMessage(string identifier, int expectedCount, int actualCount, string assertMethod, string? userMessage = null)
		{
			var result = $"{assertMethod} failed: ";
			result += userMessage is null
					? $"\"{identifier}\" was not called the expected number of times."
					: userMessage;

			result += Environment.NewLine;
			result += Environment.NewLine;
			result += $"Expected number of calls: {expectedCount}{Environment.NewLine}";
			result += $"Actual number of calls:   {actualCount}{Environment.NewLine}";

			return result;
		}
	}
}
