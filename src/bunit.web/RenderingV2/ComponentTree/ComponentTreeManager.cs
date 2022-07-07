using System.Diagnostics;
using AngleSharp.Dom;
using AngleSharp.Dom.Events;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Dom.Events;
using Bunit.Rendering;

namespace Bunit.RenderingV2.ComponentTree;

internal sealed class ComponentTreeManager : IDisposable
{
	internal readonly record struct TreeNode(int TreeUpdateCount, ComponentTreeNode Node);
	private readonly Dictionary<int, TreeNode> treeNodes = new();
	private readonly EventHandlerManager eventHandlerManager;
	private readonly BunitHtmlParser htmlParser;
	private readonly Renderer renderer;
	private int treeUpdateBatchCount;

	public ComponentTreeManager(Renderer renderer)
	{
		htmlParser = new BunitHtmlParser();
		this.renderer = renderer;
		eventHandlerManager = new();
	}

	internal ComponentTreeNode CreateTreeRoot(int componentId, RootComponent component)
	{
		var dom = htmlParser.CreateDocument();
		var rootNode = new ComponentTreeNode(componentId, component, dom.Body!);
		treeNodes.Add(componentId, new(0, rootNode));
		return rootNode;
	}

	private ComponentTreeNode CreateTreeNode(int componentId, IComponent component, INode parent)
	{
		var node = new ComponentTreeNode(componentId, component, parent);
		treeNodes.Add(componentId, new(0, node));
		return node;
	}

	internal void UpdateComponentTrees(in RenderBatch renderBatch)
	{
		treeUpdateBatchCount++;

		var numUpdatedComponents = renderBatch.UpdatedComponents.Count;
		for (var componentIndex = 0; componentIndex < numUpdatedComponents; componentIndex++)
		{
			var updatedComponent = renderBatch.UpdatedComponents.Array[componentIndex];

			if (updatedComponent.Edits.Count > 0)
			{
				UpdateTree(in updatedComponent, in renderBatch);
			}
		}

		var numDisposedComponents = renderBatch.DisposedComponentIDs.Count;
		for (var i = 0; i < numDisposedComponents; i++)
		{
			var disposedComponentId = renderBatch.DisposedComponentIDs.Array[i];
			DisposeComponent(disposedComponentId);
		}

		var numDisposeEventHandlers = renderBatch.DisposedEventHandlerIDs.Count;
		for (var i = 0; i < numDisposeEventHandlers; i++)
		{
			var disposedEventHandlerId = renderBatch.DisposedEventHandlerIDs.Array[i];
			eventHandlerManager.DisposeHandler(disposedEventHandlerId);
		}
	}

	private void UpdateTree(in RenderTreeDiff updatedComponent, in RenderBatch renderBatch)
	{
		if (!treeNodes.TryGetValue(updatedComponent.ComponentId, out var treeNode))
		{
			// TODO: log warning
			return;
		}

		if (treeNode.TreeUpdateCount < treeUpdateBatchCount)
		{
			treeNodes[updatedComponent.ComponentId] = treeNode with { TreeUpdateCount = treeUpdateBatchCount };
			ApplyEdits(in updatedComponent, in renderBatch, treeNode.Node, treeNode.Node.ParentElement);
		}
	}

	private void DisposeComponent(int componentId)
		=> throw new NotImplementedException();

#pragma warning disable MA0051 // Method is too long
	private void ApplyEdits(in RenderTreeDiff updatedComponent, in RenderBatch renderBatch, ComponentTreeNode owner, INode parent)
#pragma warning restore MA0051 // Method is too long
	{
		foreach (var edit in updatedComponent.Edits)
		{
			switch (edit.Type)
			{
				case RenderTreeEditType.PrependFrame:
				{
					// Prepending frames means adding new nodes or components to the component tree
					// and/or document object model.
					ApplyPrependFrame(edit.ReferenceFrameIndex, in renderBatch, owner, parent);
					break;
				}
				case RenderTreeEditType.RemoveFrame:
				{
					// Remove frame normally points to a single element or node to be removed.
					// However, if a frame was a Markup frame, then it could include one or
					// more nodes. Therefore we must not just remove the node at the index,
					// but all nodes that was part of the frame originally.
					//
					// Removing frames changes the sibling index of later frames added to
					// the DOM tree. E.g. removing the first frame at index = 0, means that
					// the current frame at index = 1 will now be referred to as the frame
					// at index = 0 after.
					parent.RemoveChildFrame(edit.SiblingIndex);

					// TODO: clean up event subscriptions? or does that happen later anyway?
					// TODO: Somehow let users know that the node is no longer in the DOM,
					//       This could be exposed through an extensions method, e.g.
					//       node.IsAttached().
					break;
				}
				case RenderTreeEditType.RemoveAttribute:
				{
					ApplyRemoveAttribute(in edit, parent);
					break;
				}
				case RenderTreeEditType.SetAttribute:
				{
					ApplySetAttribute(in edit, in renderBatch, parent);
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
					parent = parent.GetChildNodeByFrameIndex(edit.SiblingIndex);
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

	private void ApplyUpdateText(in RenderTreeEdit edit, in RenderBatch renderBatch, INode parent)
	{
		ref var frame = ref renderBatch.ReferenceFrames.Array[edit.ReferenceFrameIndex];

		// TOOD: do we always need to get a child node from the parent, or can
		//       set attribute be targeted at the current parent?
		var textNode = parent.GetChildNodeByFrameIndex(edit.SiblingIndex);
		textNode.TextContent = frame.TextContent;
	}

	private void ApplyPrependFrame(int referenceFrameIndex, in RenderBatch renderBatch, in ComponentTreeNode owner, INode parent)
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
				ApplyText(in frame, owner, parent);
				break;
			}
			default:
				throw new NotImplementedException($"Frame type not supported: {frame.FrameType}");
		}
	}

	private void ApplyComponent(int referenceFrameIndex, in RenderBatch renderBatch, ComponentTreeNode owner, INode parent)
	{
		ref readonly var frame = ref renderBatch.ReferenceFrames.Array[referenceFrameIndex];

		var child = CreateTreeNode(
			frame.ComponentId,
			frame.Component,
			parent);

		owner.AddChild(child);

		// Recursively apply any edits to the created component that exists in the
		// current render batch.
		// I.e. a depth first creation of the component tree.
		for (var i = 0; i < renderBatch.UpdatedComponents.Count; i++)
		{
			ref readonly var componentEdits = ref renderBatch.UpdatedComponents.Array[i];
			if (componentEdits.ComponentId == frame.ComponentId && componentEdits.Edits.Count > 0)
			{
				UpdateTree(in componentEdits, in renderBatch);
				break;
			}
		}
	}

	private void ApplyElement(int referenceFrameIndex, in RenderBatch renderBatch, ComponentTreeNode owner, INode parent)
	{
		ref readonly var frame = ref renderBatch.ReferenceFrames.Array[referenceFrameIndex];

		var element = CreateElement(owner, frame.ElementName);
		parent.AppendChildFrame(element, owner);

		// Process all reference frames related to the element.
		// This helps build the component and DOM tree depth first.
		var endIndexExclusive = referenceFrameIndex + frame.ElementSubtreeLength;
		for (var descendantIndex = referenceFrameIndex + 1; descendantIndex < endIndexExclusive; descendantIndex++)
		{
			ref readonly var candidateFrame = ref renderBatch.ReferenceFrames.Array[descendantIndex];
			if (candidateFrame.FrameType == RenderTreeFrameType.Attribute)
			{
				ApplySetAttribute(in candidateFrame, element);
			}
			else
			{
				// As soon as we see a non-attribute child, all the subsequent child frames are
				// not attributes, so bail out and insert the remnants recursively
				InsertFrameRange(descendantIndex, endIndexExclusive, in renderBatch, owner, element);
				break;
			}
		}
	}

	private void ApplyMarkup(int referenceFrameIndex, in RenderBatch renderBatch, ComponentTreeNode owner, INode parent)
	{
		ref readonly var frame = ref renderBatch.ReferenceFrames.Array[referenceFrameIndex];
		var markupNodes = ParseMarkup(frame.TextContent, parent as IElement);
		parent.AppendChildFrame(markupNodes, owner);
	}

	private void ApplyText(in RenderTreeFrame frame, ComponentTreeNode owner, INode parent)
	{
		var text = CreateTextNode(frame.TextContent, owner);
		parent.AppendChildFrame(text, owner);
	}

	private void InsertFrameRange(int startIndex, int endIndexExcl, in RenderBatch batch, in ComponentTreeNode owner, IElement containingElement)
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

	private void ApplySetAttribute(in RenderTreeEdit edit, in RenderBatch renderBatch, INode parent)
	{
		ref readonly var frame = ref renderBatch.ReferenceFrames.Array[edit.ReferenceFrameIndex];

		// TOOD: do we always need to get a child node from the parent, or can
		//       set attribute be targeted at the current parent?
		var node = parent.GetChildNodeByFrameIndex(edit.SiblingIndex);
		if (node is not IElement element)
		{
			throw new InvalidOperationException($"Cannot apply an attribute to a '{node.GetType()}'. Attributes can only be applied to '{typeof(IElement)}' types.");
		}

		ApplySetAttribute(in frame, element);
	}

#pragma warning disable MA0051 // Method is too long
	private void ApplySetAttribute(in RenderTreeFrame frame, IElement element)
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
						var dispatchResult = renderer.DispatchEventAsync(
							eventHandlerId,
							default(EventFieldInfo),
							be.BlazorEventArgs);

						be.AddEventHandlerTask(dispatchResult);
					}
					else
					{
						blazorEvent = Map(ev);
						renderer.DispatchEventAsync(
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

	private static void ApplyRemoveAttribute(in RenderTreeEdit edit, INode parent)
	{
		Debug.Assert(edit.RemovedAttributeName is not null);

		var target = parent.GetChildNodeByFrameIndex(edit.SiblingIndex);

		if (target is not IElement element)
		{
			throw new InvalidOperationException($"Cannot remove an attribute from an '{target.GetType()}'. It must be an '{typeof(IElement)}'.");
		}

		element.RemoveAttribute(edit.RemovedAttributeName);
	}

	private INodeList ParseMarkup(string markupText, IElement? contextElement)
	{
		// if context element is null the parser will create one for us.
		return htmlParser.ParseFragment(markupText, contextElement!);
	}

	private IElement CreateElement(ComponentTreeNode owner, string elementName)
	{
		// The parent elements owner (IDocument) I known to not be null.
		return owner.ParentElement.Owner!.CreateElement(elementName);
	}

	private IText CreateTextNode(string textContent, ComponentTreeNode owner)
	{
		// The parent elements owner (IDocument) I known to not be null.
		return owner.ParentElement.Owner!.CreateTextNode(textContent);
	}

	public void Dispose()
	{
		htmlParser.Dispose();
	}
}
