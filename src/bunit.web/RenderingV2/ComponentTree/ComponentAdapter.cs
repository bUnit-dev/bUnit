using System.Diagnostics;
using AngleSharp.Dom;
using AngleSharp.Dom.Events;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Dom.Events;
using AngleSharp.Html.Parser;

namespace Bunit.RenderingV2.ComponentTree;

internal class ComponentAdapter
{
	private readonly List<ComponentAdapter> children;
	private readonly IDocument dom;
	private readonly HtmlParser htmlParser;
	private readonly BunitRenderer renderer;
	private readonly EventHandlerManager eventHandlerManager;
	private int latestUpdateNumber;

	public int ComponentId { get; }

	public IComponent Component { get; }

	public INode ParentElement { get; }

	public NodeSpan NodeSpan { get; private set; }

	public IReadOnlyList<ComponentAdapter> Children => children;

	public ComponentAdapter(
		int componentId,
		IComponent component,
		INode parentElement,
		HtmlParser htmlParser,
		BunitRenderer renderer,
		EventHandlerManager eventHandlerManager)
	{
		ComponentId = componentId;
		Component = component;
		ParentElement = parentElement;
		NodeSpan = nodeSpan;
		this.htmlParser = htmlParser;
		this.renderer = renderer;
		this.eventHandlerManager = eventHandlerManager;
		this.dom = parentElement.Owner!;
		children = new List<ComponentAdapter>();
	}

	public void AddChild(ComponentAdapter child)
		=> children.Add(child);

	public void ApplyEdits(in RenderTreeDiff updatedComponent, in RenderBatch renderBatch, int updateNumber)
	{
		if (latestUpdateNumber >= updateNumber)
		{
			return;
		}

		latestUpdateNumber = updateNumber;

		ApplyEdits(in updatedComponent, in renderBatch, this, this.ParentElement);
	}

	private void ApplyEdits(in RenderTreeDiff updatedComponent, in RenderBatch renderBatch, ComponentAdapter owner, INode parent)
	{
		foreach (var edit in updatedComponent.Edits)
		{
			switch (edit.Type)
			{
				case RenderTreeEditType.PrependFrame:
				{
					ApplyPrependFrame(edit.ReferenceFrameIndex, in renderBatch, owner, parent);
					break;
				}
				case RenderTreeEditType.RemoveFrame:
				{
					// TODO: What if sibling index pointed to Markup containing multiple elements?
					owner.NodeSpan.RemoveAt(edit.SiblingIndex, parent);
					// TODO: clean up event subscriptions? or does that happen later anyway?
					// TODO: somehow let users know that the node is no longer in the DOM,
					//       if they try to use it after it has been removed (e.g. assert based on it).
					break;
				}
				case RenderTreeEditType.RemoveAttribute:
				{
					ApplyRemoveAttribute(in edit, parent);
					break;
				}
				case RenderTreeEditType.SetAttribute:
				{
					ref readonly var frame = ref renderBatch.ReferenceFrames.Array[edit.ReferenceFrameIndex];

					var element = ReferenceEquals(parent, owner.ParentElement)
						? (IElement)owner.NodeSpan.GetNodeByFrameIndex(edit.SiblingIndex)
						: (IElement)parent.ChildNodes[edit.SiblingIndex];

					ApplySetAttribute(in frame, owner, element);
					break;
				}
				case RenderTreeEditType.UpdateText:
				{
					ApplyUpdateText(in edit, in renderBatch, parent);
					break;
				}
				// StepIn seems to be about going from the current containing element into a child
				// element based on the sibling index
				case RenderTreeEditType.StepIn:
				{
					parent = owner.NodeSpan.GetNodeByFrameIndex(edit.SiblingIndex);
					break;
				}
				// StepOut seems to just ask us to step up/out to the parent element.
				// TODO: Investigate if there can be multiple steps?
				case RenderTreeEditType.StepOut:
				{
					parent = parent.ParentElement!;
					break;
				}
				default:
					throw new NotImplementedException($"Edit type not supported: {edit.Type}");
			}
		}
	}

	private static void ApplyUpdateText(in RenderTreeEdit edit, in RenderBatch renderBatch, INode parent)
	{
		ref var frame = ref renderBatch.ReferenceFrames.Array[edit.ReferenceFrameIndex];
		var textNode = parent.ChildNodes[edit.SiblingIndex];
		textNode.TextContent = frame.TextContent;
	}

	private void ApplyPrependFrame(int referenceFrameIndex, in RenderBatch renderBatch, in ComponentAdapter owner, INode parent)
	{
		ref readonly var frame = ref renderBatch.ReferenceFrames.Array[referenceFrameIndex];
		switch (frame.FrameType)
		{
			case RenderTreeFrameType.Component:
			{
				ApplyComponent(referenceFrameIndex, in renderBatch, owner, parent);
				break;
			}
			case RenderTreeFrameType.Element:
			{
				ApplyElement(referenceFrameIndex, in renderBatch, owner, parent);
				break;
			}
			case RenderTreeFrameType.Markup:
			{
				ApplyMarkup(referenceFrameIndex, in renderBatch, owner, parent);
				break;
			}
			case RenderTreeFrameType.Text:
			{
				var text = owner.dom.CreateTextNode(frame.TextContent);
				parent.AppendChild(text);
				break;
			}
			default:
				throw new NotImplementedException($"Frame type not supported: {frame.FrameType}");
		}
	}

	private static void ApplyMarkup(int referenceFrameIndex, in RenderBatch renderBatch, ComponentAdapter owner, INode parent)
	{
		ref readonly var frame = ref renderBatch.ReferenceFrames.Array[referenceFrameIndex];

		// TODO: should we detect whitespace/newlines/text only and skip using ParseFragment for speed?
		//var text = string.IsNullOrWhiteSpace(frame.MarkupContent)
		//	? frame.MarkupContent
		//	: frame.MarkupContent.TrimEnd();

		var text = frame.MarkupContent;

		var markupNodes = owner.htmlParser.ParseFragment(text, (parent as IElement)!);
		owner.NodeSpan.Append(markupNodes, parent);
	}

	private void ApplyElement(int referenceFrameIndex, in RenderBatch renderBatch, ComponentAdapter owner, INode parent)
	{
		ref readonly var frame = ref renderBatch.ReferenceFrames.Array[referenceFrameIndex];

		var newElement = owner.dom.CreateElement(frame.ElementName);

		owner.NodeSpan.Append(newElement, parent);

		var endIndexExcl = referenceFrameIndex + frame.ElementSubtreeLength;
		for (var descendantIndex = referenceFrameIndex + 1; descendantIndex < endIndexExcl; descendantIndex++)
		{
			ref readonly var candidateFrame = ref renderBatch.ReferenceFrames.Array[descendantIndex];
			if (candidateFrame.FrameType == RenderTreeFrameType.Attribute)
			{
				ApplySetAttribute(in candidateFrame, owner, newElement);
			}
			else
			{
				// As soon as we see a non-attribute child, all the subsequent child frames are
				// not attributes, so bail out and insert the remnants recursively
				InsertFrameRange(descendantIndex, endIndexExcl, in renderBatch, owner, newElement);
				break;
			}
		}
	}

	private static void ApplyComponent(int referenceFrameIndex, in RenderBatch renderBatch, ComponentAdapter owner, INode containingElement)
	{
		ref readonly var frame = ref renderBatch.ReferenceFrames.Array[referenceFrameIndex];

		// TODO: figure out how to correct represent a components first and last node.
		var nodeSpan = new NodeSpan(owner.NodeSpan);

		var child = owner.renderer.CreateComponentAdapter(
			frame.ComponentId,
			frame.Component,
			containingElement,
			nodeSpan);

		owner.AddChild(child);

		for (var i = 0; i < renderBatch.UpdatedComponents.Count; i++)
		{
			ref readonly var componentEdits = ref renderBatch.UpdatedComponents.Array[i];
			if (componentEdits.ComponentId == frame.ComponentId && componentEdits.Edits.Count > 0)
			{
				child.ApplyEdits(in componentEdits, in renderBatch, owner.latestUpdateNumber);
			}
		}
	}

	private void InsertFrameRange(int startIndex, int endIndexExcl, in RenderBatch batch, in ComponentAdapter owner, IElement containingElement)
	{
		for (var frameIndex = startIndex; frameIndex < endIndexExcl; frameIndex++)
		{
			ApplyPrependFrame(frameIndex, in batch, owner, containingElement);

			// Skip over any descendants, since they are already dealt with recursively
			ref readonly var frame = ref batch.ReferenceFrames.Array[frameIndex];
			frameIndex += CountDescendantFrames(in frame);
		}

		static int CountDescendantFrames(in RenderTreeFrame frame) => frame.FrameType switch
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

#pragma warning disable MA0051 // Method is too long
	private void ApplySetAttribute(in RenderTreeFrame frame, ComponentAdapter owner, IElement element)
#pragma warning restore MA0051 // Method is too long
	{
		switch (frame.AttributeValue)
		{
			case Delegate:
			case EventCallback:
			{
				var eventHandlerId = frame.AttributeEventHandlerId;
				var eventName = frame.AttributeName;

				// TODO: Should we pass/create an EventFieldInfo in the event handler?
				// TODO: Can we handle async event handlers via the AngleSharp event dispatch system?
				DomEventHandler eventHandler = (sender, ev) =>
				{
					EventArgs blazorEvent;
					if (ev is BunitEvent be)
					{
						blazorEvent = be.BlazorEventArgs;
						var dispatchResult = owner.renderer.DispatchEventAsync(
							eventHandlerId,
							default(EventFieldInfo),
							be.BlazorEventArgs);

						be.AddEventHandlerTask(dispatchResult);
					}
					else
					{
						blazorEvent = Map(ev);
						owner.renderer.DispatchEventAsync(
							eventHandlerId,
							default(EventFieldInfo),
							blazorEvent);
					}
					ApplySideEffect(sender, blazorEvent);
				};

				element.AddEventListener(frame.AttributeName, eventHandler);
				eventHandlerManager.RegisterHandler(
					eventHandlerId,
					() => element.RemoveEventListener(eventName, eventHandler));

				break;
			}
			default:
			{
				element.SetAttribute(
					frame.AttributeName,
					frame.AttributeValue?.ToString() ?? string.Empty);
				break;
			}
		}

		static void ApplySideEffect(object node, EventArgs e)
		{
			// This applies side effects to DOM elements.
			// Is it a good idea? There is a related issue on this.
			// Could this cause problems when the DOM tree is updated?
			// Perhaps not, since a ApplySetAttribute would simply override the value.

			// TODO: Is there a way to get AngleSharp to do this?
			// TODO: Get all side effects implemented.
			switch (node)
			{
				case IHtmlInputElement input when e is KeyboardEventArgs kb:
				{
					input.SetAttribute("value", input.GetAttribute("value") + kb.Key);
					break;
				}
				default:
					break;
			}
		}

		static EventArgs Map(Event e)
		{
			return e switch
			{
				Event _ and { Type: "onclick" } or
				MouseEvent _ and { Type: "onclick" } => new MouseEventArgs()
				{
					Type = ToBlazorEventType(e.Type),
					Detail = 1
				},
				KeyboardEvent ke => new KeyboardEventArgs
				{
					Type = ToBlazorEventType(ke.Type),
					Key = ke.Key!,
				},
				Event _ and { Type: "onchange" } => new ChangeEventArgs(),
				InputEvent ie => new ChangeEventArgs
				{
					Value = ie.Data
				},
				Event _ => EventArgs.Empty,
				_ => throw new NotImplementedException($"Mapping for {e.Type} not implemented.")
			};

			// Strip out "on" from start of event type since that is what
			// Blazor expects. AngleSharp/HTML5 requires the "on" prefix.
			static string ToBlazorEventType(string type) => type[2..];
		}
	}

	private void ApplyRemoveAttribute(in RenderTreeEdit edit, INode parent)
	{
		Debug.Assert(edit.RemovedAttributeName is not null);

		var target = parent.ChildNodes[edit.SiblingIndex];

		if (target is not IElement element)
		{
			throw new InvalidOperationException($"Cannot remove an attribute from an '{target.GetType()}'. It must be an '{typeof(IElement)}'.");
		}

		element.RemoveAttribute(edit.RemovedAttributeName);
	}
}

