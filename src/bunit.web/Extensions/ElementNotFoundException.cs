using System;
using System.Runtime.Serialization;

namespace Bunit
{
	/// <summary>
	/// Represents a failure to find an element in the searched target
	/// using a css selector.
	/// </summary>
	[Serializable]
	public class ElementNotFoundException : Exception
	{
		/// <summary>
		/// The CSS selector used to search with.
		/// </summary>
		public string CssSelector { get; }

		/// <inheritdoc/>
		public ElementNotFoundException(string cssSelector)
			: base($"No elements were found that matches the selector '{cssSelector}'")
		{
			CssSelector = cssSelector;
		}

		/// <inheritdoc/>
		protected ElementNotFoundException(string message, string cssSelector)
			: base(message)
		{
			CssSelector = cssSelector;
		}

		/// <inheritdoc/>
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
