// This code/file was originally copied from https://github.com/dotnet/aspnetcore/
// It's content has been modified from the original.
// See the NOTICE.md at the root of this repository for a copy
// of the license from the aspnetcore repository.
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;
using Bunit.Rendering;

namespace Bunit;

/// <summary>
/// This file is based on
/// https://source.dot.net/#Microsoft.AspNetCore.Mvc.ViewFeatures/RazorComponents/HtmlRenderer.cs.
/// </summary>
internal static class Htmlizer
{
	private const string BlazorInternalAttrPrefix = "__internal_";
	private const string BlazorCssScopeAttrPrefix = "b-";
	internal const string BlazorAttrPrefix = "blazor:";
	internal const string ElementReferenceAttrName = BlazorAttrPrefix + "elementReference";

	private static readonly HtmlEncoder HtmlEncoder = HtmlEncoder.Default;

	private static readonly HashSet<string> SelfClosingElements = new(StringComparer.OrdinalIgnoreCase)
	{
		"area",
		"base",
		"br",
		"col",
		"embed",
		"hr",
		"img",
		"input",
		"link",
		"meta",
		"param",
		"source",
		"track",
		"wbr",
	};

	public static bool IsBlazorAttribute(string attributeName)
	{
		if (attributeName is null)
			throw new ArgumentNullException(nameof(attributeName));
		return attributeName.StartsWith(BlazorAttrPrefix, StringComparison.Ordinal) ||
					  attributeName.StartsWith(BlazorCssScopeAttrPrefix, StringComparison.Ordinal);
	}

	public static string ToBlazorAttribute(string attributeName)
	{
		return $"{BlazorAttrPrefix}{attributeName}";
	}

	public static string GetHtml(int componentId, RenderTreeFrameDictionary framesCollection)
	{
		var context = new HtmlRenderingContext(framesCollection);
		var frames = context.GetRenderTreeFrames(componentId);
		var newPosition = RenderFrames(context, frames, 0, frames.Length);
		Debug.Assert(newPosition == frames.Length, $"frames.Length = {frames.Length}. newPosition = {newPosition}");
		return context.Result.ToString();
	}

	private static int RenderFrames(HtmlRenderingContext context, ReadOnlySpan<RenderTreeFrame> frames, int position, int maxElements)
	{
		var nextPosition = position;
		var endPosition = position + maxElements;

		while (position < endPosition)
		{
			nextPosition = RenderCore(context, frames, position);
			if (position == nextPosition)
			{
				throw new InvalidOperationException("We didn't consume any input.");
			}

			position = nextPosition;
		}

		return nextPosition;
	}

	private static int RenderCore(
		HtmlRenderingContext context,
		ReadOnlySpan<RenderTreeFrame> frames,
		int position)
	{
		var frame = frames[position];
		switch (frame.FrameType)
		{
			case RenderTreeFrameType.Element:
				return RenderElement(context, frames, position);
			case RenderTreeFrameType.Attribute:
				throw new InvalidOperationException($"Attributes should only be encountered within {nameof(RenderElement)}");
			case RenderTreeFrameType.Text:
				context.Result.Append(HtmlEncoder.Encode(frame.TextContent));
				return position + 1;
			case RenderTreeFrameType.Markup:
				context.Result.Append(frame.MarkupContent);
				return position + 1;
			case RenderTreeFrameType.Component:
				return RenderChildComponent(context, frames, position);
			case RenderTreeFrameType.Region:
				return RenderFrames(context, frames, position + 1, frame.RegionSubtreeLength - 1);
			case RenderTreeFrameType.ElementReferenceCapture:
			case RenderTreeFrameType.ComponentReferenceCapture:
				return position + 1;
			default:
				throw new InvalidOperationException($"Invalid element frame type '{frame.FrameType}'.");
		}
	}

	private static int RenderChildComponent(
		HtmlRenderingContext context,
		ReadOnlySpan<RenderTreeFrame> frames,
		int position)
	{
		var frame = frames[position];
		var childFrames = context.GetRenderTreeFrames(frame.ComponentId);
		RenderFrames(context, childFrames, 0, childFrames.Length);
		return position + frame.ComponentSubtreeLength;
	}

	private static int RenderElement(
		HtmlRenderingContext context,
		ReadOnlySpan<RenderTreeFrame> frames,
		int position)
	{
		var frame = frames[position];
		var result = context.Result;
		result.Append('<');
		result.Append(frame.ElementName);
		var afterAttributes = RenderAttributes(context, frames, position + 1, frame.ElementSubtreeLength - 1, out var capturedValueAttribute);

		// When we see an <option> as a descendant of a <select>, and the option's "value" attribute matches the
		// "value" attribute on the <select>, then we auto-add the "selected" attribute to that option. This is
		// a way of converting Blazor's select binding feature to regular static HTML.
		if (context.ClosestSelectValueAsString != null
			&& string.Equals(frame.ElementName, "option", StringComparison.OrdinalIgnoreCase)
			&& string.Equals(capturedValueAttribute, context.ClosestSelectValueAsString, StringComparison.Ordinal))
		{
			result.Append(" selected");
		}

		var remainingElements = frame.ElementSubtreeLength + position - afterAttributes;
		if (remainingElements > 0)
		{
			result.Append('>');

			var isSelect = string.Equals(frame.ElementName, "select", StringComparison.OrdinalIgnoreCase);
			if (isSelect)
			{
				context.ClosestSelectValueAsString = capturedValueAttribute;
			}

			var afterElement = RenderChildren(context, frames, afterAttributes, remainingElements);

			if (isSelect)
			{
				// There's no concept of nested <select> elements, so as soon as we're exiting one of them,
				// we can safely say there is no longer any value for this
				context.ClosestSelectValueAsString = null;
			}

			result.Append("</");
			result.Append(frame.ElementName);
			result.Append('>');
			return afterElement;
		}

		if (SelfClosingElements.Contains(frame.ElementName))
		{
			result.Append(" />");
		}
		else
		{
			result.Append('>');
			result.Append("</");
			result.Append(frame.ElementName);
			result.Append('>');
		}

		Debug.Assert(afterAttributes == position + frame.ElementSubtreeLength, $"afterAttributes = {afterAttributes}. position = {position}. frame.ElementSubtreeLength = {frame.ElementSubtreeLength}");
		return afterAttributes;
	}

	private static int RenderChildren(HtmlRenderingContext context, ReadOnlySpan<RenderTreeFrame> frames, int position, int maxElements)
	{
		if (maxElements == 0)
		{
			return position;
		}

		return RenderFrames(context, frames, position, maxElements);
	}

	[SuppressMessage("Design", "MA0051:Method is too long", Justification = "TODO: Refactor")]
	private static int RenderAttributes(
		HtmlRenderingContext context,
		ReadOnlySpan<RenderTreeFrame> frames,
		int position,
		int maxElements,
		out string? capturedValueAttribute)
	{
		capturedValueAttribute = null;

		if (maxElements == 0)
		{
			return position;
		}

		var result = context.Result;

		for (var i = 0; i < maxElements; i++)
		{
			var candidateIndex = position + i;
			var frame = frames[candidateIndex];

			// Added to write ElementReferenceCaptureId to DOM
			if (frame.FrameType == RenderTreeFrameType.ElementReferenceCapture)
			{
				var value = $" {ElementReferenceAttrName}=\"{frame.ElementReferenceCaptureId}\"";
				result.Append(value);
			}

			if (frame.FrameType != RenderTreeFrameType.Attribute)
			{
				return candidateIndex;
			}

			if (frame.AttributeName.Equals("value", StringComparison.OrdinalIgnoreCase))
			{
				capturedValueAttribute = frame.AttributeValue as string;
			}

			if (frame.AttributeEventHandlerId > 0)
			{
				// NOTE: this was changed to make it more obvious
				//       that this is a generated/special blazor attribute
				//       used for tracking event handler id's
				result.Append(' ');
				result.Append(BlazorAttrPrefix);
				result.Append(frame.AttributeName);
				result.Append('=');
				result.Append('"');
				result.Append(frame.AttributeEventHandlerId.ToString(CultureInfo.InvariantCulture));
				result.Append('"');
				continue;
			}

			switch (frame.AttributeValue)
			{
				case bool flag when flag && frame.AttributeName.StartsWith(BlazorInternalAttrPrefix, StringComparison.Ordinal):
					// NOTE: This was added to make it more obvious
					// that this is a generated/special blazor attribute
					// for internal usage
					var nameParts = frame.AttributeName.Split('_', StringSplitOptions.RemoveEmptyEntries);
					result.Append(' ');
					result.Append(BlazorAttrPrefix);
					result.Append(nameParts[2]);
					result.Append(':');
					result.Append(nameParts[1]);
					break;
				case bool flag and true:
					result.Append(' ');
					result.Append(frame.AttributeName);
					break;
				case string value:
					result.Append(' ');
					result.Append(frame.AttributeName);
					result.Append('=');
					result.Append('"');
					result.Append(HtmlEncoder.Encode(value));
					result.Append('"');
					break;
				default:
					break;
			}
		}

		return position + maxElements;
	}

	private sealed class HtmlRenderingContext
	{
		private readonly RenderTreeFrameDictionary frames;

		public HtmlRenderingContext(RenderTreeFrameDictionary frames)
		{
			this.frames = frames;
		}

		public ReadOnlySpan<RenderTreeFrame> GetRenderTreeFrames(int componentId)
			=> new(frames[componentId].Array, 0, frames[componentId].Count);

		public StringBuilder Result { get; } = new ();

		public string? ClosestSelectValueAsString { get; set; }
	}
}