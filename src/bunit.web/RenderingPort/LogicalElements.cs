// This file is a port of the LogicalElements.ts in https://github.dev/dotnet/aspnetcore
// Version ported: https://github.dev/dotnet/aspnetcore/blob/82eb6803e777e2e6c817b51ac6b3bcb5410b4236/src/Components/Web.JS/src/Rendering/LogicalElements.ts
using System.Runtime.CompilerServices;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Dom.Events;

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

	private static List<LogicalElement> GetLogicalChildrenArray(LogicalElement element)
		=> element.LogicalChildren;

	private static LogicalElement? GetLogicalParent(LogicalElement element)
		=> element.LogicalParent;

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

	public static IDocument GetDocument(this LogicalElement logicalElement)
		=> logicalElement.Node.Owner!; // we know that there is a document at this point.

	internal class LogicalElement
	{
		public string? DeferredValue { get; set; }

		public LogicalElement? LogicalParent { get; set; }

		public List<LogicalElement> LogicalChildren { get; } = new();

		public INode Node { get; }

		public LogicalElement(INode node)
			=> Node = node;		
	}

	internal record class PermutationListEntry(
		int FromSiblingIndex,
		int ToSiblingIndex);

	internal record class PermutationListEntryWithTrackingData(
		int FromSiblingIndex,
		int ToSiblingIndex,
		// These extra properties are used internally when processing the permutation list
		LogicalElement? MoveRangeStart,
		INode? MoveRangeEnd,
		INode? MoveToBeforeMarker)
		: PermutationListEntry(FromSiblingIndex, ToSiblingIndex);
}
