using Bunit.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Encodings.Web;

namespace Bunit
{
	[SuppressMessage("Usage", "BL0006:Do not use RenderTree types", Justification = "<Pending>")]
    internal class Htmlizer
    {
        private static readonly HtmlEncoder HtmlEncoder = HtmlEncoder.Default;

        private static readonly HashSet<string> SelfClosingElements = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "area", "base", "br", "col", "embed", "hr", "img", "input", "link", "meta", "param", "source", "track", "wbr"
        };

		private const string BLAZOR_INTERNAL_ATTR_PREFIX = "__internal_";

		public const string BLAZOR_ATTR_PREFIX = "blazor:";
        public const string ELEMENT_REFERENCE_ATTR_NAME = BLAZOR_ATTR_PREFIX + "elementreference";

        public static bool IsBlazorAttribute(string attributeName)
            => attributeName.StartsWith(BLAZOR_ATTR_PREFIX, StringComparison.Ordinal);

        public static string ToBlazorAttribute(string attributeName)
        {
            return $"{BLAZOR_ATTR_PREFIX}{attributeName}";
        }

        public static string GetHtml(ITestRenderer renderer, int componentId)
        {
            var frames = renderer.GetCurrentRenderTreeFrames(componentId);
            var context = new HtmlRenderingContext(renderer);
            var newPosition = RenderFrames(context, frames, 0, frames.Count);
            Debug.Assert(newPosition == frames.Count);
            return string.Join(string.Empty, context.Result);
        }

        private static int RenderFrames(HtmlRenderingContext context, ArrayRange<RenderTreeFrame> frames, int position, int maxElements)
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
            ArrayRange<RenderTreeFrame> frames,
            int position)
        {
            ref var frame = ref frames.Array[position];
            switch (frame.FrameType)
            {
                case RenderTreeFrameType.Element:
                    return RenderElement(context, frames, position);
                case RenderTreeFrameType.Attribute:
                    throw new InvalidOperationException($"Attributes should only be encountered within {nameof(RenderElement)}");
                case RenderTreeFrameType.Text:
                    context.Result.Add(HtmlEncoder.Encode(frame.TextContent));
                    return ++position;
                case RenderTreeFrameType.Markup:
                    context.Result.Add(frame.MarkupContent);
                    return ++position;
                case RenderTreeFrameType.Component:
                    return RenderChildComponent(context, frames, position);
                case RenderTreeFrameType.Region:
                    return RenderFrames(context, frames, position + 1, frame.RegionSubtreeLength - 1);
                case RenderTreeFrameType.ElementReferenceCapture:
                case RenderTreeFrameType.ComponentReferenceCapture:
                    return ++position;
                default:
                    throw new InvalidOperationException($"Invalid element frame type '{frame.FrameType}'.");
            }
        }

        private static int RenderChildComponent(
            HtmlRenderingContext context,
            ArrayRange<RenderTreeFrame> frames,
            int position)
        {
            ref var frame = ref frames.Array[position];
            var childFrames = context.Renderer.GetCurrentRenderTreeFrames(frame.ComponentId);
            RenderFrames(context, childFrames, 0, childFrames.Count);
            return position + frame.ComponentSubtreeLength;
        }

        private static int RenderElement(
            HtmlRenderingContext context,
            ArrayRange<RenderTreeFrame> frames,
            int position)
        {
            ref var frame = ref frames.Array[position];
            var result = context.Result;
            result.Add("<");
            result.Add(frame.ElementName);
            var afterAttributes = RenderAttributes(context, frames, position + 1, frame.ElementSubtreeLength - 1, out var capturedValueAttribute);

            // When we see an <option> as a descendant of a <select>, and the option's "value" attribute matches the
            // "value" attribute on the <select>, then we auto-add the "selected" attribute to that option. This is
            // a way of converting Blazor's select binding feature to regular static HTML.
            if (context.ClosestSelectValueAsString != null
                && string.Equals(frame.ElementName, "option", StringComparison.OrdinalIgnoreCase)
                && string.Equals(capturedValueAttribute, context.ClosestSelectValueAsString, StringComparison.Ordinal))
            {
                result.Add(" selected");
            }

            var remainingElements = frame.ElementSubtreeLength + position - afterAttributes;
            if (remainingElements > 0)
            {
                result.Add(">");

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

                result.Add("</");
                result.Add(frame.ElementName);
                result.Add(">");
                Debug.Assert(afterElement == position + frame.ElementSubtreeLength);
                return afterElement;
            }
            else
            {
                if (SelfClosingElements.Contains(frame.ElementName))
                {
                    result.Add(" />");
                }
                else
                {
                    result.Add(">");
                    result.Add("</");
                    result.Add(frame.ElementName);
                    result.Add(">");
                }
                Debug.Assert(afterAttributes == position + frame.ElementSubtreeLength);
                return afterAttributes;
            }
        }

        private static int RenderChildren(HtmlRenderingContext context, ArrayRange<RenderTreeFrame> frames, int position, int maxElements)
        {
            if (maxElements == 0)
            {
                return position;
            }

            return RenderFrames(context, frames, position, maxElements);
        }

        private static int RenderAttributes(
            HtmlRenderingContext context,
            ArrayRange<RenderTreeFrame> frames, int position, int maxElements, out string? capturedValueAttribute)
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
                ref var frame = ref frames.Array[candidateIndex];

                // Added to write ElementReferenceCaptureId to DOM
                if (frame.FrameType == RenderTreeFrameType.ElementReferenceCapture)
                {
                    result.Add($" {ELEMENT_REFERENCE_ATTR_NAME}=\"{frame.ElementReferenceCaptureId}\"");
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
                    // NOTE: this was changed from  
                    //       result.Add($" {frame.AttributeName}=\"{frame.AttributeEventHandlerId}\"");
                    //       to the following to make it more obvious
                    //       that this is a generated/special blazor attribute
                    //       used for tracking event handler id's
                    result.Add($" {BLAZOR_ATTR_PREFIX}{frame.AttributeName}=\"{frame.AttributeEventHandlerId}\"");
                    continue;
                }

                switch (frame.AttributeValue)
                {
                    case bool flag when flag:
                        result.Add(" ");
                        if (frame.AttributeName.StartsWith(BLAZOR_INTERNAL_ATTR_PREFIX, StringComparison.Ordinal))
                        {
                            // NOTE: this was added to make it more obvious
                            //       that this is a generated/special blazor attribute
                            //		 for internal usage
                            result.Add(BLAZOR_ATTR_PREFIX);
                        }
						result.Add(frame.AttributeName);
                        break;
                    case string value:
                        result.Add(" ");
                        result.Add(frame.AttributeName);
                        result.Add("=");
                        result.Add("\"");
                        result.Add(HtmlEncoder.Encode(value));
                        result.Add("\"");
                        break;
                    default:
                        break;
                }
            }

            return position + maxElements;
        }

        private class HtmlRenderingContext
        {
            public ITestRenderer Renderer { get; }

            public HtmlRenderingContext(ITestRenderer renderer)
            {
                Renderer = renderer;
            }

            public List<string> Result { get; } = new List<string>();

            public string? ClosestSelectValueAsString { get; set; }
        }
    }
}
