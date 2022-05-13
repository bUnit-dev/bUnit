using System.Collections.Generic;
using AngleSharp.Dom;

namespace AngleSharpWrappers
{
	/// <summary>
	/// Extensions for <see cref="Wrapper{TElement}" />.
	/// </summary>
	public static class WrappedElementExtensions
    {
        /// <summary>
        /// Unwraps a wrapped AngleSharp object, if it has been wrapped.
        /// </summary>
        public static TElement Unwrap<TElement>(this TElement wrappedObject) where TElement : class, INode
            => wrappedObject is IWrapper<TElement> wrapper ? wrapper.WrappedElement : wrappedObject;

        /// <summary>
        /// Unwraps a enumerable of wrapped AngleSharp object, if they have been wrapped.
        /// </summary>
        public static IEnumerable<TElement> Unwrap<TElement>(this IEnumerable<TElement> wrappedObjects) where TElement : class, INode
        {
            if(wrappedObjects is null) yield break;

            foreach(var node in wrappedObjects)
            {
                yield return node.Unwrap();
            }
        }
    }
}
