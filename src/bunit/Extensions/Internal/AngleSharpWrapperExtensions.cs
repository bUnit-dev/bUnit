using AngleSharp.Dom;

namespace Bunit.Web.AngleSharp;

/// <summary>
/// Extensions for <see cref="IElement"/> wrapped inside <see cref="WrapperBase{TElement}" /> types.
/// </summary>
public static class AngleSharpWrapperExtensions
{
	/// <summary>
	/// Unwraps a wrapped AngleSharp object, if it has been wrapped.
	/// </summary>
	public static TElement Unwrap<TElement>(this TElement element) where TElement : class, IElement
		=> element is IElementWrapper<TElement> wrapper
			? wrapper.WrappedElement
			: element;

	/// <summary>
	/// Unwraps a enumerable of wrapped AngleSharp object, if they have been wrapped.
	/// </summary>
	public static IEnumerable<INode> Unwrap(this IEnumerable<INode> nodes)
	{
		if (nodes is null)
			yield break;

		foreach (var node in nodes)
		{
			if (node is IElement element)
				yield return element.Unwrap();
			else
				yield return node;
		}
	}
}
