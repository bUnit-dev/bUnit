using AngleSharp.Dom;

namespace Bunit.Rendering;

internal static class ComponentBoundaryNodeExtractor
{
	private const string BoundaryStartPrefix = "bl:";
	private const string BoundaryEndPrefix = "/bl:";

	internal static string StartMarkerFor(int componentId) => $"{BoundaryStartPrefix}{componentId}";

	internal static string EndMarkerFor(int componentId) => $"{BoundaryEndPrefix}{componentId}";

	internal static INodeList Extract(INodeList rootNodes, int componentId)
	{
		var startMarker = StartMarkerFor(componentId);
		var endMarker = EndMarkerFor(componentId);
		var result = new List<INode>();

		CollectNodesBetweenMarkers(rootNodes, startMarker, endMarker, result);

		return new ReadOnlyNodeList(result);
	}

	private static bool CollectNodesBetweenMarkers(
		INodeList nodes,
		string startMarker,
		string endMarker,
		List<INode> result)
	{
		for (var i = 0; i < nodes.Length; i++)
		{
			var node = nodes[i];

			if (node is IComment comment && string.Equals(comment.Data, startMarker, StringComparison.Ordinal))
			{
				for (var j = i + 1; j < nodes.Length; j++)
				{
					var sibling = nodes[j];
					if (sibling is IComment endComment && string.Equals(endComment.Data, endMarker, StringComparison.Ordinal))
					{
						return true;
					}

					if (sibling is IComment nestedMarker && IsBoundaryComment(nestedMarker))
					{
						continue;
					}

					result.Add(sibling);
				}

				return true;
			}

			if (node.HasChildNodes
				&& CollectNodesBetweenMarkers(node.ChildNodes, startMarker, endMarker, result))
			{
				return true;
			}
		}

		return false;
	}

	private static bool IsBoundaryComment(IComment comment) =>
		comment.Data.StartsWith(BoundaryStartPrefix, StringComparison.Ordinal)
		|| comment.Data.StartsWith(BoundaryEndPrefix, StringComparison.Ordinal);
}
