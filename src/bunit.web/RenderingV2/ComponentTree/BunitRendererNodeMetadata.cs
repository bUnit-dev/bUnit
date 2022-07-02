using System.Runtime.CompilerServices;
using AngleSharp.Dom;
using Bunit.RenderingV2.AngleSharp;

namespace Bunit.RenderingV2.ComponentTree;

public static class BunitRendererNodeMetadata
{
	private static readonly ConditionalWeakTable<INode, NodeMeta> MetadataStorage
		= new ConditionalWeakTable<INode, NodeMeta>();

	private sealed record class NodeMeta
	{
		public int FrameIndex { get; set; }

		public ComponentTreeNode Owner { get; }

		public NodeMeta(int frameIndex, ComponentTreeNode owner)
		{
			FrameIndex = frameIndex;
			Owner = owner;
		}
	}

	private static NodeMeta GetMetadataOrThrow(INode node)
	{
		if (!MetadataStorage.TryGetValue(node, out var meta))
			throw new MissingNodeMetadataException();
		return meta;
	}

	private static int GetNextFrameIndex(INode parent)
	{
		if (parent.ChildNodes.Length == 0)
			return 0;

		var lastNode = parent.ChildNodes[parent.ChildNodes.Length - 1];
		var lastNodeMeta = GetMetadataOrThrow(lastNode);
		return lastNodeMeta.FrameIndex + 1;
	}

	internal static ComponentTreeNode GetOwningComponent(this INode node)
	{
		var meta = GetMetadataOrThrow(node);
		return meta.Owner;
	}

	/// <summary>
	/// Appends a single child <paramref name="node"/> at the end of the <paramref name="parent"/>'s
	/// <see cref="INode.ChildNodes"/> collection.
	/// The <paramref name="node"/> will automatically get assigned a frame index based on
	/// it's new sibling.
	/// </summary>
	/// <param name="parent"></param>
	/// <param name="node"></param>
	/// <param name="owner"></param>
	internal static void AppendChildFrame(this INode parent, INode node, ComponentTreeNode owner)
	{
		MetadataStorage.Add(node, new(GetNextFrameIndex(parent), owner));
		parent.AppendChild(node);
	}

	/// <summary>
	/// Appends a child <paramref name="nodes"/> at the end of the <paramref name="parent"/>'s
	/// <see cref="INode.ChildNodes"/> collection.
	/// The <paramref name="nodes"/> will automatically get assigned a frame index based on
	/// their new sibling.
	/// </summary>
	/// <remarks>
	/// NOTE: The nodes in the <paramref name="nodes"/> list
	/// are removed from the list when added to <paramref name="parent"/>.
	/// </remarks>
	/// <param name="parent"></param>
	/// <param name="nodes"></param>
	/// <param name="owner"></param>
	internal static void AppendChildFrame(this INode parent, INodeList nodes, ComponentTreeNode owner)
	{
		var meta = new NodeMeta(GetNextFrameIndex(parent), owner);

		foreach (var node in nodes)
		{
			MetadataStorage.Add(node, meta);
		}

		parent.AppendNodes(nodes);
	}

	/// <summary>
	/// Removes all children from the specified <paramref name="frameIndex"/>,
	/// stops tracking them.
	/// </summary>
	/// <remarks>
	///	Removing a frame (child nodes) shifts the <paramref name="frameIndex"/> of
	///	later siblings in the parents ChildNodes collection up one number.
	///	E.g. if removing a frame with index 1, then frame with index 2 will
	///	get its index updated to 1.
	/// </remarks>
	/// <param name="parent">The parent node to remove the child nodes from.</param>
	/// <param name="frameIndex">The frame index which determine what nodes to remove.</param>
	internal static void RemoveChildFrame(this INode parent, int frameIndex)
	{
		// Since frame index's are continuous increasing numbers,
		// and each frame is minimum one node big, the first
		// candidate cannot be earlier in the parent.ChildNodes
		// collection than the frameIndex itself.
		var candidateIndex = frameIndex;
		var candidate = parent.ChildNodes[candidateIndex];
		var candidateMeta = GetMetadataOrThrow(candidate);

		while (candidateMeta.FrameIndex != frameIndex)
		{
			candidateIndex++;
			candidate = parent.ChildNodes[candidateIndex];
			candidateMeta = GetMetadataOrThrow(candidate);
		}

		// Remove all nodes in frame
		while (candidateMeta.FrameIndex == frameIndex)
		{
			parent.RemoveChild(candidate);
			MetadataStorage.Remove(candidate);

			// Get next child node to see if it is inside the
			// frame that is to be removed.
			if (parent.ChildNodes.Length > candidateIndex)
			{
				candidate = parent.ChildNodes[candidateIndex];
				candidateMeta = GetMetadataOrThrow(candidate);
			}
			else
			{
				break;
			}
		}

		// Shift all frame indexes up by one from the last found
		// and removed child node to the end.
		parent.ShiftFrameIndexFrom(candidateIndex);
	}

	/// <summary>
	/// Shift all frame indexes up by one from specified <paramref name="startIndex"/>.
	/// </summary>
	private static void ShiftFrameIndexFrom(this INode parent, int startIndex)
	{
		for (int i = startIndex; i < parent.ChildNodes.Length; i++)
		{
			var node = parent.ChildNodes[i];
			var meta = GetMetadataOrThrow(node);
			MetadataStorage.AddOrUpdate(
				node,
				meta with
				{
					FrameIndex = meta.FrameIndex - 1
				});
		}
	}

	/// <summary>
	/// Get the first node from <paramref name="parent"/>
	/// in frame with the specified <paramref name="frameIndex"/>.
	/// </summary>
	internal static INode GetChildNodeByFrameIndex(this INode parent, int frameIndex)
	{
		// Since frame index's are continuous increasing numbers,
		// and each frame is minimum one node big, the first
		// candidate cannot be earlier in the parent.ChildNodes
		// collection than the frameIndex itself.
		var candidateIndex = frameIndex;
		var candidate = parent.ChildNodes[candidateIndex];
		var meta = GetMetadataOrThrow(candidate);

		while (meta.FrameIndex != frameIndex)
		{
			candidateIndex++;
			candidate = parent.ChildNodes[candidateIndex];
			meta = GetMetadataOrThrow(candidate);
		}

		return candidate;
	}
}
