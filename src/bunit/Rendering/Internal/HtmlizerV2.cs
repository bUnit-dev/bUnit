// This code/file was originally copied from https://github.com/dotnet/aspnetcore/
// It's content has been modified from the original.
// See the NOTICE.md at the root of this repository for a copy
// of the license from the aspnetcore repository.
using Bunit.Rendering.Internal;
using System.Diagnostics;
using System.Globalization;

namespace Bunit;

/// <summary>
/// This file is based on
/// https://source.dot.net/#Microsoft.AspNetCore.Mvc.ViewFeatures/RazorComponents/HtmlRenderer.cs.
/// </summary>
internal static class HtmlizerV2
{
	private const string BlazorInternalAttrPrefix = "__internal_";
	private const string BlazorCssScopeAttrPrefix = "b-";
	internal const string BlazorAttrPrefix = "blazor:";
	internal const string ElementReferenceAttrName = BlazorAttrPrefix + "elementReference";

	private static readonly HashSet<string> SelfClosingElements =
		new(StringComparer.OrdinalIgnoreCase)
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

	public static void GenerateMarkup(BunitRootComponentState componentState)
	{
		var frames = componentState.GetRenderTreeFrames(componentState.ComponentId);
		var newPosition = RenderFrames(componentState, frames, 0, frames.Count);
		Debug.Assert(
			newPosition == frames.Count,
			$"frames.Length = {frames.Count}. newPosition = {newPosition}"
		);
	}

	private static int RenderFrames(
		BunitRootComponentState componentState,
		in ArrayRange<RenderTreeFrame> frames,
		int position,
		int maxElements
	)
	{
		var nextPosition = position;
		var endPosition = position + maxElements;

		while (position < endPosition)
		{
			nextPosition = RenderCore(componentState, frames, position);
			if (position == nextPosition)
			{
				throw new InvalidOperationException("We didn't consume any input.");
			}

			position = nextPosition;
		}

		return nextPosition;
	}

	private static int RenderCore(
		BunitRootComponentState componentState,
		in ArrayRange<RenderTreeFrame> frames,
		int position
	)
	{
		var frame = frames.Array[position];
		switch (frame.FrameType)
		{
			case RenderTreeFrameType.Element:
				return RenderElement(componentState, frames, position);
			case RenderTreeFrameType.Attribute:
				throw new InvalidOperationException(
					$"Attributes should only be encountered within {nameof(RenderElement)}"
				);
			case RenderTreeFrameType.Text:
				AppendEscapeText(componentState, frame.TextContent);
				return position + 1;
			case RenderTreeFrameType.Markup:
				componentState.Append(frame.MarkupContent);
				return position + 1;
			case RenderTreeFrameType.Component:
				return RenderChildComponent(componentState, frames, position);
			case RenderTreeFrameType.Region:
				return RenderFrames(componentState, frames, position + 1, frame.RegionSubtreeLength - 1);
			case RenderTreeFrameType.ElementReferenceCapture:
			case RenderTreeFrameType.ComponentReferenceCapture:
				return position + 1;
			default:
				throw new InvalidOperationException(
					$"Invalid element frame type '{frame.FrameType}'."
				);
		}
	}

	private static int RenderChildComponent(
		BunitRootComponentState componentState,
		in ArrayRange<RenderTreeFrame> frames,
		int position)
	{
		var frame = frames.Array[position];
		var childFrames = componentState.GetRenderTreeFrames(frame.ComponentId);
		componentState.MarkComponentStart(frame.ComponentId);
		RenderFrames(componentState, childFrames, 0, childFrames.Count);
		componentState.MarkComponentStop(frame.ComponentId);
		return position + frame.ComponentSubtreeLength;
	}

	private static int RenderElement(
		BunitRootComponentState componentState,
		in ArrayRange<RenderTreeFrame> frames,
		int position
	)
	{
		var frame = frames.Array[position];
		componentState.Append('<');
		componentState.Append(frame.ElementName);
		var afterAttributes = RenderAttributes(
			componentState,
			frames,
			position + 1,
			frame.ElementSubtreeLength - 1,
			out var capturedValueAttribute
		);

		// When we see an <option> as a descendant of a <select>, and the option's "value" attribute matches the
		// "value" attribute on the <select>, then we auto-add the "selected" attribute to that option. This is
		// a way of converting Blazor's select binding feature to regular static HTML.
		if (
			componentState.ClosestSelectValueAsString != null
			&& string.Equals(frame.ElementName, "option", StringComparison.OrdinalIgnoreCase)
			&& string.Equals(
				capturedValueAttribute,
				componentState.ClosestSelectValueAsString,
				StringComparison.Ordinal
			)
		)
		{
			componentState.Append(" selected");
		}

		var remainingElements = frame.ElementSubtreeLength + position - afterAttributes;
		if (remainingElements > 0)
		{
			componentState.Append('>');

			var isSelect = string.Equals(
				frame.ElementName,
				"select",
				StringComparison.OrdinalIgnoreCase
			);
			if (isSelect)
			{
				componentState.ClosestSelectValueAsString = capturedValueAttribute;
			}

			var afterElement = RenderChildren(componentState, frames, afterAttributes, remainingElements);

			if (isSelect)
			{
				// There's no concept of nested <select> elements, so as soon as we're exiting one of them,
				// we can safely say there is no longer any value for this
				componentState.ClosestSelectValueAsString = null;
			}

			componentState.Append("</");
			componentState.Append(frame.ElementName);
			componentState.Append('>');
			return afterElement;
		}

		if (SelfClosingElements.Contains(frame.ElementName))
		{
			componentState.Append(" />");
		}
		else
		{
			componentState.Append('>');
			componentState.Append("</");
			componentState.Append(frame.ElementName);
			componentState.Append('>');
		}

		Debug.Assert(
			afterAttributes == position + frame.ElementSubtreeLength,
			$"afterAttributes = {afterAttributes}. position = {position}. frame.ElementSubtreeLength = {frame.ElementSubtreeLength}"
		);
		return afterAttributes;
	}

	private static int RenderChildren(
		BunitRootComponentState componentState,
		in ArrayRange<RenderTreeFrame> frames,
		int position,
		int maxElements
	)
	{
		if (maxElements == 0)
		{
			return position;
		}

		return RenderFrames(componentState, frames, position, maxElements);
	}

	private static int RenderAttributes(
		BunitRootComponentState componentState,
		in ArrayRange<RenderTreeFrame> frames,
		int position,
		int maxElements,
		out string? capturedValueAttribute
	)
	{
		capturedValueAttribute = null;

		if (maxElements == 0)
		{
			return position;
		}

		for (var i = 0; i < maxElements; i++)
		{
			var candidateIndex = position + i;
			var frame = frames.Array[candidateIndex];

			// Added to write ElementReferenceCaptureId to DOM
			if (frame.FrameType == RenderTreeFrameType.ElementReferenceCapture)
			{
				var value = $" {ElementReferenceAttrName}=\"{frame.ElementReferenceCaptureId}\"";
				componentState.Append(value);
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
				componentState.Append(' ');
				componentState.Append(BlazorAttrPrefix);
				componentState.Append(frame.AttributeName);
				componentState.Append('=');
				componentState.Append('"');
				componentState.Append(frame.AttributeEventHandlerId.ToString(CultureInfo.InvariantCulture));
				componentState.Append('"');
				continue;
			}

			switch (frame.AttributeValue)
			{
				case bool flag
					when flag
						&& frame.AttributeName.StartsWith(
							BlazorInternalAttrPrefix,
							StringComparison.Ordinal
						):
					// NOTE: This was added to make it more obvious
					// that this is a generated/special blazor attribute
					// for internal usage
					var nameParts = frame.AttributeName.Split(
						'_',
						StringSplitOptions.RemoveEmptyEntries
					);
					componentState.Append(' ');
					componentState.Append(BlazorAttrPrefix);
					componentState.Append(nameParts[2]);
					componentState.Append(':');
					componentState.Append(nameParts[1]);
					break;
				case true:
					componentState.Append(' ');
					componentState.Append(frame.AttributeName);
					break;
				case string value:
					componentState.Append(' ');
					componentState.Append(frame.AttributeName);
					componentState.Append('=');
					componentState.Append('"');
					AppendEscapeAttributeValue(componentState, value);
					componentState.Append('"');
					break;
				default:
					break;
			}
		}

		return position + maxElements;
	}

	private static void AppendEscapeText(BunitRootComponentState componentState, string value)
	{
		var valueSpan = value.AsSpan();
		var copyFromIndex = 0;
		for (var index = 0; index < valueSpan.Length; index++)
		{
			var c = valueSpan[index];
			if (c is '<' or '>' or '&')
			{
				componentState.Append(valueSpan.Slice(copyFromIndex, index - copyFromIndex));
				switch (c)
				{
					case '<':
						componentState.Append("&lt;");
						break;
					case '>':
						componentState.Append("&gt;");
						break;
					case '&':
						componentState.Append("&amp;");
						break;
				}
				copyFromIndex = index + 1;
			}
		}

		if (copyFromIndex < valueSpan.Length)
		{
			componentState.Append(valueSpan.Slice(copyFromIndex));
		}
	}

	private static void AppendEscapeAttributeValue(BunitRootComponentState componentState, string value)
	{
		var valueSpan = value.AsSpan();
		var copyFromIndex = 0;
		for (var index = 0; index < valueSpan.Length; index++)
		{
			var c = valueSpan[index];
			if (c is '"' or '&')
			{
				componentState.Append(valueSpan.Slice(copyFromIndex, index - copyFromIndex));
				switch (c)
				{
					case '"':
						componentState.Append("&quot;");
						break;
					case '&':
						componentState.Append("&amp;");
						break;
				}
				copyFromIndex = index + 1;
			}
		}

		if (copyFromIndex < valueSpan.Length)
		{
			componentState.Append(valueSpan.Slice(copyFromIndex));
		}
	}
}
