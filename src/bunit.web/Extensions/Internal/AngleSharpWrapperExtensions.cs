using AngleSharp.Dom;

namespace Bunit;

/// <summary>
/// Extensions for <see cref="IElement"/> wrapped inside <see cref="Web.AngleSharp.WrapperBase{TElement}" /> types.
/// </summary>
public static class AngleSharpWrapperExtensions
{	
	/// <summary>
	/// Unwraps a wrapped AngleSharp object, if it has been wrapped.
	/// </summary>
	public static TElement Unwrap<TElement>(this TElement element) where TElement : class, INode
		=> element is Bunit.Web.AngleSharp.IElementWrapper<TElement> wrapper
			? wrapper.WrappedElement
			: element;

	/// <summary>
	/// Unwraps a enumerable of wrapped AngleSharp object, if they have been wrapped.
	/// </summary>
	public static IEnumerable<INode> Unwrap(this IEnumerable<INode> elements)
	{
		if (elements is null)
			yield break;

		foreach (var element in elements)
		{
			yield return element.Unwrap();
		}
	}
}
