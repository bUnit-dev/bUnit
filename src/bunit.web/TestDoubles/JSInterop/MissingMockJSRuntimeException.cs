using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bunit.TestDoubles
{
	/// <summary>
	/// Exception use to indicate that a MockJSRuntime is required by a test
	/// but was not provided.
	/// </summary>
	[Serializable]
	public sealed class MissingMockJSRuntimeException : Exception
	{
		/// <summary>
		/// Identifier string used in the JSInvoke method.
		/// </summary>
		public string Identifier { get; }

		/// <summary>
		/// Arguments passed to the JSInvoke method.
		/// </summary>
		public IReadOnlyList<object?> Arguments { get; }

		/// <summary>
		/// Creates a new instance of the <see cref="MissingMockJSRuntimeException"/>
		/// with the arguments used in the invocation.
		/// </summary>
		/// <param name="identifier">The identifier used in the invocation.</param>
		/// <param name="arguments">The arguments used in the invocation, if any</param>
		public MissingMockJSRuntimeException(string identifier, object?[]? arguments)
			: base($"This test requires a IJSRuntime to be supplied, because the component under test invokes the IJSRuntime during the test. The invoked method is '{identifier}' and the invocation arguments are stored in the {nameof(Arguments)} property of this exception. Guidance on mocking the IJSRuntime is available on bUnit's website.")
		{
			Identifier = identifier;
			Arguments = arguments ?? Array.Empty<object?>();
			HelpLink = "https://bunit.egilhansen.com/docs/test-doubles/mocking-ijsruntime";
		}

		private MissingMockJSRuntimeException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			if (serializationInfo is null) throw new ArgumentNullException(nameof(serializationInfo));
			Identifier = serializationInfo.GetString(nameof(Identifier)) ?? string.Empty;
			Arguments = serializationInfo.GetValue(nameof(Arguments), typeof(object?[])) as object?[] ?? Array.Empty<object?>();
		}

		/// <inheritdoc/>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info is null) throw new ArgumentNullException(nameof(info));
			info.AddValue(nameof(Identifier), Identifier);
			info.AddValue(nameof(Arguments), Arguments, typeof(object?[]));
			base.GetObjectData(info, context);
		}
	}
}
