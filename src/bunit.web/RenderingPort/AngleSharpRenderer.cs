// This file is a port of the BrowserRenderer.ts in https://github.dev/dotnet/aspnetcore
// Version ported: https://github.dev/dotnet/aspnetcore/blob/8c5a59ac18d0d2d1ced5e247f5d0880650ef1ad8/src/Components/Web.JS/src/Rendering/BrowserRenderer.ts#L480
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Bunit.Rendering;
using Bunit.RenderingPort.Events;
using static Bunit.RenderingPort.LogicalElements;

namespace Bunit.RenderingPort;

internal sealed class AngleSharpRenderer : IDisposable
{
	private const string InternalAttributeNamePrefix = "__internal_";
	private const string EventStopPropagationAttributeNamePrefix = "stopPropagation_";
	private const string EventPreventDefaultAttributeNamePrefix = "preventDefault_";
	private const string BunitRootComponentTagName = "bunit-root-component";

	private readonly IHtmlTemplateElement sharedTemplateElemForParsing;
	private readonly IElement sharedSvgElemForParsing;
	private readonly Dictionary<int, LogicalElement> childComponentLocations = new();
	private readonly BunitHtmlParser htmlParser;
	private readonly EventDelegator eventDelegator;

	private readonly IDocument document;

	public AngleSharpRenderer(BunitRenderer renderer)
	{
		htmlParser = new BunitHtmlParser();
		document = htmlParser.CreateDocument();
		eventDelegator = new EventDelegator(document, renderer);

		sharedTemplateElemForParsing = (IHtmlTemplateElement)document.CreateElement("template");
		sharedSvgElemForParsing = document.CreateElement("http://www.w3.org/2000/svg", "g");
	}

	public void Dispose() => htmlParser.Dispose();


	internal RenderedComponent<RootComponent> InitializeRenderedComponent(int componentId, RootComponent component)
	{
		// Since the AngleSharpRenderer reuses a single DOM (document),
		// each time a new root component is rendered, it gets its own
		// root component, which is added as a child to the document.
		var parent = document.Body!.ToLogicalElement();
		var parentChildren = GetLogicalChildrenArray(parent);
		var childIndex = parentChildren.Count;

		var newRootElementRaw = document.CreateElement("bunit-root-component");
		newRootElementRaw.SetAttribute("componentId", componentId.ToString());
		var rootElement = newRootElementRaw.ToLogicalElement();

		childComponentLocations.Add(componentId, rootElement);
		InsertLogicalChild(newRootElementRaw, parent, childIndex);

		return new RenderedComponent<RootComponent>(component, newRootElementRaw.ChildNodes);
	}

	internal void DisposeComponent(int componentId)
	{
		if (childComponentLocations.TryGetValue(componentId, out var logicalElement))
		{
			childComponentLocations.Remove(componentId);

			if (logicalElement.Node.NodeName.Equals(BunitRootComponentTagName, StringComparison.OrdinalIgnoreCase))
			{
				document.RemoveChild(logicalElement.Node);
			}
		}
	}

	internal void DisposeEventHandler(ulong eventHandlerId)
		=> eventDelegator.RemoveListener(eventHandlerId);

	internal void UpdateComponent(in RenderBatch batch, int componentId, in ArrayBuilderSegment<RenderTreeEdit> edits, in RenderTreeFrame[] referenceFrames)
	{
		// Is this even possible here?
		if (!childComponentLocations.TryGetValue(componentId, out var element))
		{
			throw new InvalidOperationException($"No element is currently associated with component {componentId}");
		}

		// skipping logic to clear out existing nodes from DOM on first render.

		// skipping logic that gets currently focused element to reset focus after render.

		ApplyEdits(in batch, componentId, element, childIndex: 0, in edits, in referenceFrames);

		// skipping logic that sets focus back to element
	}

#pragma warning disable MA0051 // Method is too long
	private void ApplyEdits(in RenderBatch batch, int componentId, LogicalElement parent, int childIndex, in ArrayBuilderSegment<RenderTreeEdit> edits, in RenderTreeFrame[] referenceFrames)
#pragma warning restore MA0051 // Method is too long
	{
		var currentDepth = 0;
		var childIndexAtCurrentDepth = childIndex;
		var permutationList = default(List<PermutationListEntry>);

		// EGH: all this offset/count logic is built into ArrayBuilderSegment
		// var editsOffset = edits.Offset;
		// var editsLength = edits.Count;
		// var maxEditIndexExcl = editsOffset + editsLength;

		for (var editIndex = 0; editIndex < edits.Count; editIndex++)
		{
			var edit = edits[editIndex];
			var editType = edit.Type;
			switch (editType)
			{
				case RenderTreeEditType.PrependFrame:
				{
					var frameIndex = edit.ReferenceFrameIndex;
					ref readonly var frame = ref referenceFrames[frameIndex];
					var siblingIndex = edit.SiblingIndex;
					InsertFrame(in batch, componentId, parent, childIndexAtCurrentDepth + siblingIndex, in referenceFrames, in frame, frameIndex);
					break;
				}
				case RenderTreeEditType.RemoveFrame:
				{
					var siblingIndex = edit.SiblingIndex;
					RemoveLogicalChild(parent, childIndexAtCurrentDepth + siblingIndex);
					break;
				}
				case RenderTreeEditType.SetAttribute:
				{
					var frameIndex = edit.ReferenceFrameIndex;
					ref readonly var frame = ref referenceFrames[frameIndex];
					var siblingIndex = edit.SiblingIndex;
					var logicalElement = GetLogicalChild(parent, childIndexAtCurrentDepth + siblingIndex);
					if (logicalElement.Node is IElement element)
					{
						ApplyAttribute(in batch, componentId, element, in frame);
					}
					else
					{
						throw new InvalidOperationException("Cannot set attribute on non-element child");
					}
					break;
				}
				case RenderTreeEditType.RemoveAttribute:
				{
					// Note that we don't have to dispose the info we track about event handlers here, because the
					// disposed event handler IDs are delivered separately (in the 'disposedEventHandlerIds' array)
					var siblingIndex = edit.SiblingIndex;
					var element = GetLogicalChild(parent, childIndexAtCurrentDepth + siblingIndex);
					if (element.Node is IElement htmlElm)
					{
						var attributeName = edit.RemovedAttributeName!;

						// First try to remove any special property we use for this attribute
						if (!TryApplySpecialProperty(in batch, htmlElm, attributeName, default))
						{
							// If that's not applicable, it's a regular DOM attribute so remove that
							htmlElm.RemoveAttribute(attributeName);
						}
					}
					else
					{
						throw new InvalidOperationException("Cannot remove attribute from non-element child");
					}
					break;
				}
				case RenderTreeEditType.UpdateText:
				{
					var frameIndex = edit.ReferenceFrameIndex;
					ref readonly var frame = ref referenceFrames[frameIndex];
					var siblingIndex = edit.SiblingIndex;
					var textNode = GetLogicalChild(parent, childIndexAtCurrentDepth + siblingIndex);
					if (textNode.Node is IText text)
					{
						text.TextContent = frame.TextContent;
					}
					else
					{
						throw new InvalidOperationException("Cannot set text content on non-text child");
					}
					break;
				}
				case RenderTreeEditType.UpdateMarkup:
				{
					var frameIndex = edit.ReferenceFrameIndex;
					ref readonly var frame = ref referenceFrames[frameIndex];
					var siblingIndex = edit.SiblingIndex;
					RemoveLogicalChild(parent, childIndexAtCurrentDepth + siblingIndex);
					InsertMarkup(in batch, parent, childIndexAtCurrentDepth + siblingIndex, in frame);
					break;
				}
				case RenderTreeEditType.StepIn:
				{
					var siblingIndex = edit.SiblingIndex;
					parent = GetLogicalChild(parent, childIndexAtCurrentDepth + siblingIndex);
					currentDepth++;
					childIndexAtCurrentDepth = 0;
					break;
				}
				case RenderTreeEditType.StepOut:
				{
					parent = GetLogicalParent(parent)!;
					currentDepth--;
					childIndexAtCurrentDepth = currentDepth == 0 ? childIndex : 0; // The childIndex is only ever nonzero at zero depth
					break;
				}
				case RenderTreeEditType.PermutationListEntry:
				{
					permutationList ??= new List<PermutationListEntry>();
					permutationList.Add(new(childIndexAtCurrentDepth + edit.SiblingIndex,
						childIndexAtCurrentDepth + edit.MoveToSiblingIndex));
					break;
				}
				case RenderTreeEditType.PermutationListEnd:
				{
					PermuteLogicalChildren(parent, permutationList!);
					permutationList = null;
					break;
				}
				default:
				{
					throw new InvalidOperationException($"Unknown edit type: {editType}");
				}
			}
		}
	}

	private int InsertFrame(in RenderBatch batch, int componentId, LogicalElement parent, int childIndex, in RenderTreeFrame[] frames, in RenderTreeFrame frame, int frameIndex)
	{
		var frameType = frame.FrameType;
		switch (frameType)
		{
			case RenderTreeFrameType.Element:
				InsertElement(in batch, componentId, parent, childIndex, in frames, in frame, frameIndex);
				return 1;
			case RenderTreeFrameType.Text:
				InsertText(in batch, parent, childIndex, in frame);
				return 1;
			case RenderTreeFrameType.Attribute:
				throw new InvalidOperationException("Attribute frames should only be present as leading children of element frames.");
			case RenderTreeFrameType.Component:
				InsertComponent(in batch, parent, childIndex, in frame);
				return 1;
			case RenderTreeFrameType.Region:
				return InsertFrameRange(in batch, componentId, parent, childIndex, in frames, frameIndex + 1, frameIndex + frame.RegionSubtreeLength);
			case RenderTreeFrameType.ElementReferenceCapture:
				if (parent.Node is IElement element)
				{
					// this is what applyCaptureIdToElement does.
					element.SetAttribute($"_bl_{frame.ElementReferenceCaptureId}", string.Empty);
					return 0; // A "capture" is a child in the diff, but has no node in the DOM
				}
				else
				{
					throw new InvalidOperationException("Reference capture frames can only be children of element frames.");
				}
			case RenderTreeFrameType.Markup:
				InsertMarkup(in batch, parent, childIndex, in frame);
				return 1;
			default:
				throw new InvalidOperationException($"Unknown frame type: {frameType}");
		}
	}

#pragma warning disable S1172 // Unused method parameters should be removed
	private void InsertText(in RenderBatch batch, LogicalElement parent, int childIndex, in RenderTreeFrame frame)
#pragma warning restore S1172 // Unused method parameters should be removed
	{
		var textContent = frame.TextContent;
		var newTextNode = parent.GetDocument().CreateTextNode(textContent);
		InsertLogicalChild(newTextNode, parent, childIndex);
	}

#pragma warning disable S1172 // Unused method parameters should be removed
	private void InsertComponent(in RenderBatch batch, LogicalElement parent, int childIndex, in RenderTreeFrame frame)
#pragma warning restore S1172 // Unused method parameters should be removed
	{
		var containerElement = CreateAndInsertLogicalContainer(parent, childIndex);

		// All we have to do is associate the child component ID with its location. We don't actually
		// do any rendering here, because the diff for the child will appear later in the render batch.
		var childComponentId = frame.ComponentId;
		AttachComponentToElement(childComponentId, containerElement);
	}

	private void AttachComponentToElement(int componentId, LogicalElement element)
	{
		childComponentLocations.Add(componentId, element);
	}

#pragma warning disable S1172 // Unused method parameters should be removed
	private void InsertMarkup(in RenderBatch batch, LogicalElement parent, int childIndex, in RenderTreeFrame frame)
#pragma warning restore S1172 // Unused method parameters should be removed
	{
		var markupContainer = CreateAndInsertLogicalContainer(parent, childIndex);

		var markupContent = frame.MarkupContent;
		var parsedMarkup = ParseMarkup(markupContent, (IElement)GetClosestDomElement(parent)/*IsSvgElement(parent)*/);
		var logicalSiblingIndex = 0;

		// AngleSharp only allow nodes to attached to one parent, so
		// whenever the node is inserted into another node list, it is
		// removed from the parsedMarkup element.
		while (parsedMarkup.Length > 0)
		{
			InsertLogicalChild(parsedMarkup[0], markupContainer, logicalSiblingIndex++);
		}
	}

	private INodeList ParseMarkup(string markup, IElement parent)
	{
		// This approach to parsing markup does not seem to be supported by AngleSharp.
		// if (isSvg)
		// {
		// 	sharedSvgElemForParsing.InnerHtml = markup ?? " ";
		// 	return sharedSvgElemForParsing;
		// }
		// else
		// {
		// 	sharedTemplateElemForParsing.InnerHtml = markup ?? " ";
		// 	return sharedTemplateElemForParsing.Content;
		// }

		return htmlParser.ParseFragment(markup, parent);
	}

	private void InsertElement(in RenderBatch batch, int componentId, LogicalElement parent, int childIndex, in RenderTreeFrame[] frames, in RenderTreeFrame frame, int frameIndex)
	{
		var tagName = frame.ElementName;

		var newDomElementRaw = tagName == "svg" || IsSvgElement(parent)
			? document.CreateElement("http://www.w3.org/2000/svg", tagName)
			: document.CreateElement(tagName);
		var newElement = newDomElementRaw.ToLogicalElement();

		var inserted = false;

		// Apply attributes
		var descendantsEndIndexExcl = frameIndex + frame.ElementSubtreeLength;
		for (int descendantIndex = frameIndex + 1; descendantIndex < descendantsEndIndexExcl; descendantIndex++)
		{
			ref readonly var descendantFrame = ref frames[descendantIndex];
			if (descendantFrame.FrameType == RenderTreeFrameType.Attribute)
			{
				ApplyAttribute(in batch, componentId, newDomElementRaw, in descendantFrame);
			}
			else
			{
				InsertLogicalChild(newDomElementRaw, parent, childIndex);
				inserted = true;

				// As soon as we see a non-attribute child, all the subsequent child frames are
				// not attributes, so bail out and insert the remnants recursively
				InsertFrameRange(in batch, componentId, newElement, 0, in frames, descendantIndex, descendantsEndIndexExcl);
				break;
			}
		}

		// this element did not have any children, so it's not inserted yet.
		if (!inserted)
		{
			InsertLogicalChild(newDomElementRaw, parent, childIndex);
		}

		// We handle setting 'value' on a <select> in three different ways:
		// [1] When inserting a corresponding <option>, in case you're dynamically adding options.
		//     This is the case below.
		// [2] After we finish inserting the <select>, in case the descendant options are being
		//     added as an opaque markup block rather than individually. This is the other case below.
		// [3] In case the the value of the select and the option value is changed in the same batch.
		//     We just receive an attribute frame and have to set the select value afterwards.

		// We also defer setting the 'value' property for <input> because certain types of inputs have
		// default attribute values that may incorrectly constain the specified 'value'.
		// For example, range inputs have default 'min' and 'max' attributes that may incorrectly
		// clamp the 'value' property if it is applied before custom 'min' and 'max' attributes.

		if (newDomElementRaw is IHtmlOptionElement htmlOptionElement)
		{
			// Situation 1
			TrySetSelectValueFromOptionElement(htmlOptionElement);
		}
		else if (newDomElementRaw.ToLogicalElement().DeferredValue is string deferredValue)
		{
			// Situation 2
			SetDeferredElementValue(newDomElementRaw, deferredValue);
		}
	}

	private int InsertFrameRange(in RenderBatch batch, int componentId, LogicalElement parent, int childIndex, in RenderTreeFrame[] frames, int startIndex, int endIndexExcl)
	{
		var origChildIndex = childIndex;
		for (var index = startIndex; index < endIndexExcl; index++)
		{
			ref readonly var frame = ref frames[index];
			var numChildrenInserted = InsertFrame(batch, componentId, parent, childIndex, frames, frame, index);
			childIndex += numChildrenInserted;

			// Skip over any descendants, since they are already dealt with recursively
			index += CountDescendantFrames(in frame);
		}

		return childIndex - origChildIndex; // Total number of children inserted
	}

	private int CountDescendantFrames(in RenderTreeFrame frame)
	{
		return frame.FrameType switch
		{
			// The following frame types have a subtree length. Other frames may use that memory slot
			// to mean something else, so we must not read it. We should consider having nominal subtypes
			// of RenderTreeFramePointer that prevent access to non-applicable fields.
			RenderTreeFrameType.Component => frame.ComponentSubtreeLength - 1,
			RenderTreeFrameType.Element => frame.ElementSubtreeLength - 1,
			RenderTreeFrameType.Region => frame.RegionSubtreeLength - 1,
			_ => 0,
		};
	}

	private void ApplyAttribute(in RenderBatch batch, int componentId, IElement toDomElement, in RenderTreeFrame attributeFrame)
	{
		var attributeName = attributeFrame.AttributeName;
		var eventHandlerId = attributeFrame.AttributeEventHandlerId;

		// TODO: verify  that event handler id us 0 when
		// the attribute is not an event handler
		if (eventHandlerId != 0)
		{
			var eventName = StripOnPrefix(attributeName);
			eventDelegator.SetListener(toDomElement, eventName, eventHandlerId, componentId);
			return;
		}

		// First see if we have special handling for this attribute
		if (!TryApplySpecialProperty(in batch, toDomElement, attributeName, in attributeFrame))
		{
			toDomElement.SetAttribute(
				attributeName,
				attributeFrame.AttributeValue.ToString()!);
		}
	}

	private bool TryApplySpecialProperty(in RenderBatch batch, IElement element, string attributeName, in RenderTreeFrame attributeFrame)
	{
		switch (attributeName)
		{
			case "value":
				return TryApplyValueProperty(in batch, element, in attributeFrame);
			case "checked":
				return TryApplyCheckedProperty(in batch, element, in attributeFrame);
			default:
			{
				// The renderer in the browser does not set a value for an attribute
				// if the value is a boolean and its "true".
				// In Blazor the BrowserRenderer does not receive the boolean value,
				// it receives an empty string when the boolean is true.
				// If the boolean value is false, no attribute is set at all.
				if (attributeFrame.AttributeValue is bool value)
				{
					if (value)
					{
						element.SetAttribute(attributeName, string.Empty);
					}

					return true;
				}

				if (attributeName.StartsWith(InternalAttributeNamePrefix))
				{
					ApplyInternalAttribute(in batch, element, attributeName.Substring(InternalAttributeNamePrefix.Length), in attributeFrame);
					return true;
				}
				return false;
			}
		}
	}

#pragma warning disable S1172 // Unused method parameters should be removed
	private bool TryApplyValueProperty(in RenderBatch batch, IElement element, in RenderTreeFrame attributeFrame)
#pragma warning restore S1172 // Unused method parameters should be removed
	{
		var value = attributeFrame.AttributeValue;

		if (value is string valueString && element.TagName == "INPUT")
		{
			value = NormalizeInputValue(valueString, element);
		}

		switch (element.TagName)
		{
			case "INPUT":
			case "SELECT":
			case "TEXTAREA":
			{
				// <select> is special, in that anything we write to .value will be lost if there
				// isn't yet a matching <option>. To maintain the expected behavior no matter the
				// element insertion/update order, preserve the desired value separately so
				// we can recover it when inserting any matching <option> or after inserting an
				// entire markup block of descendants.

				// We also defer setting the 'value' property for <input> because certain types of inputs have
				// default attribute values that may incorrectly contain the specified 'value'.
				// For example, range inputs have default 'min' and 'max' attributes that may incorrectly
				// clamp the 'value' property if it is applied before custom 'min' and 'max' attributes.

				if (value is not null && element is IHtmlSelectElement selectElement && IsMultipleSelectElement(selectElement))
				{
					// TODO: figure out if we need to do a JSON.parse thing here
					//       or thing content is already an object.
					//value = value;
				}

				SetDeferredElementValue(element, value);

				var logicalElement = element.ToLogicalElement();
				logicalElement.DeferredValue = value?.ToString();

				return true;
			}
			case "OPTION":
			{
				if (string.Empty.Equals(value))
				{
					element.SetAttribute("value", value.ToString() ?? string.Empty);
				}
				else
				{
					element.RemoveAttribute("value");
				}

				// See above for why we have this special handling for <select>/<option>
				// Situation 3
				TrySetSelectValueFromOptionElement((IHtmlOptionElement)element);
				return true;
			}
			default:
				return false;
		}
	}

#pragma warning disable S3241 // Methods should not return values that are never used
	private bool TrySetSelectValueFromOptionElement(IHtmlOptionElement optionElement)
#pragma warning restore S3241 // Methods should not return values that are never used
	{
		var selectElem = FindClosestAncestorSelectElement(optionElement);

		if (!IsBlazorSelectElement(selectElem))
		{
			return false;
		}

		var logicalSelectElement = selectElem.ToLogicalElement();

		if (IsMultipleSelectElement(selectElem))
		{
			optionElement.IsSelected = selectElem
				.ToLogicalElement()
				.DeferredValue!
				.Contains(optionElement.Value);
		}
		else
		{
			if (logicalSelectElement.DeferredValue != optionElement.Value)
			{
				return false;
			}

			SetSingleSelectElementValue(selectElem, optionElement.Value);
			logicalSelectElement.DeferredValue = null;
		}

		return true;

		static bool IsBlazorSelectElement([NotNullWhen(true)] IHtmlSelectElement? selectElem)
			=> selectElem?.ToLogicalElement().DeferredValue is not null;
	}


	private IHtmlSelectElement? FindClosestAncestorSelectElement(IElement? element)
	{
		while (element is not null)
		{
			if (element is IHtmlSelectElement selectElement)
			{
				return selectElement;
			}
			element = element.ParentElement;
		}
		return default;
	}

	private void SetDeferredElementValue(IElement element, object? value)
	{

		if (element is IHtmlSelectElement selectElement)
		{
			if (IsMultipleSelectElement(selectElement))
			{
				SetMultipleSelectElementValue(selectElement, value as IEnumerable<string>);
			}
			else
			{
				SetSingleSelectElementValue(selectElement, value);
			}
		}
		else
		{
			((IHtmlInputElement)element).Value = value?.ToString() ?? string.Empty;
		}
	}

	private void SetSingleSelectElementValue(IHtmlSelectElement element, object? value)
	{
		// There's no sensible way to represent a select option with value 'null', because
		// (1) HTML attributes can't have null values - the closest equivalent is absence of the attribute
		// (2) When picking an <option> with no 'value' attribute, the browser treats the value as being the
		//     *text content* on that <option> element. Trying to suppress that default behavior would involve
		//     a long chain of special-case hacks, as well as being breaking vs 3.x.
		// So, the most plausible 'null' equivalent is an empty string. It's unfortunate that people can't
		// write <option value=@someNullVariable>, and that we can never distinguish between null and empty
		// string in a bound <select>, but that's a limit in the representational power of HTML.
		element.Value = value?.ToString() ?? string.Empty;
	}

	private void SetMultipleSelectElementValue(IHtmlSelectElement element, IEnumerable<string>? value)
	{
		value ??= Array.Empty<string>();
		for (var i = 0; i < element.Options.Length; i++)
		{
			element.Options[i].IsSelected = value.Contains(element.Options[i].Value);
		}
	}

#pragma warning disable S1172 // Unused method parameters should be removed
	private bool TryApplyCheckedProperty(in RenderBatch batch, IElement element, in RenderTreeFrame attributeFrame)
#pragma warning restore S1172 // Unused method parameters should be removed
	{
		if (element.TagName == "INPUT" && element is IHtmlInputElement inputElement)
		{
			var value = attributeFrame.AttributeValue;
			inputElement.IsChecked = value != null;
			return true;
		}
		return false;
	}

#pragma warning disable S1172 // Unused method parameters should be removed
	private void ApplyInternalAttribute(in RenderBatch batch, IElement element, string internalAttributeName, in RenderTreeFrame attributeFrame)
#pragma warning restore S1172 // Unused method parameters should be removed
	{
		var attributeValue = attributeFrame.AttributeValue;

		if (internalAttributeName.StartsWith(EventStopPropagationAttributeNamePrefix))
		{
			// Stop propagation
			var eventName = StripOnPrefix(internalAttributeName.Substring(EventStopPropagationAttributeNamePrefix.Length));
			eventDelegator.SetStopPropagation(element, eventName, attributeValue != null);
		}
		else if (internalAttributeName.StartsWith(EventPreventDefaultAttributeNamePrefix))
		{
			// Prevent default
			var eventName = StripOnPrefix(internalAttributeName.Substring(EventPreventDefaultAttributeNamePrefix.Length));
			eventDelegator.SetPreventDefault(element, eventName, attributeValue != null);
		}
		else
		{
			// The prefix makes this attribute name reserved, so any other usage is disallowed
			throw new InvalidOperationException($"Unsupported internal attribute '{internalAttributeName}'");
		}
	}

	private string StripOnPrefix(string attributeName)
	{
		if (attributeName.StartsWith("on", StringComparison.OrdinalIgnoreCase))
		{
			return attributeName.Substring(2);
		}

		throw new InvalidOperationException($"Attribute should be an event name, but doesn't start with 'on'. Value: '{attributeName}'");
	}

	private bool IsMultipleSelectElement([NotNullWhen(true)] IHtmlSelectElement? element)
	{
		return element?.Type == "select-multiple";
	}

	private string NormalizeInputValue(string value, IElement element)
	{
		// Time inputs (e.g. 'time' and 'datetime-local') misbehave on chromium-based
		// browsers when a time is set that includes a seconds value of '00', most notably
		// when entered from keyboard input. This behavior is not limited to specific
		// 'step' attribute values, so we always remove the trailing seconds value if the
		// time ends in '00'.
		// Similarly, if a time-related element doesn't have any 'step' attribute, browsers
		// treat this as "round to whole number of minutes" making it invalid to pass any
		// 'seconds' value, so in that case we strip off the 'seconds' part of the value.

		switch (element.GetAttribute("type"))
		{
			case "time":
				return value.Length == 8 && (value.EndsWith("00") || !element.HasAttribute("step"))
				  ? value.Substring(0, 5)
				  : value;
			case "datetime-local":
				return value.Length == 19 && (value.EndsWith("00") || !element.HasAttribute("step"))
				  ? value.Substring(0, 16)
				  : value;
			default:
				return value;
		}
	}
}
