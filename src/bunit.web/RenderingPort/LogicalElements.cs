// This file is a port of the LogicalElements.ts in https://github.dev/dotnet/aspnetcore
// Version ported: https://github.dev/dotnet/aspnetcore/blob/82eb6803e777e2e6c817b51ac6b3bcb5410b4236/src/Components/Web.JS/src/Rendering/LogicalElements.ts

using System.Runtime.CompilerServices;
using AngleSharp.Dom;
using Bunit.RenderingPort.Events;

namespace Bunit.RenderingPort;

/*

LogicalElements in Blazor works by dynamically attaching
additional properties to DOM nodes, i.e. these three:

- logicalChildrenPropname
- logicalParentPropname
- logicalEndSiblingPropname

*/
internal static class LogicalElements
{
	private static readonly ConditionalWeakTable<INode, LogicalElement> MetadataStorage
		= new ConditionalWeakTable<INode, LogicalElement>();

	public static LogicalElement ToLogicalElement(this INode element)
	{
		if (!MetadataStorage.TryGetValue(element, out var result))
		{
			result = new LogicalElement(element);
			MetadataStorage.Add(element, result);
		}

		return result;
	}

	public static LogicalElement CreateAndInsertLogicalContainer(LogicalElement parent, int childIndex)
	{
		var document = parent.GetDocument();
		var containerElement = document.CreateComment("!");
		InsertLogicalChild(containerElement, parent, childIndex);
		return containerElement.ToLogicalElement();
	}

	// SVG elements support `foreignObject` children that can hold arbitrary HTML.
	// For these scenarios, the parent SVG and `foreignObject` elements should
	// be rendered under the SVG namespace, while the HTML content should be rendered
	// under the XHTML namespace. If the correct namespaces are not provided, most
	// browsers will fail to render the foreign object content. Here, we ensure that if
	// we encounter a `foreignObject` in the SVG, then all its children will be placed
	// under the XHTML namespace.
	public static bool IsSvgElement(LogicalElement element)
	{
		// Note: This check is intentionally case-sensitive since we expect this element
		// to appear as a child of an SVG element and SVGs are case-sensitive.
		var closestElement = (IElement)GetClosestDomElement(element);
		return closestElement.NamespaceUri == "http://www.w3.org/2000/svg"
			&& closestElement.TagName != "foreignObject";
	}

	public static INode GetClosestDomElement(LogicalElement logicalElement)
	{
		if (logicalElement.Node is IElement || logicalElement.Node is IDocumentFragment)
		{
			return logicalElement.Node;
		}
		else if (logicalElement.Node is IComment comment)
		{
			return comment.Parent!;
		}
		else
		{
			throw new InvalidOperationException("Not a valid logical element");
		}
	}

	public static void InsertLogicalChild(INode child, LogicalElement parent, int childIndex)
	{
		var childAsLogicalElement = child.ToLogicalElement();

		if (child is IComment comment)
		{
			var existingGrandchildren = GetLogicalChildrenArray(childAsLogicalElement);
			if (existingGrandchildren.Count > 0)
			{
				// There's nothing to stop us implementing support for this scenario, and it's not difficult
				// (after inserting 'child' itself, also iterate through its logical children and physically
				// put them as following-siblings in the DOM). However there's no scenario that requires it
				// presently, so if we did implement it there'd be no good way to have tests for it.
				throw new InvalidOperationException("Not implemented: inserting non-empty logical container");
			}
		}

		if (GetLogicalParent(childAsLogicalElement) is not null)
		{
			// Likewise, we could easily support this scenario too (in this 'if' block, just splice
			// out 'child' from the logical children array of its previous logical parent by using
			// Array.prototype.indexOf to determine its previous sibling index).
			// But again, since there's not currently any scenario that would use it, we would not
			// have any test coverage for such an implementation.
			throw new InvalidOperationException("Not implemented: moving existing logical children");
		}

		var newSiblings = GetLogicalChildrenArray(parent);
		if (childIndex < newSiblings.Count)
		{
			// Insert
			var nextSibling = newSiblings[childIndex]!;
			nextSibling.Node.Parent!.InsertBefore(child, nextSibling.Node);
			newSiblings.Insert(childIndex, childAsLogicalElement);
		}
		else
		{
			// Append
			AppendDomNode(child, parent);
			newSiblings.Add(childAsLogicalElement);
		}

		childAsLogicalElement.LogicalParent = parent;

		// During ctor of LogicalElement, LogicalChildren is assigned and can never be null.
		//if (childAsLogicalElement.LogicalChildren is null)
		//{
		//	childAsLogicalElement.LogicalChildren = new List<LogicalElement>();
		//}
	}

	public static void RemoveLogicalChild(LogicalElement parent, int childIndex)
	{
		var childrenArray = GetLogicalChildrenArray(parent);
		var childToRemove = childrenArray[childIndex];
		childrenArray.RemoveAt(childIndex);

		// If it's a logical container, also remove its descendants
		if (childToRemove.Node is IComment)
		{
			var grandchildrenArray = GetLogicalChildrenArray(childToRemove);
			if (grandchildrenArray is not null)
			{
				while (grandchildrenArray.Count > 0)
				{
					RemoveLogicalChild(childToRemove, 0);
				}
			}
		}

		// Finally, remove the node itself
		var domNodeToRemove = childToRemove.Node;
		domNodeToRemove.Parent!.RemoveChild(domNodeToRemove);
	}

	public static List<LogicalElement> GetLogicalChildrenArray(LogicalElement element)
		=> element.LogicalChildren;

	public static LogicalElement? GetLogicalParent(LogicalElement element)
		=> element.LogicalParent;

	public static void PermuteLogicalChildren(LogicalElement parent, List<PermutationListEntry> permutationList)
	{
		// The permutationList must represent a valid permutation, i.e., the list of 'from' indices
		// is distinct, and the list of 'to' indices is a permutation of it. The algorithm here
		// relies on that assumption.

		// Each of the phases here has to happen separately, because each one is designed not to
		// interfere with the indices or DOM entries used by subsequent phases.

		// Phase 1: track which nodes we will move
		var siblings = GetLogicalChildrenArray(parent);
		var permutationListWithTrackingData = permutationList.OfType<PermutationListEntryWithTrackingData>().ToList();
		permutationListWithTrackingData.ForEach(listEntry =>
		{
			listEntry.MoveRangeStart = siblings[listEntry.FromSiblingIndex];
			listEntry.MoveRangeEnd = FindLastDomNodeInRange(listEntry.MoveRangeStart);
		});

		// Phase 2: insert markers
		permutationListWithTrackingData.ForEach(listEntry => {
			var marker = listEntry.MoveToBeforeMarker = parent.GetDocument().CreateComment("marker");
			var insertBeforeNode = siblings[listEntry.ToSiblingIndex + 1];
			if (insertBeforeNode != null) {
				insertBeforeNode.LogicalParent.Node.InsertBefore(marker, insertBeforeNode.Node);
			} else {
				AppendDomNode(marker, parent);
			}
		});

		// Phase 3: move descendants & remove markers
		permutationListWithTrackingData.ForEach(listEntry =>
		{
			var insertBefore = listEntry.MoveToBeforeMarker!;
			var parentDomNode = insertBefore.Parent!;
			var elementToMove = listEntry.MoveRangeStart!;
			var moveEndNode = listEntry.MoveRangeEnd!;
			var nextToMove = elementToMove.Node;
			while (nextToMove != null) {
				var nextNext = nextToMove.NextSibling;
				parentDomNode.InsertBefore(nextToMove, insertBefore);

				if (nextToMove == moveEndNode) {
					break;
				} else {
					nextToMove = nextNext;
				}
			}

			parentDomNode.RemoveChild(insertBefore);
		});

		// Phase 4: update siblings index
		permutationListWithTrackingData.ForEach(listEntry => {
			siblings[listEntry.ToSiblingIndex] = listEntry.MoveRangeStart!;
		});
	}

	public static LogicalElement GetLogicalChild(LogicalElement parent, int childIndex)
		=> GetLogicalChildrenArray(parent)[childIndex];

	private static void AppendDomNode(INode child, LogicalElement parent)
	{
		// This function only puts 'child' into the DOM in the right place relative to 'parent'
		// It does not update the logical children array of anything
		if (parent.Node is IElement || parent.Node is IDocumentFragment)
		{
			parent.Node.AppendChild(child);
		}
		else if (parent.Node is IComment comment)
		{
			var parentLogicalNextSibling = GetLogicalNextSibling(parent);
			if (parentLogicalNextSibling is not null)
			{
				// Since the parent has a logical next-sibling, its appended child goes right before that
				parentLogicalNextSibling.Node.Parent!.InsertBefore(child, parentLogicalNextSibling.Node);
			}
			else
			{
				// Since the parent has no logical next-sibling, keep recursing upwards until we find
				// a logical ancestor that does have a next-sibling or is a physical element.
				AppendDomNode(child, GetLogicalParent(parent)!);
			}
		}
		else
		{
			// Should never happen
			throw new InvalidOperationException($"Cannot append node because the parent is not a valid logical element.Parent: {parent}");
		}
	}

	private static LogicalElement? GetLogicalNextSibling(LogicalElement element)
	{
		var siblings = GetLogicalChildrenArray(GetLogicalParent(element)!);
		var siblingIndex = siblings.IndexOf(element);
		return siblingIndex + 1 < siblings.Count
			? siblings[siblingIndex + 1]
			: null;
	}

	// Returns the final node (in depth-first evaluation order) that is a descendant of the logical element.
	// As such, the entire subtree is between 'element' and 'findLastDomNodeInRange(element)' inclusive.
	private static INode FindLastDomNodeInRange(LogicalElement element)
	{
		if (element.Node is IElement || element.Node is IDocumentFragment)
		{
			return element.Node;
		}

		var nextSibling = GetLogicalNextSibling(element);
		if (nextSibling != null)
		{
			// Simple case: not the last logical sibling, so take the node before the next sibling
			return nextSibling.Node.PreviousSibling;
		}
		else {
			// Harder case: there's no logical next-sibling, so recurse upwards until we find
			// a logical ancestor that does have one, or a physical element
			var logicalParent = GetLogicalParent(element)!;
			return logicalParent.Node is IElement || logicalParent.Node is IDocumentFragment
				? logicalParent.Node.LastChild
				: FindLastDomNodeInRange(logicalParent);
		}

	}

	public static IDocument GetDocument(this LogicalElement logicalElement)
		=> logicalElement.Node.Owner!; // we know that there is a document at this point.

	internal class LogicalElement
	{
		public string? DeferredValue { get; set; }

		public LogicalElement? LogicalParent { get; set; }

		public List<LogicalElement> LogicalChildren { get; } = new();

		public INode Node { get; }

		public Dictionary<string, EventHandlerInfosForElement> EventsCollections { get; } = new();

		public LogicalElement(INode node)
			=> Node = node;
	}

	internal class PermutationListEntry
	{
		public PermutationListEntry(
			int fromSiblingIndex,
			int toSiblingIndex)
		{
			FromSiblingIndex = fromSiblingIndex;
			ToSiblingIndex = toSiblingIndex;
		}

		public int FromSiblingIndex { get; set; }
		public int ToSiblingIndex { get; set; }
	}

	internal class PermutationListEntryWithTrackingData : PermutationListEntry
	{
		public PermutationListEntryWithTrackingData(
			int fromSiblingIndex,
			int toSiblingIndex,
			// These extra properties are used internally when processing the permutation list
			LogicalElement? moveRangeStart,
			INode? moveRangeEnd,
			INode? moveToBeforeMarker) : base(fromSiblingIndex, toSiblingIndex)
		{
			MoveRangeStart = moveRangeStart;
			MoveRangeEnd = moveRangeEnd;
			MoveToBeforeMarker = moveToBeforeMarker;
		}

		public LogicalElement? MoveRangeStart { get; set; }
		public INode? MoveRangeEnd { get; set; }
		public INode? MoveToBeforeMarker { get; set; }
	}
}
