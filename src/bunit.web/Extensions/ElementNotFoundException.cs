using System;
using System.Runtime.Serialization;

namespace Bunit
{
	/// <summary>
	/// Represents a failure to find an element in the searched target
	/// using a CSS selector.
	/// </summary>
	[Serializable]
	public class ElementNotFoundException : Exception
	{
		/// <summary>
		/// Gets the CSS selector used to search with.
		/// </summary>
		public string CssSelector { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ElementNotFoundException"/> class.
		/// </summary>
		public ElementNotFoundException(string cssSelector)
			: base($"No elements were found that matches the selector '{cssSelector}'")
		{
			CssSelector = cssSelector;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ElementNotFoundException"/> class.
		/// </summary>
		protected ElementNotFoundException(string message, string cssSelector)
			: base(message)
		{
			CssSelector = cssSelector;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ElementNotFoundException"/> class.
		/// </summary>
		protected ElementNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
			CssSelector = serializationInfo?.GetString(nameof(CssSelector)) ?? string.Empty;
		}

		/// <inheritdoc/>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info is null) throw new ArgumentNullException(nameof(info));
			info.AddValue(nameof(CssSelector), CssSelector);
			base.GetObjectData(info, context);
		}
	}
}
