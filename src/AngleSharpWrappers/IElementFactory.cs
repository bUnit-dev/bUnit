using AngleSharp.Dom;

namespace AngleSharpWrappers
{
	/// <summary>
	/// Represents an <see cref="IElement"/> factory, used by a <see cref="Wrapper{TElement}"/>.
	/// </summary>
	public interface IElementFactory<out TElement> where TElement : class, INode
	{
		/// <summary>
		/// A method that returns the element to wrap.
		/// </summary>
		/// <returns></returns>
		TElement GetElement();
	}
}
