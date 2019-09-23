using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;

namespace Egil.RazorComponents.Testing.Render
{
    [SuppressMessage("Usage", "BL0006:Do not use RenderTree types", Justification = "<Pending>")]
    internal class Htmlizer
    {
        private static readonly HtmlEncoder _htmlEncoder = HtmlEncoder.Default;

        private static readonly HashSet<string> _selfClosingElements = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "area", "base", "br", "col", "embed", "hr", "img", "input", "link", "meta", "param", "source", "track", "wbr"
        };

        public static string GetHtml(TestRenderer renderer, int componentId)
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
                    context.Result.Add(_htmlEncoder.Encode(frame.TextContent));
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
                if (_selfClosingElements.Contains(frame.ElementName))
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
                    result.Add($" {frame.AttributeName}=\"{frame.AttributeEventHandlerId}\"");
                    continue;
                }

                switch (frame.AttributeValue)
                {
                    case bool flag when flag:
                        result.Add(" ");
                        result.Add(frame.AttributeName);
                        break;
                    case string value:
                        result.Add(" ");
                        result.Add(frame.AttributeName);
                        result.Add("=");
                        result.Add("\"");
                        result.Add(_htmlEncoder.Encode(value));
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
            public HtmlRenderingContext(TestRenderer renderer)
            {
                Renderer = renderer;
            }

            public TestRenderer Renderer { get; }

            public List<string> Result { get; } = new List<string>();

            public string? ClosestSelectValueAsString { get; set; }
        }
    }
}
