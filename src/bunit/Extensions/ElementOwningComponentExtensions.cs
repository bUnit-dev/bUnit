using AngleSharp.Dom;
using Bunit.Rendering;
using Bunit.Web.AngleSharp;

namespace Bunit;

/// <summary>
/// Extension methods for getting the component that rendered a DOM node.
/// </summary>
public static class ElementOwningComponentExtensions
{
	/// <summary>
	/// Gets the component that rendered the <paramref name="node"/>.
	/// </summary>
	/// <param name="node">The node to find the rendering component for.</param>
	/// <exception cref="InvalidOperationException">
	/// Thrown when the <paramref name="node"/> cannot be traced back to a component,
	/// e.g. because it was not rendered by a <see cref="BunitRenderer"/>.
	/// </exception>
	public static IRenderedComponent<IComponent> GetOwningComponent(this INode node)
	{
		ArgumentNullException.ThrowIfNull(node);

		var renderer = node.GetBunitContext()?.Renderer
			?? throw new InvalidOperationException(
				$"The node was not rendered by bUnit's '{nameof(BunitRenderer)}', so the component that rendered it is unknown.");

		var element = FindElementWithSourceReference(node)
			?? throw new InvalidOperationException(
				"The node cannot be traced back to a component, because neither it nor any of its ancestors was parsed from the markup of a component.");

		var (root, sourceIndexOffset) = FindRenderTreeRootFor(renderer, element.Owner);

		var snapshot = root.MarkupSnapshot
			?? throw new InvalidOperationException("The render tree the node belongs to has no markup.");

		var sourceIndex = RootMarkupSnapshot.GetSourceIndex(element) + sourceIndexOffset;

		foreach (var range in snapshot.Ranges)
		{
			if (!range.Contains(sourceIndex))
			{
				continue;
			}

			var candidate = renderer.GetRenderedComponent(range.ComponentId);

			// Ranges are in post-order, so the first match is the innermost component.
			if (candidate.Instance is not BunitRootComponent)
			{
				return candidate;
			}
		}

		throw new InvalidOperationException("The node cannot be traced back to a component.");
	}

	/// <summary>
	/// Gets the closest <typeparamref name="TComponent"/> that the <paramref name="node"/>
	/// was rendered inside of.
	/// </summary>
	/// <typeparam name="TComponent">The type of component to find.</typeparam>
	/// <param name="node">The node to find the rendering component for.</param>
	/// <exception cref="ComponentNotFoundException">
	/// Thrown when the <paramref name="node"/> was not rendered inside a <typeparamref name="TComponent"/>.
	/// </exception>
	public static IRenderedComponent<TComponent> GetOwningComponent<TComponent>(this INode node)
		where TComponent : IComponent
	{
		var owner = node.GetOwningComponent();

		for (var candidate = owner; candidate is not null; candidate = candidate.Parent())
		{
			if (candidate.Instance is TComponent)
			{
				return (IRenderedComponent<TComponent>)candidate;
			}
		}

		throw new ComponentNotFoundException(typeof(TComponent));
	}

	/// <summary>
	/// Gets the root of the render tree the <paramref name="document"/> was parsed for, and the
	/// offset that rebases a source index in it onto the root's markup.
	/// </summary>
	/// <remarks>
	/// A private document holds one component's markup, so its indices start at zero rather
	/// than at that component's position in the root's markup.
	/// </remarks>
	private static (IRenderedComponentRoot Root, int SourceIndexOffset) FindRenderTreeRootFor(BunitRenderer renderer, IDocument? document)
	{
		if (renderer.FindRenderTreeRoot(document) is { } sharedRoot)
		{
			return (sharedRoot, 0);
		}

		if (renderer.FindPrivateDocumentOwner(document) is { } owner
			&& owner.Root.MarkupSnapshot?.TryGetRange(owner.ComponentId, out var range) == true)
		{
			return (owner.Root, range.Start);
		}

		throw new InvalidOperationException(
			"The node cannot be traced back to a component, because it does not belong to the document of a rendered component tree.");
	}

	private static IElement? FindElementWithSourceReference(INode node)
	{
		var candidate = node is IElement element
			? element.Unwrap()
			: node.ParentElement;

		while (candidate is not null)
		{
			if (RootMarkupSnapshot.GetSourceIndex(candidate) >= 0)
			{
				return candidate;
			}

			candidate = candidate.ParentElement;
		}

		return null;
	}
}
